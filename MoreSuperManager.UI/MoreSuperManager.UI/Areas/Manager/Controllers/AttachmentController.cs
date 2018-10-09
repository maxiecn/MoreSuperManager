using Helper.Core.Library;
using MoreSuperManager.DAL;
using MoreSuperManager.ENUM;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class AttachmentController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string searchKey = "", string attachmentType = null, int pageIndex = 1)
        {
            if (attachmentType == "-1") attachmentType = "";

            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBAttachmentModel> modelList = DALFactory.Attachment.Page(searchKey, attachmentType, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, SearchKey = searchKey, AttachmentType = attachmentType }), null, null);

            ViewBag.AttachmentTypeList = new List<DBKeyValueModel>()
            {
                new DBKeyValueModel(){ Key = UploadTypeEnum.COVER, Value = "封面" },
                new DBKeyValueModel(){ Key = UploadTypeEnum.IMAGE, Value = "图片" },
                new DBKeyValueModel(){ Key = UploadTypeEnum.FILE, Value = "附件" },
            };
            ViewData["AttachmentType"] = attachmentType;
            return View(modelList);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                bool status = this.DeleteAttachmentFile(identityID.ToString());
                if (status)
                {
                    return DALFactory.Attachment.Delete(identityID);
                }
                return status;
            }, Url.Action("List"));
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                bool status = this.DeleteAttachmentFile(identityIDList);
                if (status)
                {
                    return DALFactory.Attachment.DeleteMore(identityIDList);
                }
                return status;
            }, null, Url.Action("List"));
        }

        [NonAction]
        private bool DeleteAttachmentFile(string identityIDList)
        {
            try
            {
                List<DBAttachmentModel> modelList = DALFactory.Attachment.List(identityIDList);
                if (modelList != null && modelList.Count > 0)
                {
                    foreach (DBAttachmentModel modelItem in modelList)
                    {
                        string filePath = Server.MapPath(string.Format("~/{0}", modelItem.AttachmentPath));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}