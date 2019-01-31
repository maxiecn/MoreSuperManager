using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class TopicDAL
    {
        private const string TABLE_NAME = "T_Topic";

        public bool Operater(DBTopicModel model)
        {
            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBTopicModel>(model, p => new { p.IdentityID, p.TopicVisitNum, p.TopicDateTime }, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBTopicModel>(model, p => p.IdentityID == p.IdentityID, p => new { p.IdentityID, p.TopicVisitNum, p.TopicDateTime }, TABLE_NAME);
            }
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBTopicModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBTopicModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public bool StatusMore(string identityIDList, int topicStatus)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Update<DBTopicModel>(new { TopicStatus = topicStatus }, p => dataList.Contains(p.IdentityID), null, TABLE_NAME);
        }
        public DBTopicModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBTopicModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.TopicType, p.PositionTypeList, p.TopicTitle, p.TopicTags, p.TopicCoverImageUrl, p.TopicSummary, p.TopicContent, p.TopicOriginalWebsite, p.TopicOriginalUrl, p.TopicUserCode, p.TopicStatus, p.ChannelCode }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }

        public List<DBTopicFullModel> Page(string channelCode, string searchKey, int topicType, int topicPositionType, int topicStatus, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
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
            if (topicStatus != -1)
            {
                stringBuilder.Append(string.Format(" TopicStatus = {0} and ", topicStatus));
            }
            if (!string.IsNullOrEmpty(searchKey))
            {
                stringBuilder.Append(string.Format(" TopicTitle like '%{0}%' ", searchKey));
                stringBuilder.Append(" and ");
            }
            if (topicType > 0)
            {
                stringBuilder.Append(" TopicType = ");
                stringBuilder.Append(topicType);
                stringBuilder.Append(" and ");
            }
            if (topicPositionType > 0)
            {
                stringBuilder.Append(" CHARINDEX(',");
                stringBuilder.Append(topicPositionType);
                stringBuilder.Append(",', PositionTypeList)>0 and ");
            }

            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });
            return DataBaseHelper.ToEntityList<DBTopicFullModel>("", new DataBaseParameterItem("T_Topic", "IdentityID", pageIndex, pageSize, whereSql, "IdentityID asc")
            {
                FieldSql = "IdentityID, TopicType, PositionTypeList, TopicTitle, TopicCoverImageUrl, TopicStatus, TopicUserCode, TopicVisitNum, TopicDateTime, ChannelCode, (select TypeName from T_TopicType with(nolock) where T_TopicType.IdentityID=T.TopicType) as TopicTypeName, (select ChannelName from T_Channel with(nolock) where T_Channel.ChannelCode=T.ChannelCode) as ChannelName",
                Field = "IdentityID, TopicType, PositionTypeList, TopicTitle, TopicCoverImageUrl, TopicStatus, TopicUserCode, TopicVisitNum, TopicDateTime, ChannelCode"
            }, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
