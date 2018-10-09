using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.MODEL
{
    public class ViewPageModel
    {
        /// <summary>
        /// 分页
        /// </summary>
        public string PageUrl { get; set; }
        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 页总数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
