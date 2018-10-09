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
    public class ProjectController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string searchKey = "", int projectType = -1, int flowID = -1, int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBProjectFullModel> modelList = DALFactory.Project.Page(searchKey, projectType, flowID, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, SearchKey = searchKey, ProjectType = projectType, FlowID = flowID }), null, null);
            ViewData["ProjectType"] = projectType;
            ViewData["FlowID"] = flowID;
            ViewBag.ProjectTypeList = DALFactory.ProjectType.List();
            ViewBag.FlowList = DALFactory.Flow.List();
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
            ViewBag.ProjectTypeList = DALFactory.ProjectType.List();
            ViewBag.FlowList = DALFactory.Flow.List();
            return View("Edit", identityID > 0 ? DALFactory.Project.Select(identityID) : null);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.Project.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBProjectModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBProjectModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.Project.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBProjectModel model)
        {
            return this.OperaterConfirm(() =>
            {
                return FilterFactory.Project.Operater(model);
            }, null, () =>
            {
                return DALFactory.Project.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }));
        }
    }
}