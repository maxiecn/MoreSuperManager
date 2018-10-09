using Helper.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class ViewTreeMenuModel : DBMenuModel, ITreeMenu
    {
        public int TreeID
        {
            get { return this.IdentityID; }
            set { }
        }
        public string TreeName
        {
            get { return this.MenuName; }
            set { }
        }
        public int TreeSort
        {
            get { return this.MenuSort; }
            set { }
        }
        public int LayerIndex { get; set; }
        public string LayerName { get; set; }
    }
}
