using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MoreSuperManager.UI
{
    public class ConfigHelper
    {
        // 系统根目录
        public static string RootDirectoryPath = null;

        public static string ConnectionString
        {
            get { return GetValue("connectionString"); }
        }

        public static string ConfuseKey
        {
            get { return GetValue("confuseKey"); }
        }

        public static string TokenType
        {
            get { return GetValue("tokenType"); }
        }

        public static string TokenName
        {
            get { return GetValue("tokenName"); }
        }

        public static string TokenUserCode
        {
            get { return GetValue("tokenUserCode"); }
        }

        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}