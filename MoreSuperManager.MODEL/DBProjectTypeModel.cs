using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBProjectTypeModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int TypeSort { get; set; }
    }
}
