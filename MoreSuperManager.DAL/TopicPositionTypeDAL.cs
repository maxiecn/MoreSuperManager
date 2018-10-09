using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class TopicPositionTypeDAL
    {
        private const string TABLE_NAME = "T_TopicPositionType";

        public bool Operater(DBTopicPositionTypeModel model)
        {
            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBTopicPositionTypeModel>(model, p => p.IdentityID, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBTopicPositionTypeModel>(model, p => p.IdentityID == p.IdentityID, p => p.IdentityID, TABLE_NAME);
            }
        }
        public bool Exists(string typeName, int identityID)
        {
            return DataBaseHelper.Exists<DBTopicPositionTypeModel>(new { TypeName = typeName }, p => p.IdentityID, p => p.TypeName == p.TypeName, identityID, TABLE_NAME);
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBTopicPositionTypeModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBTopicPositionTypeModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public DBTopicPositionTypeModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBTopicPositionTypeModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.TypeName, p.TypeSort, p.ChannelCode }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public List<DBTopicPositionTypeModel> List()
        {
            return DataBaseHelper.More<DBTopicPositionTypeModel>(null, p => new { p.IdentityID, p.TypeName, p.ChannelCode }, null, p => p.TypeSort, true, TABLE_NAME);
        }
        public List<DBTopicPositionTypeModel> ChannelList(string channelCode)
        {
            return DataBaseHelper.More<DBTopicPositionTypeModel>(new { ChannelCode = channelCode }, p => new { p.IdentityID, p.TypeName }, p => p.ChannelCode == p.ChannelCode, p => p.TypeSort, true, TABLE_NAME);
        }

        public List<DBTopicPositionTypeFullModel> Page(string channelCode, string searchKey, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            StringBuilder stringBuilder = new StringBuilder();
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
                stringBuilder.Append("%' and ");
            }
            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });

            Dictionary<string, object> parameterList = new Dictionary<string, object>();
            parameterList.Add("@FieldSql", "IdentityID, TypeName, TypeSort, ChannelCode, (select ChannelName from T_Channel with(nolock) where T_Channel.ChannelCode=T.ChannelCode) as ChannelName");
            parameterList.Add("@Field", "IdentityID, TypeName, TypeSort, ChannelCode");
            parameterList.Add("@TableName", "T_TopicPositionType");
            parameterList.Add("@PrimaryKey", "IdentityID");
            parameterList.Add("@PageIndex", pageIndex);
            parameterList.Add("@PageSize", pageSize);
            parameterList.Add("@WhereSql", whereSql);
            parameterList.Add("@OrderSql", "TypeSort desc");

            return DataBaseHelper.ToEntityList<DBTopicPositionTypeFullModel>("", parameterList, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
