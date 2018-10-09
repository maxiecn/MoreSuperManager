using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class ApplicationDAL
    {
        private const string TABLE_NAME = "T_Application";

        public bool Operater(List<DBApplicationModel> dataList)
        {
            return (bool)DataBaseHelper.Transaction((con, transaction) =>
            {
                DataBaseHelper.TransactionNonQuery(con, transaction, "delete from T_Application");
                DataBaseHelper.TransactionEntityListBatchImport<DBApplicationModel>(con, transaction, "T_Application", dataList);
                return true;
            });
        }

        public List<DBApplicationModel> List()
        {
            return DataBaseHelper.More<DBApplicationModel>(null, p => new { p.IdentityID, p.ApplicationName, p.ApplicationType, p.ApplicationIcon, p.ApplicationUrl, p.ApplicationX, p.ApplicationY, p.ApplicationWidth, p.ApplicationHeight }, null, p => p.IdentityID, false, TABLE_NAME);
        }
    }
}
