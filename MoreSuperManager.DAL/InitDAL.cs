using Helper.Core.Library;
using MoreSuperManager.ENUM;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class InitDAL
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="userCode">用户名</param>
        /// <param name="nickName">登录名</param>
        /// <param name="userPassword">密码</param>
        /// <param name="directoryPath">初始化配置所在目录</param>
        /// <returns></returns>
        public bool Init(string userCode, string nickName, string userPassword, string directoryPath)
        {
            int count = DataBaseHelper.ExecuteScalar<int>("select count(1) from T_User with(nolock)");
            if (count > 0) return false;

            if (!System.IO.Directory.Exists(directoryPath)) return false;

            return (bool)DataBaseHelper.Transaction((con, transaction) =>
            {
                #region 添加频道信息
                // 读取初始化频道数据
                Dictionary<string, string> channelDict = TxtHelper.ToDict<string, string>(directoryPath + "Channels.txt", ",");
                if (channelDict != null && channelDict.Count > 0)
                {
                    List<DBChannelModel> channelModelList = new List<DBChannelModel>();
                    foreach (KeyValuePair<string, string> keyValueItem in channelDict)
                    {
                        channelModelList.Add(new DBChannelModel() { ChannelCode = keyValueItem.Key, ChannelName = keyValueItem.Value, ChannelSort = 0 });
                    }
                    DataBaseHelper.TransactionEntityListBatchImport<DBChannelModel>(con, transaction, "T_Channel", channelModelList);
                }
                #endregion

                #region 添加角色信息
                // 读取初始化角色名
                string roleName = TxtHelper.Read(directoryPath + "Roles.txt");
                // 向角色表添加数据
                int roleID = DataBaseHelper.TransactionScalar<int>(con, transaction, "insert into T_Role(RoleName, ChannelCode, MenuList, ActionList)values(@RoleName, @ChannelCode, @MenuList, @ActionList);select SCOPE_IDENTITY();", new DBRoleModel() { RoleName = roleName, ChannelCode = ChannelCodeTypeEnum.ALL, ActionList = "", MenuList = "" });
                #endregion

                #region 添加用户信息
                // 向用户添加数据
                bool result = DataBaseHelper.TransactionNonQuery(con, transaction, "insert into T_User(UserCode, NickName, UserPassword, RoleID)values(@UserCode, @NickName, @UserPassword, @RoleID)", new { UserCode = userCode, NickName = nickName, UserPassword = userPassword, RoleID = roleID }) > 0;
                #endregion

                #region 添加权限数据
                // 读取初始化权限数据
                Dictionary<string, string> actionTypeDict = TxtHelper.ToDict<string, string>(directoryPath + "Actions.txt", ",");
                if (actionTypeDict != null && actionTypeDict.Count > 0)
                {
                    List<DBActionTypeModel> actionTypeModelList = new List<DBActionTypeModel>();
                    foreach (KeyValuePair<string, string> keyValueItem in actionTypeDict)
                    {
                        actionTypeModelList.Add(new DBActionTypeModel() { TypeCode = keyValueItem.Key, TypeName = keyValueItem.Value, TypeSort = 0 });
                    }
                    DataBaseHelper.TransactionEntityListBatchImport<DBActionTypeModel>(con, transaction, "T_ActionType", actionTypeModelList);
                }
                #endregion

                #region 添加模块数据
                // 读取模块权限数据
                Dictionary<string, string> moduleActionDict = TxtHelper.ToDict<string, string>(directoryPath + "ModuleActions.txt", ":");
                // 读取模块数据
                Dictionary<string, string> moduleDict = TxtHelper.ToDict<string, string>(directoryPath + "Modules.txt", ",");

                // 添加 ModuleList
                List<DBModuleModel> moduleModelList = new List<DBModuleModel>();
                foreach (KeyValuePair<string, string> keyValueItem in moduleDict)
                {
                    moduleModelList.Add(new DBModuleModel() { ChannelCode = ChannelCodeTypeEnum.ALL, ModuleCode = keyValueItem.Key, ModuleName = keyValueItem.Value, ActionList = moduleActionDict.ContainsKey(keyValueItem.Key) ? moduleActionDict[keyValueItem.Key] : "" });
                }
                DataBaseHelper.TransactionEntityListBatchImport<DBModuleModel>(con, transaction, "T_Module", moduleModelList);
                #endregion

                #region 添加菜单数据，只支持二级菜单，二级以上菜单需要初始化进入后台之后再手动调整

                List<ViewInitMenuModel> menuModelList = new List<ViewInitMenuModel>();

                // 读取菜单数据
                List<ViewInitMenuItemModel> menuItemModelList = TxtHelper.ToEntityList<ViewInitMenuItemModel>(directoryPath + "Menus.txt", ",");
                // 读取根级菜单数据
                List<ViewInitMenuItemModel> rootMenuItemModelList = menuItemModelList.Where(p => string.IsNullOrEmpty(p.ParentCode) || p.ParentCode == "-1").ToList();

                foreach (ViewInitMenuItemModel rootMenuItemModel in rootMenuItemModelList)
                {
                    ViewInitMenuModel menuModel = new ViewInitMenuModel()
                    {
                        TrunkMenu = new DBMenuModel() { ParentID = 0, ChannelCode = ChannelCodeTypeEnum.ALL, MenuName = rootMenuItemModel.MenuName, BelongModule = "-1", MenuIcon = rootMenuItemModel.MenuIcon, MenuUrl = "", ActionList = "", MenuSort = 0 },
                        NodeMenuList = new List<DBMenuModel>() { }
                    };
                    List<ViewInitMenuItemModel> nodeMenuItemModelList = menuItemModelList.Where(p => p.ParentCode == rootMenuItemModel.MenuCode).ToList();
                    if (nodeMenuItemModelList != null && nodeMenuItemModelList.Count > 0)
                    {
                        foreach (ViewInitMenuItemModel nodeMenuItemModel in nodeMenuItemModelList)
                        {
                            menuModel.NodeMenuList.Add(new DBMenuModel() { ChannelCode = ChannelCodeTypeEnum.ALL, MenuName = nodeMenuItemModel.MenuName, MenuUrl = nodeMenuItemModel.MenuUrl, BelongModule = nodeMenuItemModel.MenuCode, ActionList = moduleActionDict.ContainsKey(nodeMenuItemModel.MenuCode) ? moduleActionDict[nodeMenuItemModel.MenuCode] : "", ParentID = 0, MenuIcon = "", MenuSort = 0 });
                        }
                    }
                    menuModelList.Add(menuModel);
                }
                foreach (ViewInitMenuModel menuItem in menuModelList)
                {
                    DBMenuModel menuModel = menuItem.TrunkMenu;
                    int menuID = DataBaseHelper.TransactionScalar<int>(con, transaction, "insert into T_Menu(ChannelCode, ParentID, MenuName, MenuUrl, BelongModule, ActionList, MenuSort, MenuIcon)values(@ChannelCode, @ParentID, @MenuName, @MenuUrl, @BelongModule, @ActionList, @MenuSort, @MenuIcon);select SCOPE_IDENTITY();", new { menuModel.ChannelCode, menuModel.ParentID, menuModel.MenuName, menuModel.MenuIcon, menuModel.BelongModule, menuModel.MenuUrl, menuModel.ActionList, menuModel.MenuSort });
                    for (int menuIndex = 0; menuIndex < menuItem.NodeMenuList.Count; menuIndex++)
                    {
                        menuItem.NodeMenuList[menuIndex].ParentID = menuID;
                    }
                    DataBaseHelper.TransactionEntityListBatchImport<DBMenuModel>(con, transaction, "T_Menu", menuItem.NodeMenuList);
                }

                List<DBMenuModel> menuDataList = DataBaseHelper.TransactionEntityList<DBMenuModel>(con, transaction, "select IdentityID from T_Menu with(nolock)");
                string menuIDList = StringHelper.PadChar(StringHelper.ToString<int>(menuDataList.Select(p => p.IdentityID).ToList(), ","));

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(",");
                foreach (KeyValuePair<string, string> keyValueItem in moduleActionDict)
                {
                    List<string> menuActionDataList = StringHelper.ToList<string>(keyValueItem.Value, ",", true);
                    foreach (string menuActionData in menuActionDataList)
                    {
                        stringBuilder.Append(string.Format("{0}:{1},", keyValueItem.Key, menuActionData));
                    }
                }
                #endregion

                #region 更新角色数据
                DataBaseHelper.TransactionNonQuery(con, transaction, "update T_Role set MenuList=@MenuList, ActionList=@ActionList where IdentityID=@IdentityID", new { MenuList = menuIDList, ActionList = stringBuilder.ToString(), IdentityID = roleID });
                #endregion

                return true;
            });
        }
    }
}
