using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.ENUM
{
    public class OperaterTypeEnum
    {
        /// <summary>
        /// 默认
        /// </summary>
        public const string DEFAULT = "0";
        public const string DEFAULTNAME = "取消审核";
        /// <summary>
        /// 删除
        /// </summary>
        public const string DELETE = "1";
        public const string DELETENAME = "删除";
        /// <summary>
        /// 已审核
        /// </summary>
        public const string CHECKED = "2";
        public const string CHECKEDNAME = "审核";
    }
}
