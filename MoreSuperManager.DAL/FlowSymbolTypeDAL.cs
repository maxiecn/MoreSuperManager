using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class FlowSymbolTypeDAL
    {
        private const string TABLE_NAME = "T_FlowSymbolType";

        public bool Operater(DBFlowSymbolTypeModel model)
        {
            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBFlowSymbolTypeModel>(model, p => p.IdentityID, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBFlowSymbolTypeModel>(model, p => p.IdentityID == p.IdentityID, p => p.IdentityID, TABLE_NAME);
            }
        }
        public bool Exists(string channelCode, string typeCode, int identityID)
        {
            return DataBaseHelper.Exists<DBFlowSymbolTypeModel>(new { TypeCode = typeCode, ChannelCode = channelCode }, p => p.IdentityID, p => p.ChannelCode == channelCode && p.TypeCode == p.TypeCode, identityID, TABLE_NAME);
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBFlowSymbolTypeModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBFlowSymbolTypeModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public DBFlowSymbolTypeModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBFlowSymbolTypeModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.TypeCode, p.TypeName, p.TypeSort, p.ChannelCode }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public List<DBFlowSymbolTypeModel> List()
        {
            return DataBaseHelper.More<DBFlowSymbolTypeModel>(null, p => new { p.IdentityID, p.TypeCode, p.TypeName, p.ChannelCode }, null, p => p.TypeSort, true, TABLE_NAME);
        }
        public List<DBFlowSymbolTypeModel> ChannelList(string channelCode)
        {
            return DataBaseHelper.More<DBFlowSymbolTypeModel>(new { ChannelCode = channelCode }, p => new { p.IdentityID, p.TypeCode, p.TypeName, p.ChannelCode }, p=>p.ChannelCode == channelCode, p => p.TypeSort, true, TABLE_NAME);
        }

        public List<DBFlowSymbolTypeFullModel> Page(string channelCode, string searchKey, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
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
                stringBuilder.Append(" TypeCode like '%");
                stringBuilder.Append(searchKey);
                stringBuilder.Append("%' or TypeName like '%");
                stringBuilder.Append(searchKey);
                stringBuilder.Append("%' ");
            }

            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });
            return DataBaseHelper.ToEntityList<DBFlowSymbolTypeFullModel>("", new DataBaseParameterItem("T_FlowSymbolType", "IdentityID", pageIndex, pageSize, whereSql, "TypeSort desc")
            {
                FieldSql = "IdentityID, TypeCode, TypeName, TypeSort, ChannelCode, (select ChannelName from T_Channel with(nolock) where T_Channel.ChannelCode=T.ChannelCode) as ChannelName",
                Field = "IdentityID, TypeCode, TypeName, TypeSort, ChannelCode"
            }, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
