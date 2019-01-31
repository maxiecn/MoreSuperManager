using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class AttachmentDAL
    {
        private const string TABLE_NAME = "T_Attachment";

        public bool Operater(DBAttachmentModel model)
        {
            return DataBaseHelper.Insert<DBAttachmentModel>(model, p => new { p.IdentityID, p.AttachmentDate }, TABLE_NAME);
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBAttachmentModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBAttachmentModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public List<DBAttachmentModel> List(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.More<DBAttachmentModel>(null, p => new { p.IdentityID, p.AttachmentType, p.AttachmentName, p.AttachmentSize, p.AttachmentPath }, p => dataList.Contains(p.IdentityID), null, true, TABLE_NAME);
        }

        public List<DBAttachmentModel> Page(string searchKey, string attachmentType, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            StringBuilder stringBuilder = new StringBuilder();

            searchKey = StringHelper.FilterSpecChar(searchKey);

            if (!string.IsNullOrEmpty(searchKey))
            {
                stringBuilder.Append(string.Format(" AttachmentName like '%{0}%' ", searchKey));
                stringBuilder.Append(" and ");
            }
            if (!string.IsNullOrEmpty(attachmentType))
            {
                stringBuilder.Append(" AttachmentType = '");
                stringBuilder.Append(attachmentType);
                stringBuilder.Append("' and ");
            }

            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });
            return DataBaseHelper.ToEntityList<DBAttachmentModel>("", new DataBaseParameterItem("T_Attachment", "IdentityID", pageIndex, pageSize, whereSql, "IdentityID asc")
            {
                FieldSql = "IdentityID, AttachmentType,AttachmentName,AttachmentSize,AttachmentPath,AttachmentDate"
            }, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
