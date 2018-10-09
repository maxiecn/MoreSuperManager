using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBMessageReplyModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 留言编号
        /// </summary>
        public int MessageID { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReplyContent { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 回复日期
        /// </summary>
        public DateTime ReplyDate { get; set; }
    }
}
