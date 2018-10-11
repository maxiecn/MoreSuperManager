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
    public class TopicTypeController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string channelCode = "", string searchKey = "")
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);

            List<DBChannelModel> channelModelList = this.IsSuperManager ? DALFactory.Channel.ChannelList() : null;
            if (string.IsNullOrEmpty(channelCode) && (channelModelList != null && channelModelList.Count > 0)) channelCode = channelModelList[0].ChannelCode;

            // 设置树形菜单数据
            List<ViewTreeTopicTypeModel> dataList = TreeHelper.ToMenuList<ViewTreeTopicTypeModel>(DALFactory.TopicType.All(this.GetChannelCode(channelCode), searchKey));
            this.InitViewData(searchKey, 0, "", channelModelList, channelCode);
            return View(dataList);
        }

        [RoleActionFilter]
        public ActionResult Add(int parentID = 0)
        {
            return this.Edit(0, parentID);
        }

        [RoleActionFilter]
        public ActionResult Edit(int identityID = 0, int parentID = 0)
        {
            DBTopicTypeModel model = identityID > 0 ? DALFactory.TopicType.Select(identityID) : null;

            int topicTypeID = model != null ? model.IdentityID : 0;
            string channelCode = null;
            List<DBChannelModel> channelModelList = null;

            this.InitChannelViewData<DBTopicTypeModel>(model, (p, k) =>
            {
                channelCode = p;
                channelModelList = k;
            }, () =>
            {
                return DALFactory.Channel.ChannelList();
            });

            List<DBTopicTypeModel> topicTypeModelList = DALFactory.TopicType.List(0);

            ViewBag.TopicTypeJsonText = this.GetTopicTypeJsonText(channelModelList, topicTypeModelList, topicTypeID);
            ViewBag.TopicTypeList = topicTypeModelList.Where(p => p.ChannelCode == channelCode && p.IdentityID != topicTypeID).ToList();

            ViewBag.ParentID = parentID;
            return View("Edit", model);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.TopicType.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBTopicTypeModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBTopicTypeModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.Menu.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBTopicTypeModel model)
        {
            this.SetChannelCode<DBTopicTypeModel>(model);

            return this.OperaterConfirm(() =>
            {
                return FilterFactory.TopicType.Operater(model);
            }, null, () =>
            {
                return DALFactory.TopicType.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID, parentID = model.ParentID }));
        }

        private string GetTopicTypeJsonText(List<DBChannelModel> channelModelList, List<DBTopicTypeModel> modelList, int identityID)
        {
            return ConstHelper.GetJsonText<DBTopicTypeModel>(channelModelList, modelList, (DBTopicTypeModel model) =>
            {
                return model.IdentityID;
            }, (DBTopicTypeModel model) =>
            {
                return model.TypeName;
            }, null, false, (DBTopicTypeModel model) =>
            {
                return model.IdentityID == identityID;
            }, new DBKeyValueModel() { Key = "0", Value = "根级类别" });
        }
    }
}