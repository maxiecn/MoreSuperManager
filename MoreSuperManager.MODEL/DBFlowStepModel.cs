using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBFlowStepModel
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        public int IdentityID { get; set; }
        /// <summary>
        /// 流程编号
        /// </summary>
        public int FlowID { get; set; }
        /// <summary>
        /// 步骤编号
        /// </summary>
        public string StepCode { get; set; }
        /// <summary>
        /// 步骤符号
        /// </summary>
        public string StepSymbol { get; set; }
        /// <summary>
        /// 步骤名称
        /// </summary>
        public string StepName { get; set; }
        /// <summary>
        /// 步骤简称
        /// </summary>
        public string StepAddrName { get; set; }
        /// <summary>
        /// 角色列表
        /// </summary>
        public string RoleList { get; set; }
        /// <summary>
        /// 子编号
        /// </summary>
        public string StepList { get; set; }
        /// <summary>
        /// 下一步编号
        /// </summary>
        public string NextStep { get; set; }
        /// <summary>
        /// 上坐标
        /// </summary>
        public float PositionTop { get; set; }
        /// <summary>
        /// 左坐标
        /// </summary>
        public float PositionLeft { get; set; }
    }
}
