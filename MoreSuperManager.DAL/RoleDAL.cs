using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class RoleDAL
    {
        private const string TABLE_NAME = "T_Role";

        public bool Operater(DBRoleModel model, string menuList, string actionList)
        {
            model.ActionList = StringHelper.PadChar(actionList, ","); ;
            model.MenuList = StringHelper.PadChar(menuList, ",");

            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBRoleModel>(model, p => p.IdentityID, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBRoleModel>(model, p => p.IdentityID == p.IdentityID, p => p.IdentityID, TABLE_NAME);
            }
        }
        public bool Exists(string channelCode, string roleName, int identityID)
        {
            return DataBaseHelper.Exists<DBRoleModel>(new { RoleName = roleName, ChannelCode = channelCode }, p => p.IdentityID, p => p.RoleName == p.RoleName && p.ChannelCode == channelCode, identityID, TABLE_NAME);
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBRoleModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBRoleModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public bool Clone(int sourceRoleID, int targetRoleID)
        {
            DBRoleModel sourceRoeModel = DALFactory.Role.Select(sourceRoleID);
            DBRoleModel targetRoleModel = DALFactory.Role.Select(targetRoleID);
            if (sourceRoeModel == null || targetRoleModel == null) return false;

            List<DBModuleModel> moduleModelList = DALFactory.Module.CloneList(sourceRoeModel.ChannelCode);
            List<DBMenuModel> menuModelList = DALFactory.Menu.CloneList(sourceRoeModel.ChannelCode);

            return (bool)DataBaseHelper.Transaction((con, transaction) =>
            {
                DataBaseHelper.TransactionEntityListBatchImport<DBModuleModel>(con, transaction, "T_Module", moduleModelList);
                DataBaseHelper.TransactionEntityListBatchImport<DBMenuModel>(con, transaction, "T_Menu", menuModelList);
                return DataBaseHelper.Update<DBRoleModel>(new { MenuList = sourceRoeModel.MenuList, ActionList = sourceRoeModel.ActionList, IdentityID = targetRoleModel.IdentityID }, p => p.IdentityID == p.IdentityID, p => new { p.ChannelCode, p.RoleName }, TABLE_NAME);
            });
        }
        public DBRoleModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBRoleModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.RoleName, p.MenuList, p.ActionList, p.ChannelCode }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public List<DBRoleModel> List()
        {
            return DataBaseHelper.More<DBRoleModel>(null, p => new { p.IdentityID, p.RoleName, p.ChannelCode }, null, null, true, TABLE_NAME);
        }

        public List<DBRoleModel> ChannelList(string channelCode)
        {
            return DataBaseHelper.More<DBRoleModel>(new { ChannelCode = channelCode }, p => new { p.IdentityID, p.RoleName }, p => p.ChannelCode == channelCode, null, true, TABLE_NAME);
        }

        public List<DBRoleFullModel> Page(string channelCode, string searchKey, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            channelCode = StringHelper.FilterSpecChar(channelCode);
            searchKey = StringHelper.FilterSpecChar(searchKey);

            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(channelCode) && channelCode != "-2")
            {
                stringBuilder.Append(" ChannelCode = '");
                stringBuilder.Append(channelCode);
                stringBuilder.Append("' and ");
            }
            if (!string.IsNullOrEmpty(searchKey))
            {
                stringBuilder.Append(" RoleName like '%");
                stringBuilder.Append(searchKey);
                stringBuilder.Append("%' ");
            }
            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });

            Dictionary<string, object> parameterList = new Dictionary<string, object>();
            parameterList.Add(DataBaseParameterEnum.FieldSql, "IdentityID, RoleName, ChannelCode, (select ChannelName from T_Channel with(nolock) where T_Channel.ChannelCode=T.ChannelCode) as ChannelName");
            parameterList.Add(DataBaseParameterEnum.Field, "IdentityID, RoleName, ChannelCode");
            parameterList.Add(DataBaseParameterEnum.TableName, "T_Role");
            parameterList.Add(DataBaseParameterEnum.PrimaryKey, "IdentityID");
            parameterList.Add(DataBaseParameterEnum.PageIndex, pageIndex);
            parameterList.Add(DataBaseParameterEnum.PageSize, pageSize);
            parameterList.Add(DataBaseParameterEnum.WhereSql, whereSql);
            parameterList.Add(DataBaseParameterEnum.OrderSql, "IdentityID desc");
            parameterList.Add(DataBaseParameterEnum.JoinSql, "");

            return DataBaseHelper.ToEntityList<DBRoleFullModel>("", parameterList, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
