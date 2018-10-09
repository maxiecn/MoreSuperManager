using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBUserLogModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 用户 IP
        /// </summary>
        public string LoginIP { get; set; }
        /// <summary>
        /// 登录日期
        /// </summary>
        public DateTime LoginDate { get; set; }
        /// <summary>
        /// 登录状态
        /// </summary>
        public int LoginStatus { get; set; }
    }
}
