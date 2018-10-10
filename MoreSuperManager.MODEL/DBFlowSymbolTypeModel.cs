using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBFlowSymbolTypeModel : IChannelModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 频道编号
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 符号编号
        /// </summary>
        public string TypeCode { get; set; }
        /// <summary>
        /// 符号名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int TypeSort { get; set; }
    }
}
