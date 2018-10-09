using Helper.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class ViewInitMenuItemModel
    {
        [TxtT(0)]
        public string MenuCode { get; set; }
        [TxtT(1)]
        public string MenuName { get; set; }
        [TxtT(2)]
        public string MenuUrl { get; set; }
        [TxtT(3)]
        public string MenuIcon { get; set; }
        [TxtT(4)]
        public string ParentCode { get; set; }
    }
}
