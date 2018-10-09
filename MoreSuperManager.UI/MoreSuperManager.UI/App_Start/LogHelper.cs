using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MoreSuperManager.UI
{
    public class LogHelper
    {
        #region 对外公开方法
        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("logerror");
            Task.Run(() => log.Error(message));
        }

        /// <summary>
        /// 写入异常消息
        /// </summary>
        /// <param name="exception"></param>
        public static void Error(Exception exception)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("logerror");
            Task.Run(() => log.Error(exception.Message.ToString() + "/r/n" + exception.Source.ToString() + "/r/n" + exception.TargetSite.ToString() + "/r/n" + exception.StackTrace.ToString()));
        }
        #endregion
    }
}