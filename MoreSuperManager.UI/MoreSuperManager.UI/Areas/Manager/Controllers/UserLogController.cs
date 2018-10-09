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
    public class UserLogController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string searchKey = "", int loginStatus = -1, int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBUserLogModel> modelList = DALFactory.UserLog.Page(searchKey, loginStatus, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, SearchKey = searchKey, LoginStatus = loginStatus }), null, null);
            ViewBag.StatusTypeList = new List<DBKeyValueModel>()
            {
                new DBKeyValueModel(){ Key = LoginStatusTypeEnum.FAILED.ToString(), Value = "失败" },
                new DBKeyValueModel(){ Key = LoginStatusTypeEnum.SUCCESS.ToString(), Value = "成功" },
            };
            ViewData["LoginStatus"] = loginStatus;
            return View(modelList);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.UserLog.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.UserLog.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }
    }
}