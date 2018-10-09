using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBVoteItemModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 选项编号
        /// </summary>
        public string ItemID { get; set; }
        /// <summary>
        /// 投票编号
        /// </summary>
        public int VoteID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string ItemTitle { get; set; }
        /// <summary>
        /// 复选/单选/文本
        /// </summary>
        public int ItemType { get; set; }
        /// <summary>
        /// 选项内容，
        /// </summary>
        public string ItemContent { get; set; }
        /// <summary>
        /// 复选框最大投票数量
        /// </summary>
        public int ItemMaxCount { get; set; }
        /// <summary>
        /// 投票次数
        /// </summary>
        public int ItemNum { get; set; }
    }
}
