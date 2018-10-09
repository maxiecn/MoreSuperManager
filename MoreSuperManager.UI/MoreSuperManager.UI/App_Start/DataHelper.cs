using Helper.Core.Library;
using MoreSuperManager.ENUM;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MoreSuperManager.UI
{
    public class DataHelper
    {
        /// <summary>
        /// 锁定操作
        /// </summary>
        private static readonly object lockItem = new object();
        /// <summary>
        /// 角色菜单数据
        /// KEY：为角色编号
        /// VALUE：为菜单列表
        /// </summary>
        private static Dictionary<string, List<DBMenuModel>> AuthMenuModelDict = null;
        /// <summary>
        /// 角色菜单权限（验证所有标了 RoleMenuFilter 特性的 Action）
        /// 第一个 KEY：角色编号
        /// 第二个 KEY：菜单 URL
        /// 第三个 KEY 和 VALUE：菜单 URL 参数名称和值
        /// </summary>
        private static Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> AuthMenuDict = null;
        /// <summary>
        /// 角色操作权限（验证所有标了 RoleActionFilter 特性的 Action）
        /// 第一个 KEY：角色编号
        /// 第二个 KEY：Controller 名称
        /// 第三个 KEY 和 VALUE：Action 名称
        /// </summary>
        private static Dictionary<string, Dictionary<string, Dictionary<string, string>>> AuthActionDict = null;

        #region 验证菜单或者操作是否存在列表中
        /// <summary>
        /// 判断菜单编号是否存在于菜单编号列表字符串中
        /// </summary>
        /// <param name="identityID">菜单编号</param>
        /// <param name="menuList">菜单编号列表</param>
        /// <returns></returns>
        public static bool ExistsMenu(int identityID, string menuList)
        {
            if (string.IsNullOrEmpty(menuList)) return false;
            return menuList.IndexOf(string.Format(",{0},", identityID)) >= 0;
        }
        /// <summary>
        /// 判断动作编号是否存在于动作编号列表字符串中
        /// </summary>
        /// <param name="actionCode">动作编号</param>
        /// <param name="moduleCode">所属模块（Controller）</param>
        /// <param name="actionList">动作编号列表</param>
        /// <returns></returns>
        public static bool ExistsAction(string actionCode, string moduleCode, string actionList)
        {
            if (string.IsNullOrEmpty(actionList)) return false;

            return actionList.IndexOf(string.Format(",{0}:{1},", moduleCode, actionCode)) >= 0;
        }
        #endregion

        /// <summary>
        /// 初始化角色菜单和动作权限验证数据，在登录或者设置角色权限时调用
        /// </summary>
        /// <param name="roleID">角色编号</param>
        /// <param name="modelList">菜单列表</param>
        /// <param name="actionList">动作列表</param>
        /// <param name="moduleList">模块列表</param>
        public static void InitRoleMenuAndActionData(string roleID, List<DBMenuModel> menuList, string actionList, List<DBModuleModel> moduleList)
        {
            if (AuthMenuModelDict == null)
            {
                AuthMenuModelDict = new Dictionary<string, List<DBMenuModel>>();
            }
            if (AuthMenuDict == null)
            {
                AuthMenuDict = new Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>>();
            }
            if (AuthActionDict == null)
            {
                AuthActionDict = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
            }
            // 设置角色菜单信息
            AuthMenuModelDict[roleID] = menuList;

            Dictionary<string, string> ModuleDict = new Dictionary<string, string>();
            if (moduleList != null && moduleList.Count > 0)
            {
                foreach (DBModuleModel moduleModel in moduleList)
                {
                    ModuleDict.Add(moduleModel.ModuleCode.ToLower(), moduleModel.ActionList.ToLower());
                }
            }

            if (menuList == null || menuList.Count == 0) return;
            // 锁定对象
            lock (lockItem)
            {
                #region 初始化菜单数据
                // 设置角色菜单
                AuthMenuDict[roleID] = new Dictionary<string, List<Dictionary<string, string>>>();
                // 遍历菜单数据
                foreach (DBMenuModel model in menuList)
                {
                    string menuUrl = null;
                    Dictionary<string, string> paramDict = null;

                    // 如果访问地址不为空
                    if (!string.IsNullOrEmpty(model.MenuUrl))
                    {
                        menuUrl = model.MenuUrl.ToLower();
                        // 如果地址中包含 ? 符号
                        if (menuUrl.IndexOf("?") > 0)
                        {
                            // 获取前缀
                            StringKeyValueData<string, string> keyValueData = StringHelper.ToKeyValueData<string, string>(menuUrl, "?");
                            menuUrl = keyValueData.Key;
                            // 获取参数
                            paramDict = StringHelper.ToDict<string, string>(keyValueData.Value.ToLower(), "&", "=");
                        }
                        if (!AuthMenuDict[roleID].ContainsKey(menuUrl))
                        {
                            AuthMenuDict[roleID][menuUrl] = new List<Dictionary<string, string>>();
                        }
                        // 设置数据内容
                        AuthMenuDict[roleID][menuUrl].Add(paramDict);
                    }
                }
                #endregion

                #region 初始化动作数据
                // 设置角色动作
                AuthActionDict[roleID] = new Dictionary<string, Dictionary<string, string>>();
                if (!string.IsNullOrEmpty(actionList))
                {
                    List<StringKeyValueData<string, string>> actionDataList = StringHelper.ToKeyValueList<string, string>(actionList, ",", ":", StringCaseTypeEnum.Lower);
                    if (actionDataList != null && actionDataList.Count > 0)
                    {
                        foreach (StringKeyValueData<string, string> actionData in actionDataList)
                        {
                            if (!ModuleDict.ContainsKey(actionData.Key)) continue;

                            if (!AuthActionDict[roleID].ContainsKey(actionData.Key))
                            {
                                AuthActionDict[roleID][actionData.Key] = new Dictionary<string, string>();
                            }
                            if (IsContains(actionData.Value, ModuleDict[actionData.Key]))
                            {
                                if (!AuthActionDict[roleID][actionData.Key].ContainsKey(actionData.Value))
                                {
                                    AuthActionDict[roleID][actionData.Key].Add(actionData.Value, actionData.Value);
                                }
                            }
                        }
                    }
                }
                #endregion
            }
        }
        /// <summary>
        /// 根据角色编号获取菜单列表
        /// </summary>
        /// <param name="roleID">角色编号</param>
        /// <returns></returns>
        public static List<DBMenuModel> GetRoleMenuModelList(string roleID)
        {
            if (AuthMenuModelDict == null || !AuthMenuModelDict.ContainsKey(roleID)) return null;
            return AuthMenuModelDict[roleID];
        }

        #region 验证角色菜单访问权限
        /// <summary>
        /// 验证角色菜单访问权限
        /// </summary>
        /// <param name="roleID">角色编号</param>
        /// <param name="menuUrl">访问的 URL 路径</param>
        /// <param name="paramDict">访问的 URL 参数字典</param>
        /// <returns></returns>
        public static bool AuthMenu(string roleID, string menuUrl, IDictionary<string, object> paramDict)
        {
            // 如果角色数据不存在
            if (AuthMenuDict == null || !AuthMenuDict.ContainsKey(roleID)) return false;
            // 如果菜单数据不存在
            if (!AuthMenuDict[roleID].ContainsKey(menuUrl)) return false;

            foreach (Dictionary<string, string> paramItem in AuthMenuDict[roleID][menuUrl])
            {
                if (AuthMenuItem(paramItem, paramDict)) return true;
            }
            return false;
        }
        /// <summary>
        /// 验证 URL 参数值是否匹配
        /// </summary>
        /// <param name="paramDict"></param>
        /// <param name="paramValueDict"></param>
        /// <returns></returns>
        private static bool AuthMenuItem(Dictionary<string, string> paramDict, IDictionary<string, object> paramValueDict)
        {
            if (paramDict == null || paramDict.Count == 0) return true;

            foreach (KeyValuePair<string, string> keyValueItem in paramDict)
            {
                if (!paramValueDict.ContainsKey(keyValueItem.Key) || paramValueDict[keyValueItem.Key].ToString() != keyValueItem.Value) return false;
            }
            return true;
        }
        #endregion

        #region 验证角色操作权限
        /// <summary>
        /// 验证角色动作操作权限
        /// </summary>
        /// <param name="roleID">角色编号</param>
        /// <param name="controller">Controller 名称</param>
        /// <param name="action">Action 名称</param>
        /// <returns></returns>
        public static bool AuthAction(string roleID, string controllerName, string actionName)
        {
            // 如果未开启权限验证
            if (!SettingHelper.AuthOpenStatus) return true;

            if (!string.IsNullOrEmpty(controllerName)) controllerName = controllerName.ToLower();
            if (!string.IsNullOrEmpty(actionName)) actionName = actionName.ToLower();

            if (AuthActionDict == null || !AuthActionDict.ContainsKey(roleID)) return false;
            if (!AuthActionDict[roleID].ContainsKey(controllerName)) return false;
            // 验证授权
            return AuthActionDict[roleID][controllerName].ContainsKey(actionName);
        }
        #endregion

        /// <summary>
        /// 获取模块下的 Action 列表数据
        /// </summary>
        /// <param name="moduleCode">模块名称</param>
        /// <param name="moduleList">模块列表数据</param>
        /// <param name="actionTypeList">Action 列表数据</param>
        /// <returns></returns>
        public static List<DBActionTypeModel> GetModuleActionList(string moduleCode, List<DBModuleModel> moduleList, List<DBActionTypeModel> actionTypeList)
        {
            DBModuleModel moduleItem = moduleList.Where(p => p.ModuleCode == moduleCode).FirstOrDefault();
            if (moduleItem == null) return null;

            return GetActionList(moduleItem.ActionList, actionTypeList);
        }
        /// <summary>
        /// 获取 Action 列表数据
        /// </summary>
        /// <param name="actionList">动作编号列表字符串</param>
        /// <param name="actionTypeList">Action 列表数据</param>
        /// <returns></returns>
        public static List<DBActionTypeModel> GetActionList(string actionList, List<DBActionTypeModel> actionTypeList)
        {
            if (string.IsNullOrEmpty(actionList)) return null;

            List<DBActionTypeModel> resultList = new List<DBActionTypeModel>();

            List<string> moduleActionList = StringHelper.ToList<string>(actionList, ",");
            foreach (string moduleAction in moduleActionList)
            {
                DBActionTypeModel typeItem = actionTypeList.Where(p => p.TypeCode == moduleAction).FirstOrDefault();
                if (typeItem != null) resultList.Add(typeItem);
            }

            return resultList;
        }
        /// <summary>
        /// 判断 Key 是否在 Data 中
        /// </summary>
        /// <param name="key">Key 数据</param>
        /// <param name="data">Key 用逗号分隔的数据</param>
        /// <returns></returns>
        public static bool IsContains(string key, string data)
        {
            if (string.IsNullOrEmpty(data)) return false;

            List<string> dataList = data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return dataList.IndexOf(key) >= 0;
        }
    }
}