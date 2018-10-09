using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBChannelModel
    {
        public int IdentityID { get; set; }
        public string ChannelCode { get; set; }
        public string ChannelName { get; set; }
        public int ChannelSort { get; set; }
    }
}
