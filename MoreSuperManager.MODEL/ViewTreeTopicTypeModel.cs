using Helper.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class ViewTreeTopicTypeModel : DBTopicTypeModel, ITreeMenu
    {
        public int TreeID
        {
            get { return this.IdentityID; }
            set { }
        }
        public string TreeName
        {
            get { return this.TypeName; }
            set { }
        }
        public int TreeSort
        {
            get { return this.TypeSort; }
            set { }
        }
        public int LayerIndex { get; set; }
        public string LayerName { get; set; }
    }
}
