using Helper.Core.Library;
using MoreSuperManager.DAL;
using MoreSuperManager.ENUM;
using MoreSuperManager.FILTER;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class IndexMapperController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string channelCode = "", int indexType = -1, int pageIndex = 1)
        {
            List<DBIndexMapperFullModel> modelList = DALFactory.IndexMapper.Page(this.GetChannelCode(channelCode), indexType, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);

            this.InitViewData("", pageIndex, Url.Action("List", new { PageIndex = -999, ChannelCode = channelCode, IndexType = indexType }), DALFactory.Channel.ChannelList(), channelCode);

            ViewBag.IndexTypeList = ConstHelper.GetIndexMapperList();
            ViewData["IndexType"] = indexType;

            return View(modelList);
        }

        [RoleActionFilter]
        public ActionResult Add()
        {
            return this.Edit();
        }

        [RoleActionFilter]
        public ActionResult Edit(int identityID = 0)
        {
            DBIndexMapperModel model = identityID > 0 ? DALFactory.IndexMapper.Select(identityID) : null;

            string channelCode = null;
            List<DBChannelModel> channelModelList = null;

            this.InitChannelViewData<DBIndexMapperModel>(model, (p, k) =>
            {
                channelCode = p;
                channelModelList = k;
            }, () =>
            {
                return DALFactory.Channel.ChannelList();
            });

            Dictionary<int, List<DBKeyValueCodeModel>> indexTypeMapperKeyValueDict = this.GetIndexTypeMapperKeyValueDict(this.viewUserModel.ChannelCode);
            ViewBag.IndexTypeList = ConstHelper.GetIndexMapperList();
            if(model != null)
            {
                ViewBag.IndexIDList = ConstHelper.GetIndexMapperKeyValueList(model.IndexType);
                if (indexTypeMapperKeyValueDict.ContainsKey(model.IndexType))
                {
                    ViewBag.IndexMapperList = indexTypeMapperKeyValueDict[model.IndexType].Where(p => p.Code == model.ChannelCode).ToList();
                }
            }
            else
            {
                ViewBag.IndexIDList = ConstHelper.GetIndexMapperKeyValueList(IndexMapperTypeEnum.TOPIC);
                if (indexTypeMapperKeyValueDict.ContainsKey(IndexMapperTypeEnum.TOPIC))
                {
                    ViewBag.IndexMapperList = indexTypeMapperKeyValueDict[IndexMapperTypeEnum.TOPIC].Where(p => p.Code == channelCode).ToList();
                }
            }

            ViewBag.IndexJsonText = this.GetIndexJsonText();
            ViewBag.MapperJsonText = this.GetMapperJsonText(this.GetIndexTypeMapperKeyValueDict(this.viewUserModel.ChannelCode), channelModelList);

            ViewBag.ChannelCode = this.viewUserModel.ChannelCode;
            return View("Edit", model);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.IndexMapper.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBIndexMapperModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBIndexMapperModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.IndexMapper.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBIndexMapperModel model)
        {
            this.SetChannelCode<DBIndexMapperModel>(model);

            return this.OperaterConfirm(() =>
            {
                return FilterFactory.IndexMapper.Operater(model);
            }, null, () =>
            {
                return DALFactory.IndexMapper.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }));
        }

        private string GetIndexJsonText()
        {
            return ConstHelper.GetJsonText(ConstHelper.GetIndexMapperKeyValueDict());
        }
        private string GetMapperJsonText(Dictionary<int, List<DBKeyValueCodeModel>> keyValueDict, List<DBChannelModel> channelModelList)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");

            int dictIndex = 0;
            foreach(KeyValuePair<int, List<DBKeyValueCodeModel>> keyValueItem in keyValueDict)
            {
                int channelIndex = 0;
                foreach(DBChannelModel channelMoelItem in channelModelList)
                {
                    stringBuilder.Append("\\\"");
                    stringBuilder.Append(keyValueItem.Key);
                    stringBuilder.Append("-");
                    stringBuilder.Append(channelMoelItem.ChannelCode);
                    stringBuilder.Append("\\\":[");

                    List<DBKeyValueCodeModel> dataList = keyValueItem.Value.Where(p => p.Code == channelMoelItem.ChannelCode).ToList();
                    if(dataList != null && dataList.Count > 0)
                    {
                        int itemIndex = 0;
                        foreach(DBKeyValueCodeModel dataItem in dataList)
                        {
                            stringBuilder.Append("{\\\"key\\\":\\\"");
                            stringBuilder.Append(dataItem.Key);
                            stringBuilder.Append("\\\",\\\"value\\\":\\\"");
                            stringBuilder.Append(dataItem.Value);
                            stringBuilder.Append("\\\"}");
                            if(itemIndex < dataList.Count - 1)
                            {
                                stringBuilder.Append(",");
                            }
                            itemIndex++;
                        }
                    }

                    stringBuilder.Append("]");
                    if(channelIndex < channelModelList.Count - 1)
                    {
                        stringBuilder.Append(",");
                    }
                    channelIndex++;
                }
                if(dictIndex < keyValueDict.Count - 1)
                {
                    stringBuilder.Append(",");
                }
                dictIndex++;
            }

            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        #region 每个项目这儿都会不同，所以需要修改此方法
        private Dictionary<int, List<DBKeyValueCodeModel>> GetIndexTypeMapperKeyValueDict(string channelCode)
        {
            Dictionary<int, List<DBKeyValueCodeModel>> resultDict = new Dictionary<int, List<DBKeyValueCodeModel>>();

            List<ViewTreeTopicTypeModel> topicTypeModelList = TreeHelper.ToMenuList<ViewTreeTopicTypeModel>(DALFactory.TopicType.TreeList());
            if (topicTypeModelList != null && topicTypeModelList.Count > 0)
            {
                resultDict.Add(IndexMapperTypeEnum.TOPIC, new List<DBKeyValueCodeModel>());
                foreach(ViewTreeTopicTypeModel modelItem in topicTypeModelList)
                {
                    resultDict[IndexMapperTypeEnum.TOPIC].Add(new DBKeyValueCodeModel() { Key = modelItem.IdentityID.ToString(), Value = modelItem.LayerName, Code = modelItem.ChannelCode });
                }
            }

            List<DBLinkFriendTypeModel> linkTypeModelList = DALFactory.LinkFriendType.List();
            if(linkTypeModelList != null && linkTypeModelList.Count > 0)
            {
                resultDict.Add(IndexMapperTypeEnum.LINKFRIEND, new List<DBKeyValueCodeModel>());
                foreach(DBLinkFriendTypeModel modelItem in linkTypeModelList)
                {
                    resultDict[IndexMapperTypeEnum.LINKFRIEND].Add(new DBKeyValueCodeModel() { Key = modelItem.IdentityID.ToString(), Value = modelItem.TypeName, Code = modelItem.ChannelCode });
                }
            }

            return resultDict;
        }
        #endregion
    }
}