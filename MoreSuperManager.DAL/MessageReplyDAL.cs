using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class MessageReplyDAL
    {
        private const string TABLE_NAME = "T_MessageReply";

        public bool Operater(DBMessageReplyModel model, int messageStatus)
        {
            return (bool)DataBaseHelper.Transaction((con, transaction) =>
            {
                DataBaseHelper.TransactionUpdate<DBMessageModel>(con, transaction, new { IdentityID = model.MessageID, MessageStatus = messageStatus }, p => p.IdentityID == p.IdentityID, p => p.IdentityID, "T_Message");
                return DataBaseHelper.TransactionInsert<DBMessageReplyModel>(con, transaction, model, p => new { p.IdentityID, p.ReplyDate }, TABLE_NAME);
            });
        }
    }
}
