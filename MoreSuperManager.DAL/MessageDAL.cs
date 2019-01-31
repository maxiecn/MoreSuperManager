using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class MessageDAL
    {
        private const string TABLE_NAME = "T_Message";

        public bool Operater(DBMessageModel model)
        {
            return DataBaseHelper.Insert<DBMessageModel>(model, p => new { p.IdentityID, p.MessageStatus, p.MessageDate }, TABLE_NAME);
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBMessageModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBMessageModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public bool Status(int identityID, int messageStatus)
        {
            return DataBaseHelper.Update<DBMessageModel>(new { IdentityID = identityID, MessageStatus = messageStatus }, p => p.IdentityID == p.IdentityID, p => p.IdentityID, TABLE_NAME);
        }
        public DBMessageFullModel FullSelect(int identityID)
        {
            string commandText = "select T_Message.IdentityID, ContactName, ContactTelphone, ContactEmail, MessageContent, ContactIP, MessageDate, MessageStatus, T_MessageReply.ReplyContent,T_MessageReply.UserCode,T_MessageReply.NickName,T_MessageReply.ReplyDate from T_Message with(nolock) left join T_MessageReply with(nolock) on T_Message.IdentityID=T_MessageReply.MessageID where T_Message.IdentityID=@IdentityID";
            return DataBaseHelper.ToEntity<DBMessageFullModel>(commandText, new { IdentityID = identityID });
        }

        public List<DBMessageModel> Page(string searchKey, int messageStatus, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            StringBuilder stringBuilder = new StringBuilder();

            searchKey = StringHelper.FilterSpecChar(searchKey);

            if (!string.IsNullOrEmpty(searchKey))
            {
                stringBuilder.Append(string.Format(" (ContactName like '%{0}%' ", searchKey));
                stringBuilder.Append(string.Format(" or ContactTelphone like '%{0}%' ", searchKey));
                stringBuilder.Append(string.Format(" or ContactEmail like '%{0}%' ", searchKey));
                stringBuilder.Append(string.Format(" or ContactIP like '%{0}%' ", searchKey));
                stringBuilder.Append(") and ");
            }
            if (messageStatus != -1)
            {
                stringBuilder.Append(" MessageStatus = ");
                stringBuilder.Append(messageStatus);
                stringBuilder.Append(" and ");
            }

            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });
            return DataBaseHelper.ToEntityList<DBMessageModel>("", new DataBaseParameterItem("T_Message", "IdentityID", pageIndex, pageSize, whereSql, "IdentityID asc")
            {
                FieldSql = "IdentityID, ContactName, ContactTelphone, ContactEmail, MessageContent, ContactIP, MessageDate, MessageStatus"
            }, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
