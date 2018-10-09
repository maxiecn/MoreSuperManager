using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class FlowStepDAL
    {
        private const string TABLE_NAME = "T_FlowStep";

        public List<DBFlowStepModel> List(int flowID)
        {
            return DataBaseHelper.More<DBFlowStepModel>(new { FlowID = flowID }, p => new { p.IdentityID, p.FlowID, p.StepCode, p.StepSymbol, p.StepName, p.StepAddrName, p.RoleList, p.StepList, p.NextStep, p.PositionTop, p.PositionLeft }, p => p.FlowID == p.FlowID, null, true, TABLE_NAME);
        }
        public List<DBFlowStepModel> List()
        {
            return DataBaseHelper.More<DBFlowStepModel>(null, p => new { p.IdentityID, p.StepCode, p.StepName }, null, null, true, TABLE_NAME);
        }

        public bool Operater(int flowID, List<DBFlowStepModel> modelList)
        {
            List<DataBaseTransactionItem> transactionItemList = new List<DataBaseTransactionItem>();
            if (modelList != null && modelList.Count > 0)
            {
                foreach (DBFlowStepModel model in modelList)
                {
                    transactionItemList.Add(new DataBaseTransactionItem()
                    {
                        CommandText = "update T_FlowStep set RoleList=@RoleList where IdentityID=@IdentityID",
                        ExecuteType = DataBaseExecuteTypeEnum.ExecuteNonQuery,
                        ParameterList = new { RoleList = StringHelper.PadChar(model.RoleList, ","), IdentityID = model.IdentityID }
                    });
                }
            }
            return DataBaseHelper.TransactionNonQuery(transactionItemList) > 0;
        }
    }
}
