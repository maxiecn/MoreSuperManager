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
    public class MessageController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string searchKey = "", int messageStatus = -1, int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBMessageModel> modelList = DALFactory.Message.Page(searchKey, messageStatus, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, SearchKey = searchKey, MessageStatus = messageStatus }), null, null);

            ViewBag.StatusTypeList = new List<DBKeyValueModel>()
            {
                new DBKeyValueModel(){ Key = MessageStatusTypeEnum.DEFAULT.ToString(), Value = "默认" },
                new DBKeyValueModel(){ Key = MessageStatusTypeEnum.READ.ToString(), Value = "已读" },
                new DBKeyValueModel(){ Key = MessageStatusTypeEnum.REPLY.ToString(), Value = "已回复" },
            };
            ViewData["MessageStatus"] = messageStatus;
            return View(modelList);
        }

        [RoleActionFilter]
        public ActionResult Info(int identityID)
        {
            DBMessageFullModel model = DALFactory.Message.FullSelect(identityID);
            if(model == null)
            {
                return this.RedirectToLoginUrl();
            }
            // 如果是未回复状态
            if (model.MessageStatus != MessageStatusTypeEnum.REPLY)
            {
                // 设置为已读状态
                DALFactory.Message.Status(identityID, MessageStatusTypeEnum.READ);
            }
            return View(model);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.Message.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ReplyOperater(DBMessageReplyModel model)
        {
            model.UserCode = this.viewUserModel.UserCode;
            model.NickName = this.viewUserModel.NickName;

            return this.Operater(() =>
            {
                return FilterFactory.MessageReply.Operater(model);
            }, null, () =>
            {
                return DALFactory.MessageReply.Operater(model, MessageStatusTypeEnum.REPLY);
            }, Url.Action("List"), Url.Action("List"));
        }

        [HttpPost]
        public ActionResult UploadOperater(string type, string fromType, string CKEditorFuncNum = null)
        {
            // 上传文件
            return this.UploadOperater(() =>
            {
                return DataHelper.AuthAction(this.viewUserModel.RoleID.ToString(), "Message", "Upload");
            }, type, fromType, CKEditorFuncNum, "Messages", (string attachmentType, string attachmentName, int attachmentSize, string attachmentPath) =>
            {
                return DALFactory.Attachment.Operater(new MODEL.DBAttachmentModel()
                {
                    AttachmentType = attachmentType,
                    AttachmentName = attachmentName,
                    AttachmentSize = attachmentSize,
                    AttachmentPath = attachmentPath
                });
            });
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.Message.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }
    }
}