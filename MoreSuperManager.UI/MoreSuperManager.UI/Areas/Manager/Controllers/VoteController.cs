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
        public ActionResult List(string searchKey = "", int voteType = -1, int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBVoteFullModel> modelList = DALFactory.Vote.Page(searchKey, voteType, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, SearchKey = searchKey, VoteType = voteType }), null, null);
            ViewData["VoteType"] = voteType;
            ViewBag.VoteTypeList = DALFactory.VoteType.List();
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
            ViewBag.VoteTypeList = DALFactory.VoteType.List();
            ViewBag.VoteItemList = identityID > 0 ? this.GetVoteJsonText(identityID) : "{}";
            return View("Edit", identityID > 0 ? DALFactory.Vote.Select(identityID) : null);
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
        private string GetVoteJsonText(int identityID)
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

        [NonAction]
        private ActionResult AddOrEditOperater(DBVoteModel model, string voteItemList)
        {
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
    }
}