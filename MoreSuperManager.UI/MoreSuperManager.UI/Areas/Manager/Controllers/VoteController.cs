using Helper.Core.Library;
using Newtonsoft.Json;
using MoreSuperManager.DAL;
using MoreSuperManager.ENUM;
using MoreSuperManager.FILTER;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class VoteController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string channelCode = "", string searchKey = "", int voteType = -1, int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBVoteFullModel> modelList = DALFactory.Vote.Page(this.GetChannelCode(channelCode), searchKey, voteType, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);
            List<DBChannelModel> channelModelList = DALFactory.Channel.ChannelList();

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, ChannelCode = channelCode, SearchKey = searchKey, VoteType = voteType }), channelModelList, channelCode);
            ViewData["VoteType"] = voteType;
            ViewBag.VoteTypeList = this.InitVoteTypeKeyValueList(DALFactory.VoteType.List(), channelModelList, this.viewUserModel.ChannelCode);
            return View(modelList);
        }

        [RoleActionFilter]
        public ActionResult Add()
        {
            return this.Edit(0);
        }

        [RoleActionFilter]
        public ActionResult Edit(int identityID = 0)
        {
            DBVoteModel model = identityID > 0 ? DALFactory.Vote.Select(identityID) : null;

            string channelCode = null;
            List<DBChannelModel> channelModelList = null;

            this.InitChannelViewData<DBVoteModel>(model, (p, k) =>
            {
                channelCode = p;
                channelModelList = k;
            }, () =>
            {
                return DALFactory.Channel.ChannelList();
            });

            List<DBVoteTypeModel> voteTypeModelList = DALFactory.VoteType.List();

            ViewBag.VoteTypeList = voteTypeModelList.Where(p => p.ChannelCode == channelCode).ToList();
            ViewBag.VoteTypeJsonText = this.GetVoteTypeJsonText(channelModelList, voteTypeModelList);

            ViewBag.VoteItemJsonText = identityID > 0 ? this.GetVoteItemJsonText(identityID) : "{}";

            return View("Edit", model);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.Vote.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBVoteModel model, string voteItemList)
        {
            return this.AddOrEditOperater(model, voteItemList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBVoteModel model, string voteItemList)
        {
            return this.AddOrEditOperater(model, voteItemList);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.Vote.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [NonAction]
        private string GetVoteItemJsonText(int identityID)
        {
            List<DBVoteItemModel> modelList = DALFactory.VoteItem.List(identityID);
            if (modelList == null || modelList.Count == 0) return "{}";

            Dictionary<string, string> itemDict = new Dictionary<string, string>();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (DBVoteItemModel model in modelList)
            {
                if (itemDict.ContainsKey(model.ItemID)) continue;
                itemDict.Add(model.ItemID, model.ItemID);

                List<string> itemContentList = modelList.Where(p => p.ItemID == model.ItemID).Select(p => p.ItemContent).ToList();

                stringBuilder.Append("{\\\"id\\\":\\\"");
                stringBuilder.Append(model.ItemID);
                stringBuilder.Append("\\\",\\\"itemType\\\":");
                stringBuilder.Append(model.ItemType);
                stringBuilder.Append(",\\\"itemTitle\\\":\\\"");
                stringBuilder.Append(model.ItemTitle);
                stringBuilder.Append("\\\",\\\"itemMaxCount\\\":");
                stringBuilder.Append(model.ItemMaxCount);
                stringBuilder.Append(",\\\"voteItemList\\\":[");
                var contentIndex = 0;
                foreach(string itemContent in itemContentList)
                {
                    stringBuilder.Append("\\\"");
                    stringBuilder.Append(itemContent);
                    stringBuilder.Append("\\\"");
                    if (contentIndex < itemContentList.Count - 1) stringBuilder.Append(",");
                    contentIndex++;
                }
                stringBuilder.Append("],\\\"isEdit\\\":false");
                stringBuilder.Append("},");
            }
            return string.Format("[{0}]", stringBuilder.ToString().TrimEnd(new char[] { ',' }));
        }
        private string GetVoteTypeJsonText(List<DBChannelModel> channelModelList, List<DBVoteTypeModel> modelList)
        {
            return ConstHelper.GetJsonText<DBVoteTypeModel>(channelModelList, modelList, (DBVoteTypeModel model) =>
            {
                return model.IdentityID;
            }, (DBVoteTypeModel model) =>
            {
                return model.TypeName;
            });
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBVoteModel model, string voteItemList)
        {
            this.SetChannelCode<DBVoteModel>(model);

            JsonSerializer jsonSerializer = new JsonSerializer();
            List<DBVoteItemModel> dataList = jsonSerializer.Deserialize(new JsonTextReader(new StringReader(voteItemList)), typeof(List<DBVoteItemModel>)) as List<DBVoteItemModel>;

            List<DBVoteItemModel> modelList = new List<DBVoteItemModel>();
            if (dataList != null && dataList.Count > 0)
            {
                foreach (DBVoteItemModel dataItem in dataList)
                {
                    // 如果是单选或者多选
                    if (dataItem.ItemType != VoteItemTypeEnum.TEXT)
                    {
                        // 拆分选项数据
                        List<string> itemContentList = StringHelper.ToList<string>(dataItem.ItemContent, ",", true);
                        foreach (string itemContent in itemContentList)
                        {
                            DBVoteItemModel modelItem = new DBVoteItemModel() { ItemID = dataItem.ItemID, ItemType = dataItem.ItemType, ItemTitle = dataItem.ItemTitle, ItemContent = itemContent };
                            // 如果是单选
                            if (dataItem.ItemType == VoteItemTypeEnum.RADIO)
                            {
                                modelItem.ItemMaxCount = 0;
                            }
                            else
                            {
                                modelItem.ItemMaxCount = dataItem.ItemMaxCount;
                            }
                            modelList.Add(modelItem);
                        }
                    }
                    else
                    {
                        modelList.Add(new DBVoteItemModel() { ItemID = dataItem.ItemID, ItemType = dataItem.ItemType, ItemTitle = dataItem.ItemTitle });
                    }
                }
            }

            return this.OperaterConfirm(() =>
            {
                if (modelList == null || modelList.Count == 0) return "投票题库数据错误！";
                return FilterFactory.Vote.Operater(model, voteItemList);
            }, null, () =>
            {
                return DALFactory.Vote.Operater(model, modelList);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("FlowDesign", new { identityID = model.IdentityID }));
        }

        private List<DBKeyValueModel> InitVoteTypeKeyValueList(List<DBVoteTypeModel> modelList, List<DBChannelModel> channelModelList, string channelCode)
        {
            return ConstHelper.GetChannelKeyValueList<DBVoteTypeModel>(channelModelList, modelList, channelCode, (DBVoteTypeModel model) =>
            {
                return model.IdentityID;
            }, (DBVoteTypeModel model) =>
            {
                return model.TypeName;
            });
        }
    }
}