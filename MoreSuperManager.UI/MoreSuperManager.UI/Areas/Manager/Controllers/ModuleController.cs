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
    public class ModuleController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string channelCode = "", string searchKey = "", int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBModuleFullModel> modelList = DALFactory.Module.Page(this.GetChannelCode(channelCode), searchKey, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, ChannelCode = channelCode, SearchKey = searchKey }), this.IsSuperManager ? ConstHelper.ChannelList(DALFactory.Channel.ChannelList()) : null, channelCode);

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
            if (this.IsSuperManager)
            {
                ViewBag.ChannelList = ConstHelper.ChannelList(DALFactory.Channel.List());
            }

            ViewBag.ActionTypeList = DALFactory.ActionType.List();
            return View("Edit", identityID > 0 ? DALFactory.Module.Select(identityID) : null);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.Module.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBModuleModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBModuleModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.Module.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBModuleModel model)
        {
            string actionList = this.Request["actionList"].ToString();
            model.ActionList = actionList;

            this.SetChannelCode<DBModuleModel>(model);

            return this.OperaterConfirm(() =>
            {
                return FilterFactory.Module.Operater(model);
            }, () =>
            {
                return DALFactory.Module.Exists(model.ChannelCode, model.ModuleCode, model.IdentityID);
            }, () =>
            {
                return DALFactory.Module.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }), "模块编号已存在！");
        }
    }
}