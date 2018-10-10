using MoreSuperManager.ENUM;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace MoreSuperManager.UI
{
    public class ConstHelper
    {
        private static readonly Dictionary<int, List<DBKeyValueModel>> IndexMapperKeyValueDict = new Dictionary<int, List<DBKeyValueModel>>()
        {
            { IndexMapperTypeEnum.TOPIC, new List<DBKeyValueModel>(){
                new DBKeyValueModel(){ Key = "1", Value = "栏目一" },
                new DBKeyValueModel(){ Key = "2", Value = "栏目二" },
            }},
            { IndexMapperTypeEnum.LINKFRIEND, new List<DBKeyValueModel>(){
                new DBKeyValueModel(){ Key = "1", Value = "链接一" },
                new DBKeyValueModel(){ Key = "2", Value = "链接二" },
            }}
        };

        private static readonly Dictionary<string, string> OperaterKeyValueDict = new Dictionary<string, string>()
        {
            { OperaterTypeEnum.DEFAULT, "取消审核"},
            { OperaterTypeEnum.DELETE, "删除" },
            { OperaterTypeEnum.CHECKED, "审核" }
        };

        public static List<DBKeyValueModel> GetIndexMapperList()
        {
            List<DBKeyValueModel> modelList = new List<DBKeyValueModel>()
            {
                new DBKeyValueModel(){ Key = IndexMapperTypeEnum.TOPIC.ToString(), Value = GetIndexMapperName(IndexMapperTypeEnum.TOPIC) },
                new DBKeyValueModel(){ Key = IndexMapperTypeEnum.LINKFRIEND.ToString(), Value = GetIndexMapperName(IndexMapperTypeEnum.LINKFRIEND) },
            };
            return modelList;
        }
        public static Dictionary<int, List<DBKeyValueModel>> GetIndexMapperKeyValueDict()
        {
            return IndexMapperKeyValueDict;
        }
        public static List<DBKeyValueModel> GetIndexMapperKeyValueList(int indexType)
        {
            if (IndexMapperKeyValueDict == null || !IndexMapperKeyValueDict.ContainsKey(indexType)) return null;
            return IndexMapperKeyValueDict[indexType];
        }
        public static string GetIndexMapperName(int indexType)
        {
            if (indexType == IndexMapperTypeEnum.TOPIC) return "新闻";
            if (indexType == IndexMapperTypeEnum.LINKFRIEND) return "链接";
            return "";
        }
        public static string GetIndexMapperName(int indexType, int indexID)
        {
            if (IndexMapperKeyValueDict == null || !IndexMapperKeyValueDict.ContainsKey(indexType)) return "";
            DBKeyValueModel model = IndexMapperKeyValueDict[indexType].Where(p => p.Key == indexID.ToString()).FirstOrDefault();
            if (model == null) return "";
            return model.Value;
        }
        public static string GetOperaterName(string operaterType)
        {
            if (OperaterKeyValueDict == null || OperaterKeyValueDict.Count == 0 || !OperaterKeyValueDict.ContainsKey(operaterType)) return "";
            return OperaterKeyValueDict[operaterType];
        }

        public static List<DBChannelModel> ChannelList(List<DBChannelModel> dataList)
        {
            dataList.Insert(0, new DBChannelModel() { ChannelCode = ChannelCodeTypeEnum.ALL, ChannelName = ChannelCodeTypeEnum.ALLNAME });
            return dataList;
        }

        public static string GetJsonText<T>(List<DBChannelModel> channelModelList, List<T> dataList, Func<T, object> keyFunc, Func<T, string> valueFunc, Func<T, string> codeFunc = null, bool containsManager = false, Func<T, bool> exceptFunc = null, DBKeyValueModel rootItem = null, bool containsManagerData = false) where T : IChannelModel
        {
            List<string> channelCodeList = channelModelList.Select(p => p.ChannelCode).ToList();
            if (containsManager) channelCodeList.Insert(0, ChannelCodeTypeEnum.ALL);

            channelCodeList = channelCodeList.Distinct().ToList();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");

            int channelIndex = 0;
            foreach (string channelCode in channelCodeList)
            {
                stringBuilder.Append("\\\"");
                stringBuilder.Append(channelCode);
                stringBuilder.Append("\\\":[");
                if (rootItem != null)
                {
                    stringBuilder.Append("{\\\"key\\\":\\\"");
                    stringBuilder.Append(rootItem.Key);
                    stringBuilder.Append("\\\",\\\"value\\\":\\\"");
                    stringBuilder.Append(rootItem.Value);
                    stringBuilder.Append("\\\"}");
                }
                List<T> dataItemList = null;
                if (containsManagerData && (string.IsNullOrEmpty(channelCode) && channelCode == ChannelCodeTypeEnum.ALL))
                {
                    dataItemList = dataList;
                }
                else
                {
                    dataItemList = dataList.Where(p => p.ChannelCode == channelCode).ToList();
                }
                if (dataItemList != null && dataItemList.Count > 0)
                {
                    if (rootItem != null)
                    {
                        stringBuilder.Append(",");
                    }
                    int itemIndex = 0;
                    foreach (T t in dataItemList)
                    {
                        if (exceptFunc == null || !exceptFunc(t))
                        {
                            stringBuilder.Append("{\\\"key\\\":\\\"");
                            stringBuilder.Append(keyFunc != null ? keyFunc(t) : "");
                            stringBuilder.Append("\\\",\\\"value\\\":\\\"");
                            stringBuilder.Append(valueFunc != null ? valueFunc(t) : "");
                            stringBuilder.Append("\\\"");
                            if(codeFunc != null)
                            {
                                stringBuilder.Append(",\\\"code\\\":\\\"");
                                stringBuilder.Append(codeFunc(t));
                                stringBuilder.Append("\\\"");
                            }
                            stringBuilder.Append("}");
                            if (itemIndex < dataItemList.Count - 1)
                            {
                                stringBuilder.Append(",");
                            }
                        }
                        itemIndex++;
                    }
                }
                stringBuilder.Append("]");
                if (channelIndex < channelCodeList.Count - 1)
                {
                    stringBuilder.Append(",");
                }
                channelIndex++;
            }

            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
        public static string GetJsonText(Dictionary<int, List<DBKeyValueModel>> KeyValueDict)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");

            if (KeyValueDict != null && KeyValueDict.Count > 0)
            {
                int typeIndex = 0;
                foreach (KeyValuePair<int, List<DBKeyValueModel>> keyValueItem in KeyValueDict)
                {
                    stringBuilder.Append("\\\"");
                    stringBuilder.Append(keyValueItem.Key);
                    stringBuilder.Append("\\\":[");

                    List<DBKeyValueModel> keyValueList = keyValueItem.Value;
                    int keyValueCount = keyValueList.Count;
                    for (int index = 0; index < keyValueCount; index++)
                    {
                        stringBuilder.Append("{\\\"key\\\":\\\"");
                        stringBuilder.Append(keyValueList[index].Key);
                        stringBuilder.Append("\\\",\\\"value\\\":\\\"");
                        stringBuilder.Append(keyValueList[index].Value);
                        stringBuilder.Append("\\\"}");
                        if (index < keyValueCount - 1)
                        {
                            stringBuilder.Append(",");
                        }
                    }

                    stringBuilder.Append("]");
                    if (typeIndex < KeyValueDict.Count - 1)
                    {
                        stringBuilder.Append(",");
                    }
                    typeIndex++;
                }
            }

            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
        public static List<DBKeyValueModel> GetChannelKeyValueList<T>(List<DBChannelModel> channelModelList, List<T> modelList, string channelCode, Func<T, object> keyFunc, Func<T, string> valueFunc) where T : IChannelModel
        {
            List<DBKeyValueModel> resultList = new List<DBKeyValueModel>();

            Dictionary<string, string> channelDict = new Dictionary<string, string>();
            foreach (DBChannelModel modelItem in channelModelList)
            {
                channelDict.Add(modelItem.ChannelCode, modelItem.ChannelName);
            }
            if (modelList != null && modelList.Count > 0)
            {
                List<string> channelCodeItemList = modelList.Select(p => p.ChannelCode).Distinct().ToList();
                foreach (string channelCodeItem in channelCodeItemList)
                {
                    List<T> dataList = modelList.Where(p => p.ChannelCode == channelCodeItem).ToList();
                    if (dataList != null && dataList.Count > 0)
                    {
                        foreach (T t in dataList)
                        {
                            string valueText = valueFunc != null ? valueFunc(t) : "";
                            resultList.Add(new DBKeyValueModel()
                            {
                                Key = keyFunc != null ? keyFunc(t).ToString() : "",
                                Value = ((string.IsNullOrEmpty(channelCode) || channelCode == "-1") && channelDict.ContainsKey(t.ChannelCode)) ? string.Format("{0}/{1}", channelDict[t.ChannelCode], valueText) : valueText
                            });
                        }
                    }
                }
            }
            return resultList;
        }
    }
}