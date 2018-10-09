using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoreSuperManager.DAL;
using MoreSuperManager.FILTER;
using MoreSuperManager.MODEL;
using Helper.Core.Library;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class LinkFriendTypeController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string channelCode = "", string searchKey = "", int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBLinkFriendTypeFullModel> modelList = DALFactory.LinkFriendType.Page(this.GetChannelCode(channelCode), searchKey, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, ChannelCode = channelCode, SearchKey = searchKey }), DALFactory.Channel.ChannelList(), channelCode);

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
            return View("Edit", identityID > 0 ? DALFactory.LinkFriendType.Select(identityID) : null);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.LinkFriendType.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBLinkFriendTypeModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBLinkFriendTypeModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.LinkFriendType.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBLinkFriendTypeModel model)
        {
            this.SetChannelCode<DBLinkFriendTypeModel>(model);

            return this.OperaterConfirm(() =>
            {
                return FilterFactory.LinkFriendType.Operater(model);
            }, () =>
            {
                return DALFactory.LinkFriendType.Exists(model.ChannelCode, model.TypeName, model.IdentityID);
            }, () =>
            {
                return DALFactory.LinkFriendType.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }), "类别名称已存在！");
        }
    }
}