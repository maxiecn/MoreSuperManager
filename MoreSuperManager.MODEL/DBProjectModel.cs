using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBProjectModel
    {
        /// <summary>
        /// 项目 ID
        /// </summary>
        public int IdentityID { get; set; }
        /// <summary>
        /// 项目分类
        /// </summary>
        public int ProjectType { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 流程编号
        /// </summary>
        public int FlowID { get; set; }
        /// <summary>
        /// 当前流程节点
        /// </summary>
        public int FlowStepID { get; set; }
    }
}
