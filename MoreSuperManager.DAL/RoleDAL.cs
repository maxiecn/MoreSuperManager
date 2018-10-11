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
            parameterList.Add("@FieldSql", "IdentityID, RoleName, ChannelCode, (select ChannelName from T_Channel with(nolock) where T_Channel.ChannelCode=T.ChannelCode) as ChannelName");
            parameterList.Add("@Field", "IdentityID, RoleName, ChannelCode");
            parameterList.Add("@TableName", "T_Role");
            parameterList.Add("@PrimaryKey", "IdentityID");
            parameterList.Add("@PageIndex", pageIndex);
            parameterList.Add("@PageSize", pageSize);
            parameterList.Add("@WhereSql", whereSql);
            parameterList.Add("@OrderSql", "IdentityID desc");

            return DataBaseHelper.ToEntityList<DBRoleFullModel>("", parameterList, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
