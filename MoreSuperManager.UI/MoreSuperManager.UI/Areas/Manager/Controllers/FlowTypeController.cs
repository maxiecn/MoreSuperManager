using Helper.Core.Library;
using MoreSuperManager.DAL;
using MoreSuperManager.ENUM;
using MoreSuperManager.FILTER;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class FlowTypeController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string channelCode = "", string searchKey = "", int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBFlowTypeFullModel> modelList = DALFactory.FlowType.Page(this.GetChannelCode(channelCode), searchKey, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, ChannelCode = channelCode, SearchKey = searchKey }), this.IsSuperManager ? DALFactory.Channel.ChannelList() : null, channelCode);

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
            if(this.IsSuperManager)
            {
                ViewBag.ChannelList = DALFactory.Channel.ChannelList();
            }
            return View("Edit", identityID > 0 ? DALFactory.FlowType.Select(identityID) : null);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.FlowType.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBFlowTypeModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBFlowTypeModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.FlowType.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBFlowTypeModel model)
        {
            this.SetChannelCode<DBFlowTypeModel>(model);

            return this.OperaterConfirm(() =>
            {
                return FilterFactory.FlowType.Operater(model);
            }, () =>
            {
                return DALFactory.FlowType.Exists(model.ChannelCode, model.TypeName, model.IdentityID);
            }, () =>
            {
                return DALFactory.FlowType.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }), "流程类别已存在！");
        }
    }
}