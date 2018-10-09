using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBProjectFullModel : DBProjectModel
    {
        public string ProjectTypeName { get; set; }
        public string FlowName { get; set; }
        public string FlowStepName { get; set; }
    }
}
