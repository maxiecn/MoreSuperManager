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
    public class NoticeController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string channelCode = "", string searchKey = "", int noticeType = -1, int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBNoticeFullModel> modelList = DALFactory.Notice.Page(this.GetChannelCode(channelCode), searchKey, noticeType, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);
            List<DBChannelModel> channelModelList = DALFactory.Channel.ChannelList();

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, ChannelCode = channelCode, SearchKey = searchKey, NoticeType = noticeType }), channelModelList, channelCode);

            ViewBag.NoticeTypeList = this.InitNoticeTypeKeyValueList(DALFactory.NoticeType.List(), channelModelList, channelCode);
            ViewData["NoticeType"] = noticeType;
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
            ViewBag.NoticeTypeList = DALFactory.NoticeType.List();
            return View("Edit", identityID > 0 ? DALFactory.Notice.Select(identityID) : null);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.Notice.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        public ActionResult UploadOperater(string type, string fromType, string CKEditorFuncNum = null)
        {
            // 上传文件
            return this.UploadOperater(() =>
            {
                return DataHelper.AuthAction(this.viewUserModel.RoleID.ToString(), "Notice", "Upload");
            }, type, fromType, CKEditorFuncNum, "Notices");
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBNoticeModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBNoticeModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.Notice.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBNoticeModel model)
        {
            return this.OperaterConfirm(() =>
            {
                return FilterFactory.Notice.Operater(model);
            }, null, () =>
            {
                return DALFactory.Notice.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }));
        }
        private List<DBKeyValueModel> InitNoticeTypeKeyValueList(List<DBNoticeTypeModel> modelList, List<DBChannelModel> channelModelList, string channelCode)
        {
            return ConstHelper.GetChannelKeyValueList<DBNoticeTypeModel>(channelModelList, modelList, channelCode, (DBNoticeTypeModel model) =>
            {
                return model.IdentityID;
            }, (DBNoticeTypeModel model) =>
            {
                return model.TypeName;
            });
        }
    }
}