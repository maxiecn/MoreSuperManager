using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class ChannelDAL
    {
        private const string TABLE_NAME = "T_Channel";

        public bool Operater(DBChannelModel model)
        {
            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBChannelModel>(model, p => p.IdentityID, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBChannelModel>(model, p => p.IdentityID == p.IdentityID, p => p.IdentityID, TABLE_NAME);
            }
        }

        public bool Exists(string channelCode, int identityID)
        {
            return DataBaseHelper.Exists<DBChannelModel>(new { ChannelCode = channelCode }, p => p.IdentityID, p => p.ChannelCode == p.ChannelCode, identityID, TABLE_NAME);
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBChannelModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBChannelModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public DBChannelModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBChannelModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.ChannelCode, p.ChannelName, p.ChannelSort }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public List<DBChannelModel> List()
        {
            return DataBaseHelper.More<DBChannelModel>(null, p => new { p.IdentityID, p.ChannelCode, p.ChannelName }, null, p => p.ChannelSort, true, TABLE_NAME);
        }
        public List<DBChannelModel> ChannelList()
        {
            return List().Where(p => !string.IsNullOrEmpty(p.ChannelCode) && p.ChannelCode != "-1").ToList();
        }
        public List<DBChannelModel> Page(string searchKey, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(searchKey))
            {
                stringBuilder.Append(" ChannelCode like '%");
                stringBuilder.Append(searchKey);
                stringBuilder.Append("%' or ChannelName like '%");
                stringBuilder.Append(searchKey);
                stringBuilder.Append("%' ");
            }
            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });

            Dictionary<string, object> parameterList = new Dictionary<string, object>();
            parameterList.Add("@FieldSql", "IdentityID, ChannelCode, ChannelName, ChannelSort");
            parameterList.Add("@Field", "");
            parameterList.Add("@TableName", "T_Channel");
            parameterList.Add("@PrimaryKey", "IdentityID");
            parameterList.Add("@PageIndex", pageIndex);
            parameterList.Add("@PageSize", pageSize);
            parameterList.Add("@WhereSql", whereSql);
            parameterList.Add("@OrderSql", "ChannelSort desc, IdentityID desc");

            return DataBaseHelper.ToEntityList<DBChannelModel>("", parameterList, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
