using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBMenuModel : IChannelModel
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        public int IdentityID { get; set; }
        /// <summary>
        /// 频道编号
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 父编号
        /// </summary>
        public int ParentID { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 菜单访问地址
        /// </summary>
        public string MenuUrl { get; set; }
        /// <summary>
        /// 所属模块
        /// </summary>
        public string BelongModule { get; set; }
        /// <summary>
        /// 动作列表
        /// </summary>
        public string ActionList { get; set; }
        /// <summary>
        /// 菜单排序
        /// </summary>
        public int MenuSort { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuIcon { get; set; }
    }
}
