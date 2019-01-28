using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class ProjectDAL
    {
        private const string TABLE_NAME = "T_Project";

        public bool Operater(DBProjectModel model, int flowStepID = 0)
        {
            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBProjectModel>(model, p => p.IdentityID, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBProjectModel>(model, p => p.IdentityID == p.IdentityID, p => p.IdentityID, TABLE_NAME);
            }
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBProjectModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBProjectModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public DBProjectModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBProjectModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.ProjectType, p.ProjectName, p.FlowID, p.FlowStepID, p.ChannelCode }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }

        public List<DBProjectFullModel> Page(string channelCode, string searchKey, int projectType, int flowID, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
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
                stringBuilder.Append(string.Format(" ProjectName like '%{0}%' ", searchKey));
                stringBuilder.Append(" and ");
            }
            if (projectType > 0)
            {
                stringBuilder.Append(" ProjectType = ");
                stringBuilder.Append(projectType);
                stringBuilder.Append(" and ");
            }
            if (flowID > 0)
            {
                stringBuilder.Append(" FlowID = ");
                stringBuilder.Append(flowID);
                stringBuilder.Append(" and ");
            }

            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });

            Dictionary<string, object> parameterList = new Dictionary<string, object>();
            parameterList.Add(DataBaseParameterEnum.FieldSql, "IdentityID, ProjectName, ProjectType, FlowStepID, ChannelCode, (select TypeName from T_ProjectType with(nolock) where T_ProjectType.IdentityID=T.ProjectType) as ProjectTypeName, (select FlowName from T_Flow with(nolock) where T_Flow.IdentityID=T.FlowID) as FlowName, (select StepName from T_FlowStep with(nolock) where T_FlowStep.IdentityID = T.FlowStepID) as FlowStepName, (select ChannelName from T_Channel with(nolock) where T_Channel.ChannelCode=T.ChannelCode) as ChannelName");
            parameterList.Add(DataBaseParameterEnum.Field, "IdentityID, ProjectName, ProjectType, FlowID, FlowStepID, ChannelCode");
            parameterList.Add(DataBaseParameterEnum.TableName, "T_Project");
            parameterList.Add(DataBaseParameterEnum.PrimaryKey, "IdentityID");
            parameterList.Add(DataBaseParameterEnum.PageIndex, pageIndex);
            parameterList.Add(DataBaseParameterEnum.PageSize, pageSize);
            parameterList.Add(DataBaseParameterEnum.WhereSql, whereSql);
            parameterList.Add(DataBaseParameterEnum.OrderSql, "IdentityID asc");
            parameterList.Add(DataBaseParameterEnum.JoinSql, "");

            return DataBaseHelper.ToEntityList<DBProjectFullModel>("", parameterList, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
