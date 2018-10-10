using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBFlowTypeModel : IChannelModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 频道编号
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 类别排序
        /// </summary>
        public int TypeSort { get; set; }
    }
}
