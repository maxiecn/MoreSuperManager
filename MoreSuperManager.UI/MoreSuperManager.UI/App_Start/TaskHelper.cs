using Helper.Core.Library;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MoreSuperManager.UI
{
    public class TaskHelper
    {
        private const string BAK_DATABASE = "bak_database";
        /// <summary>
        /// 备份数据库
        /// </summary>
        public static void RunBakDataBase()
        {
            QuartzHelper.Delete(BAK_DATABASE);

            // 备份数据库
            string cronExpression = SettingHelper.BakCron;
            if (!string.IsNullOrEmpty(cronExpression))
            {
                QuartzHelper.Task<BakDataBaseJob>(cronExpression, BAK_DATABASE);
            }
        }

        public static bool ExecuteBakDataBase()
        {
            try
            {
                string bakPath = SettingHelper.BakPath;
                if (string.IsNullOrEmpty(bakPath)) return false;

                if (bakPath.StartsWith("~"))
                {
                    bakPath = ConfigHelper.RootDirectoryPath + StringHelper.TrimStart(bakPath, "~/");
                }
                if (!bakPath.EndsWith("/") && !bakPath.EndsWith("\\"))
                {
                    bakPath += "/";
                }

                if (!System.IO.Directory.Exists(bakPath))
                {
                    System.IO.Directory.CreateDirectory(bakPath);
                }

                string databaseKey = "database";

                Dictionary<string, string> connectionDict = StringHelper.ToDict<string, string>(ConfigHelper.ConnectionString, ";", "=");
                if (!connectionDict.ContainsKey(databaseKey)) return false;

                string dataBaseName = connectionDict[databaseKey];
                string bakFullPath = bakPath + DateTime.Now.ToString("yyyyMMdd") + ".bak";
                // 如果文件已存在
                if (System.IO.File.Exists(bakFullPath))
                {
                    bakFullPath = bakFullPath.Insert(bakFullPath.LastIndexOf("."), "_" + System.Guid.NewGuid().ToString("N"));
                }

                string commandText = string.Format("BACKUP DATABASE {0} TO DISK = '{1}' ", dataBaseName, bakFullPath);
                DataBaseHelper.ExecuteNonQuery(commandText);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class BakDataBaseJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            bool result = TaskHelper.ExecuteBakDataBase();
            if (!result)
            {
                if (SettingHelper.LogOpenStatus)
                {
                    LogHelper.Error("数据库备份失败！");
                }
            }
        }
    }
}