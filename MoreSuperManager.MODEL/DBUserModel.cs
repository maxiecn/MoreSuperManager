using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBUserModel
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        public int IdentityID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPassword { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>
        public int RoleID { get; set; }
    }
}
