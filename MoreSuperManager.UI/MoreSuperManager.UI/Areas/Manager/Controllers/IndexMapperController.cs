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
            List<DBIndexMapperModel> modelList = DALFactory.IndexMapper.Page(this.GetChannelCode(channelCode), indexType, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);

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
            Dictionary<int, List<DBKeyValueModel>> indexTypeMapperKeyValueDict = this.GetIndexTypeMapperKeyValueDict("");
            DBIndexMapperModel model = identityID > 0 ? DALFactory.IndexMapper.Select(identityID) : null;
            ViewBag.IndexTypeList = ConstHelper.GetIndexMapperList();
            if(model != null)
            {
                ViewBag.IndexIDList = ConstHelper.GetIndexMapperKeyValueList(model.IndexType);
                if (indexTypeMapperKeyValueDict.ContainsKey(model.IndexType))
                {
                    ViewBag.IndexMapperList = indexTypeMapperKeyValueDict[model.IndexType];
                }
            }
            else
            {
                ViewBag.IndexIDList = ConstHelper.GetIndexMapperKeyValueList(IndexMapperTypeEnum.TOPIC);
                if (indexTypeMapperKeyValueDict.ContainsKey(IndexMapperTypeEnum.TOPIC))
                {
                    ViewBag.IndexMapperList = indexTypeMapperKeyValueDict[IndexMapperTypeEnum.TOPIC];
                }
            }
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

        public ActionResult GetIndexJsonText()
        {
            return this.GetJsonText(ConstHelper.GetIndexMapperKeyValueDict());
        }
        public ActionResult GetMapperJsonText()
        {
            return this.GetJsonText(this.GetIndexTypeMapperKeyValueDict(""));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBIndexMapperModel model)
        {
            return this.OperaterConfirm(() =>
            {
                return FilterFactory.IndexMapper.Operater(model);
            }, null, () =>
            {
                return DALFactory.IndexMapper.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }));
        }

        private ActionResult GetJsonText(Dictionary<int, List<DBKeyValueModel>> KeyValueDict)
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
                        stringBuilder.Append("{\\\"key\\\":");
                        stringBuilder.Append(keyValueList[index].Key);
                        stringBuilder.Append(",\\\"value\\\":\\\"");
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
            return this.Content(stringBuilder.ToString());
        }

        #region 每个项目这儿都会不同，所以需要修改此方法
        private Dictionary<int, List<DBKeyValueModel>> GetIndexTypeMapperKeyValueDict(string channelCode)
        {
            Dictionary<int, List<DBKeyValueModel>> resultDict = new Dictionary<int, List<DBKeyValueModel>>();

            List<ViewTreeTopicTypeModel> topicTypeModelList = TreeHelper.ToMenuList<ViewTreeTopicTypeModel>(DALFactory.TopicType.TreeList());
            if (topicTypeModelList != null && topicTypeModelList.Count > 0)
            {
                resultDict.Add(IndexMapperTypeEnum.TOPIC, new List<DBKeyValueModel>());
                foreach(ViewTreeTopicTypeModel modelItem in topicTypeModelList)
                {
                    resultDict[IndexMapperTypeEnum.TOPIC].Add(new DBKeyValueModel() { Key = modelItem.IdentityID.ToString(), Value = modelItem.LayerName });
                }
            }

            List<DBLinkFriendTypeModel> linkTypeModelList = DALFactory.LinkFriendType.List();
            if(linkTypeModelList != null && linkTypeModelList.Count > 0)
            {
                resultDict.Add(IndexMapperTypeEnum.LINKFRIEND, new List<DBKeyValueModel>());
                foreach(DBLinkFriendTypeModel modelItem in linkTypeModelList)
                {
                    resultDict[IndexMapperTypeEnum.LINKFRIEND].Add(new DBKeyValueModel() { Key = modelItem.IdentityID.ToString(), Value = modelItem.TypeName });
                }
            }

            return resultDict;
        }
        #endregion
    }
}