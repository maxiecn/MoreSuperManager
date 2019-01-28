using Helper.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreSuperManager.MODEL;

namespace MoreSuperManager.DAL
{
    public class NoticeDAL
    {
        private const string TABLE_NAME = "T_Notice";

        public bool Operater(DBNoticeModel model)
        {
            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBNoticeModel>(model, p => new { p.IdentityID, p.NoticeDateTime }, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBNoticeModel>(model, p => p.IdentityID == p.IdentityID, p => new { p.IdentityID, p.NoticeDateTime }, TABLE_NAME);
            }
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBNoticeModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBNoticeModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public DBNoticeModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBNoticeModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.NoticeType, p.NoticeTitle, p.NoticeContent, p.ChannelCode }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public List<DBNoticeFullModel> Page(string channelCode, string searchKey, int noticeType, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
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
                stringBuilder.Append(string.Format(" NoticeTitle like '%{0}%' ", searchKey));
                stringBuilder.Append(" and ");
            }
            if (noticeType > 0)
            {
                stringBuilder.Append(" NoticeType = ");
                stringBuilder.Append(noticeType);
                stringBuilder.Append(" and ");
            }

            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });

            Dictionary<string, object> parameterList = new Dictionary<string, object>();
            parameterList.Add(DataBaseParameterEnum.FieldSql, "IdentityID, NoticeType, NoticeTitle, NoticeDateTime, ChannelCode, (select TypeName from T_NoticeType with(nolock) where T_NoticeType.IdentityID=T.NoticeType) as NoticeTypeName, (select ChannelName from T_Channel with(nolock) where T_Channel.ChannelCode=T.ChannelCode) as ChannelName");
            parameterList.Add(DataBaseParameterEnum.Field, "IdentityID, NoticeType, NoticeTitle, NoticeDateTime, ChannelCode");
            parameterList.Add(DataBaseParameterEnum.TableName, "T_Notice");
            parameterList.Add(DataBaseParameterEnum.PrimaryKey, "IdentityID");
            parameterList.Add(DataBaseParameterEnum.PageIndex, pageIndex);
            parameterList.Add(DataBaseParameterEnum.PageSize, pageSize);
            parameterList.Add(DataBaseParameterEnum.WhereSql, whereSql);
            parameterList.Add(DataBaseParameterEnum.OrderSql, "IdentityID asc");
            parameterList.Add(DataBaseParameterEnum.JoinSql, "");

            return DataBaseHelper.ToEntityList<DBNoticeFullModel>("", parameterList, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
