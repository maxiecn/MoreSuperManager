using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class DBAttachmentModel
    {
        public int IdentityID { get; set; }
        /// <summary>
        /// 附件类型
        /// </summary>
        public string AttachmentType { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        public string AttachmentName { get; set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public int AttachmentSize { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string AttachmentPath { get; set; }
        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime AttachmentDate { get; set; }
    }
}
