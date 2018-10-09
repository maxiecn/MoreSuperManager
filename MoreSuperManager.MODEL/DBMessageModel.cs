using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBMessageModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTelphone { get; set; }
        /// <summary>
        /// 联系邮箱
        /// </summary>
        public string ContactEmail { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string MessageContent { get; set; }
        /// <summary>
        /// 联系人 IP
        /// </summary>
        public string ContactIP { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int MessageStatus { get; set; }
        /// <summary>
        /// 留言日期
        /// </summary>
        public DateTime MessageDate { get; set; }
    }
}
