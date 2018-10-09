using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBVoteTextModel
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        public int IdentityID { get; set; }
        /// <summary>
        /// 投票编号
        /// </summary>
        public int VoteID { get; set; }
        /// <summary>
        /// 投票选项编号
        /// </summary>
        public int ItemID { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string VoteText { get; set; }
    }
}
