using Helper.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreSuperManager.MODEL;

namespace MoreSuperManager.DAL
{
    public class LinkFriendTypeDAL
    {
        private const string TABLE_NAME = "T_LinkFriendType";

        public bool Operater(DBLinkFriendTypeModel model)
        {
            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBLinkFriendTypeModel>(model, p => p.IdentityID, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBLinkFriendTypeModel>(model, p => p.IdentityID == p.IdentityID, p => p.IdentityID, TABLE_NAME);
            }
        }
        public bool Exists(string channelCode, string typeName, int identityID)
        {
            return DataBaseHelper.Exists<DBLinkFriendTypeModel>(new { TypeName = typeName }, p => p.IdentityID, p => p.ChannelCode == channelCode && p.TypeName == p.TypeName, identityID, TABLE_NAME);
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBLinkFriendTypeModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBLinkFriendTypeModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public DBLinkFriendTypeModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBLinkFriendTypeModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.TypeName, p.TypeSort,p.ChannelCode }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public List<DBLinkFriendTypeModel> List()
        {
            return DataBaseHelper.More<DBLinkFriendTypeModel>(null, p => new { p.IdentityID, p.TypeName, p.ChannelCode }, null, p => p.TypeSort, true, TABLE_NAME);
        }
        public List<DBLinkFriendTypeModel> ChannelList(string channelCode)
        {
            return DataBaseHelper.More<DBLinkFriendTypeModel>(new { ChannelCode = channelCode }, p => new { p.IdentityID, p.TypeName, p.ChannelCode }, p=>p.ChannelCode == channelCode, p => p.TypeSort, true, TABLE_NAME);
        }

        public List<DBLinkFriendTypeFullModel> Page(string channelCode, string searchKey, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            StringBuilder stringBuilder = new StringBuilder();

            channelCode = StringHelper.FilterSpecChar(channelCode);
            searchKey = StringHelper.FilterSpecChar(searchKey);

            if (!string.IsNullOrEmpty(channelCode) && channelCode != "-1")
            {
                stringBuilder.Append(" ChannelCode = '");
                stringBuilder.Append(channelCode);
                stringBuilder.Append("' and ");
            }
            if (!string.IsNullOrEmpty(searchKey))
            {
                stringBuilder.Append(" TypeName like '%");
                stringBuilder.Append(searchKey);
                stringBuilder.Append("%' ");
            }

            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });
            return DataBaseHelper.ToEntityList<DBLinkFriendTypeFullModel>("", new DataBaseParameterItem("T_LinkFriendType", "IdentityID", pageIndex, pageSize, whereSql, "TypeSort desc")
            {
                FieldSql = "IdentityID, TypeName, TypeSort, ChannelCode, (select ChannelName from T_Channel with(nolock) where T_Channel.ChannelCode=T.ChannelCode) as ChannelName",
                Field = "IdentityID, TypeName, TypeSort, ChannelCode"
            }, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
