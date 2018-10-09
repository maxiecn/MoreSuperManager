using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBRoleModel : IChannelModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 频道编号
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 菜单列表
        /// </summary>
        public string MenuList { get; set; }
        /// <summary>
        /// 动作列表
        /// </summary>
        public string ActionList { get; set; }
    }
}
