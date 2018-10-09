using Helper.Core.Library;
using MoreSuperManager.ENUM;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoreSuperManager.UI
{
    public class SettingHelper
    {
        private static string XmlPath = null;
        private static Dictionary<string, string> SettingDict = null;

        #region 初始 XML 数据，以及设置 XML 数据
        public static void Init(string xmlPath)
        {
            XmlPath = xmlPath;
            SettingDict = XmlHelper.ToDict<string, string>(xmlPath, "//item", "name", "value");
        }
        public static bool Set(Dictionary<string, string> dict)
        {
            try
            {
                if (dict == null) return false;

                string prevBakCon = (SettingDict != null && SettingDict.ContainsKey(SettingTypeEnum.BAKCRON)) ? SettingDict[SettingTypeEnum.BAKCRON] : "";
                string currentBakCon = (dict.ContainsKey(SettingTypeEnum.BAKCRON)) ? dict[SettingTypeEnum.BAKCRON] : "";

                if (!dict.ContainsKey(SettingTypeEnum.LOGOPENSTATUS))
                {
                    dict[SettingTypeEnum.LOGOPENSTATUS] = "0";
                }
                if (!dict.ContainsKey(SettingTypeEnum.AUTHOPENSTATUS))
                {
                    dict[SettingTypeEnum.AUTHOPENSTATUS] = "0";
                }
                if (!dict.ContainsKey(SettingTypeEnum.ATTACHOPENSTATUS))
                {
                    dict[SettingTypeEnum.ATTACHOPENSTATUS] = "0";
                }

                List<ViewSettingModel> modelList = new List<ViewSettingModel>();
                foreach (KeyValuePair<string, string> keyValueItem in dict)
                {
                    modelList.Add(new ViewSettingModel() { Name = keyValueItem.Key, Value = keyValueItem.Value });
                }
                XmlHelper.SetValue<ViewSettingModel>(XmlPath, "//settings", "item[@name=\"{name}\"]", modelList, null);
                SettingDict = dict;

                if (prevBakCon != currentBakCon)
                {
                    // 重新设置备份作业
                    TaskHelper.RunBakDataBase();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 配置数据
        public static string Version
        {
            get { return GetValue(SettingTypeEnum.VERSION); }
        }
        public static string ManagerTitle
        {
            get { return GetValue(SettingTypeEnum.MANAGERTITLE); }
        }
        public static int ManagerPageSize
        {
            get
            {
                string value = GetValue(SettingTypeEnum.MANAGERPAGESIZE);
                if (string.IsNullOrEmpty(value)) return 1;

                return int.Parse(value);
            }
        }
        public static bool LogOpenStatus
        {
            get
            {
                string value = GetValue(SettingTypeEnum.LOGOPENSTATUS);
                if (string.IsNullOrEmpty(value)) return false;

                return int.Parse(value) > 0;
            }
        }
        public static bool AuthOpenStatus
        {
            get
            {
                string value = GetValue(SettingTypeEnum.AUTHOPENSTATUS);
                if (string.IsNullOrEmpty(value)) return true;

                return int.Parse(value) > 0;
            }
        }
        public static bool AttachOpenStatus
        {
            get
            {
                string value = GetValue(SettingTypeEnum.ATTACHOPENSTATUS);
                if (string.IsNullOrEmpty(value)) return false;

                return int.Parse(value) > 0;
            }
        }
        public static int UploadImageMaxSize
        {
            get
            {
                string value = GetValue(SettingTypeEnum.UPLOADIMAGEMAXSIZE);
                if (string.IsNullOrEmpty(value)) return 0;
                return int.Parse(value);
            }
        }
        public static string UploadImageExt
        {
            get { return GetValue(SettingTypeEnum.UPLOADIMAGEEXT); }
        }
        public static int UploadVideoMaxSize
        {
            get
            {
                string value = GetValue(SettingTypeEnum.UPLOADVIDEOMAXSIZE);
                if (string.IsNullOrEmpty(value)) return 0;
                return int.Parse(value);
            }
        }
        public static string UploadVideoExt
        {
            get { return GetValue(SettingTypeEnum.UPLOADVIDEOEXT); }
        }
        public static int UploadFileMaxSize
        {
            get
            {
                string value = GetValue(SettingTypeEnum.UPLOADFILEMAXSIZE);
                if (string.IsNullOrEmpty(value)) return 0;
                return int.Parse(value);
            }
        }
        public static string UploadFileExt
        {
            get { return GetValue(SettingTypeEnum.UPLOADFILEEXT); }
        }
        public static string BakCron
        {
            get { return GetValue(SettingTypeEnum.BAKCRON); }
        }
        public static string BakPath
        {
            get { return GetValue(SettingTypeEnum.BAKPATH); }
        }
        #endregion

        #region 根据配置 KEY 获取配置数据
        private static string GetValue(string key)
        {
            if (SettingDict == null || !SettingDict.ContainsKey(key)) return "";
            return SettingDict[key];
        }
        #endregion
    }
}