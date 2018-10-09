using Helper.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    [XmlT("item")]
    public class ViewSettingModel
    {
        [XmlT("name")]
        public string Name { get; set; }
        [XmlT("value")]
        public string Value { get; set; }
    }
}
