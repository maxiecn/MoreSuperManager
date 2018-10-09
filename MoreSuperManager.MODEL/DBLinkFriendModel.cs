using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBLinkFriendModel : IChannelModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 频道编号
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public int LinkFriendType { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public string LinkFriendCoverImageUrl { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string LinkFriendName { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string LinkFriendUrl { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int LinkFriendSort { get; set; }
    }
}
