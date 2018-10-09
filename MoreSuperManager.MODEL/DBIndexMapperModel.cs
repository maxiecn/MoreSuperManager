using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBIndexMapperModel : IChannelModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 频道编号
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 映射类别
        /// </summary>
        public int IndexType { get; set; }
        /// <summary>
        /// 映射编号
        /// </summary>
        public int IndexID { get; set; }
        /// <summary>
        /// 对应编号
        /// </summary>
        public int MapperID { get; set; }
    }
}
