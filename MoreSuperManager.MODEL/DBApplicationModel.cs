using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBApplicationModel
    {
        public int IdentityID { get; set; }
        public string ApplicationIcon { get; set; }
        public string ApplicationUrl { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationType { get; set; }
        public int ApplicationX { get; set; }
        public int ApplicationY { get; set; }
        public int ApplicationWidth { get; set; }
        public int ApplicationHeight { get; set; }
    }
}
