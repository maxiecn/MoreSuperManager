using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBModuleModel : IChannelModel
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        public int IdentityID { get; set; }
        /// <summary>
        /// 模块编号
        /// </summary>
        public string ModuleCode { get; set; }
        /// <summary>
        /// 频道编号
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 动作列表
        /// </summary>
        public string ActionList { get; set; }
    }
}
