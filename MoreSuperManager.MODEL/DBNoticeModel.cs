using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBNoticeModel : IChannelModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 频道编号
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 公告类别
        /// </summary>
        public int NoticeType { get; set; }
        /// <summary>
        /// 公告标题
        /// </summary>
        public string NoticeTitle { get; set; }
        /// <summary>
        /// 公告内容
        /// </summary>
        public string NoticeContent { get; set; }
        /// <summary>
        /// 发表时间
        /// </summary>
        public DateTime NoticeDateTime { get; set; }
    }
}
