using Helper.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreSuperManager.MODEL;

namespace MoreSuperManager.DAL
{
    public class LinkFriendDAL
    {
        private const string TABLE_NAME = "T_LinkFriend";

        public bool Operater(DBLinkFriendModel model)
        {
            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBLinkFriendModel>(model, p => p.IdentityID, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBLinkFriendModel>(model, p => p.IdentityID == p.IdentityID, p => p.IdentityID, TABLE_NAME);
            }
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBLinkFriendModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBLinkFriendModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public DBLinkFriendModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBLinkFriendModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.LinkFriendType, p.LinkFriendCoverImageUrl, p.LinkFriendName, p.LinkFriendUrl, p.LinkFriendSort, p.ChannelCode }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }

        public List<DBLinkFriendFullModel> Page(string channelCode, string searchKey, int linkFriendType, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            channelCode = StringHelper.FilterSpecChar(channelCode);
            searchKey = StringHelper.FilterSpecChar(searchKey);

            StringBuilder stringBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(channelCode) && channelCode != "-1")
            {
                stringBuilder.Append(" ChannelCode = '");
                stringBuilder.Append(channelCode);
                stringBuilder.Append("' and ");
            }
            if (!string.IsNullOrEmpty(searchKey))
            {
                stringBuilder.Append(string.Format(" LinkFriendName like '%{0}%' ", searchKey));
                stringBuilder.Append(" and ");
            }
            if (linkFriendType > 0)
            {
                stringBuilder.Append(" LinkFriendType = ");
                stringBuilder.Append(linkFriendType);
                stringBuilder.Append(" and ");
            }

            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });

            Dictionary<string, object> parameterList = new Dictionary<string, object>();
            parameterList.Add(DataBaseParameterEnum.FieldSql, "IdentityID, LinkFriendType, LinkFriendCoverImageUrl, LinkFriendName, LinkFriendUrl, LinkFriendSort, ChannelCode, (select TypeName from T_LinkFriendType with(nolock) where T_LinkFriendType.IdentityID=T.LinkFriendType) as LinkFriendTypeName, (select ChannelName from T_Channel with(nolock) where T_Channel.ChannelCode=T.ChannelCode) as ChannelName");
            parameterList.Add(DataBaseParameterEnum.Field, "IdentityID, LinkFriendType, LinkFriendCoverImageUrl, LinkFriendName, LinkFriendUrl, LinkFriendSort, ChannelCode");
            parameterList.Add(DataBaseParameterEnum.TableName, "T_LinkFriend");
            parameterList.Add(DataBaseParameterEnum.PrimaryKey, "IdentityID");
            parameterList.Add(DataBaseParameterEnum.PageIndex, pageIndex);
            parameterList.Add(DataBaseParameterEnum.PageSize, pageSize);
            parameterList.Add(DataBaseParameterEnum.WhereSql, whereSql);
            parameterList.Add(DataBaseParameterEnum.OrderSql, "IdentityID asc");
            parameterList.Add(DataBaseParameterEnum.JoinSql, "");

            return DataBaseHelper.ToEntityList<DBLinkFriendFullModel>("", parameterList, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
