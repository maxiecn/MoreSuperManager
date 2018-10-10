using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBFlowModel : IChannelModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 频道编号
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 流程类别
        /// </summary>
        public int FlowType { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        public string FlowName { get; set; }
    }
}
