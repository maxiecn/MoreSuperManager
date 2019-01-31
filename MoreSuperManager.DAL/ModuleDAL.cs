using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class ModuleDAL
    {
        private const string TABLE_NAME = "T_Module";

        public bool Operater(DBModuleModel model)
        {
            model.ActionList = StringHelper.PadChar(model.ActionList, ",");

            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBModuleModel>(model, p => p.IdentityID, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBModuleModel>(model, p => p.IdentityID == p.IdentityID, p => p.IdentityID, TABLE_NAME);
            }
        }
        public bool Exists(string channelCode, string moduleCode, int identityID)
        {
            return DataBaseHelper.Exists<DBModuleModel>(new { ModuleCode = moduleCode, ChannelCode = channelCode }, p => p.IdentityID, p => p.ModuleCode == p.ModuleCode && p.ChannelCode == p.ChannelCode, identityID, TABLE_NAME);
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBModuleModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBModuleModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public DBModuleModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBModuleModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.ModuleCode, p.ModuleName, p.ActionList, p.ChannelCode }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public DBModuleModel Select(string moduleCode)
        {
            return DataBaseHelper.Single<DBModuleModel>(new { ModuleCode = moduleCode }, p => new { p.IdentityID, p.ModuleCode, p.ModuleName, p.ActionList, p.ChannelCode }, p => p.ModuleCode == p.ModuleCode, TABLE_NAME);
        }
        public List<DBModuleModel> List()
        {
            return DataBaseHelper.More<DBModuleModel>(null, p => new { p.IdentityID, p.ModuleCode, p.ModuleName, p.ActionList, p.ChannelCode }, null, null, true, TABLE_NAME);
        }
        public List<DBModuleModel> ChannelList(string channelCode)
        {
            return DataBaseHelper.More<DBModuleModel>(new { ChannelCode = channelCode }, p => new { p.IdentityID, p.ModuleCode, p.ChannelCode, p.ModuleName, p.ActionList }, p => p.ChannelCode == p.ChannelCode, null, true, TABLE_NAME);
        }
        public List<DBModuleModel> CloneList(string channelCode)
        {
            return DataBaseHelper.More<DBModuleModel>(new { ChannelCode = channelCode }, p => new { p.ModuleCode, p.ChannelCode, p.ModuleName, p.ActionList }, p => p.ChannelCode == p.ChannelCode, null, true, TABLE_NAME);
        }

        public List<DBModuleFullModel> Page(string channelCode, string searchKey, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            StringBuilder stringBuilder = new StringBuilder();

            channelCode = StringHelper.FilterSpecChar(channelCode);
            searchKey = StringHelper.FilterSpecChar(searchKey);

            if (!string.IsNullOrEmpty(channelCode) && channelCode != "-2")
            {
                stringBuilder.Append(" ChannelCode = '");
                stringBuilder.Append(channelCode);
                stringBuilder.Append("' and ");
            }
            if(!string.IsNullOrEmpty(searchKey))
            {
                stringBuilder.Append(" ModuleCode like '%");
                stringBuilder.Append(searchKey);
                stringBuilder.Append("%' or ModuleName like '%");
                stringBuilder.Append(searchKey);
                stringBuilder.Append("%' ");
            }

            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });
            return DataBaseHelper.ToEntityList<DBModuleFullModel>("", new DataBaseParameterItem("T_Module", "IdentityID", pageIndex, pageSize, whereSql, "IdentityID desc")
            {
                FieldSql = "IdentityID, ModuleCode, ModuleName, ActionList, ChannelCode, (select ChannelName from T_Channel with(nolock) where T_Channel.ChannelCode=T.ChannelCode) as ChannelName",
                Field = "IdentityID, ModuleCode, ModuleName, ActionList, ChannelCode"
            }, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
