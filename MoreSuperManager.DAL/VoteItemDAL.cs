using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class VoteItemDAL
    {
        private const string TABLE_NAME = "T_VoteItem";

        public List<DBVoteItemModel> List(int voteID)
        {
            return DataBaseHelper.More<DBVoteItemModel>(new { VoteID = voteID }, p => new { p.IdentityID, p.ItemID, p.VoteID, p.ItemTitle, p.ItemType, p.ItemContent, p.ItemMaxCount, p.ItemNum }, p => p.VoteID == p.VoteID, null, true, TABLE_NAME);
        }
    }
}
