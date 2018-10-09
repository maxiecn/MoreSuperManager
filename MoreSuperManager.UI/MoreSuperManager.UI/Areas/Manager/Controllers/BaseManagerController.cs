using Helper.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoreSuperManager.DAL;
using MoreSuperManager.ENUM;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class BaseManagerController : BaseController
    {
        #region 提示内容
        protected const string SUCCESS_NOTE = "数据保存成功！";
        protected const string ERROR_NOTE = "数据保存失败！";
        protected const string SUCCESS_ADDANDCONTINUE_NOTE = "数据保存成功，是否继续添加新数据？";
        protected const string SUCCESS_DELETE_NOTE = "数据删除成功！";
        protected const string ERROR_DELETE_NOTE = "数据删除失败！";

        protected const string EXISTS_NOTE = "数据已存在！";
        protected const string NULL_NOTE = "数据不可为空！";
        #endregion

        [NonAction]
        protected virtual ActionResult Delete(Func<bool> callback, string url)
        {
            bool result = false;
            if (callback != null) result = callback();
            if (result)
            {
                return this.RedirectToUrl(SUCCESS_DELETE_NOTE, url, true);
            }
            else
            {
                return this.RedirectToUrl(ERROR_DELETE_NOTE, url, false);
            }
        }

        [NonAction]
        protected virtual ActionResult Operater(Func<string> filterCallback, Func<bool> existsCallback, Func<bool> callback, string successUrl, string errorUrl, string existsNote = EXISTS_NOTE, string successNote = SUCCESS_NOTE, string errorNote = ERROR_NOTE, bool filterParent = false, bool existsParent = false, bool successParent = false, bool errorParent = false, string okUrl = null, string okNote = null)
        {
            string filterText = null;
            if (filterCallback != null) filterText = filterCallback();

            if (!string.IsNullOrEmpty(filterText))
            {
                return this.RedirectToUrl(filterText, errorUrl, false, filterParent);
            }

            if (existsCallback != null && existsCallback())
            {
                return this.RedirectToUrl(existsNote, errorUrl, false, existsParent);
            }

            bool result = false;
            if (callback != null) result = callback();

            if (result)
            {
                if (string.IsNullOrEmpty(okUrl))
                {
                    return this.RedirectToUrl(successNote, successUrl, result, successParent);
                }
                else
                {
                    return this.RedirectToUrl(okNote, okUrl, successUrl, result, successParent);
                }
            }
            else
            {
                return this.RedirectToUrl(errorNote, errorUrl, result, errorParent);
            }
        }

        protected virtual ActionResult OperaterConfirm(Func<string> filterCallback, Func<bool> existsCallback, Func<bool> callback, string successUrl, string okUrl, string errorUrl, string existsNote = EXISTS_NOTE, string okNote = SUCCESS_ADDANDCONTINUE_NOTE, string successNote = SUCCESS_NOTE, string errorNote = ERROR_NOTE, bool filterParent = false, bool existsParent = false, bool successParent = false, bool errorParent = false)
        {
            return this.Operater(filterCallback, existsCallback, callback, successUrl, errorUrl, existsNote, successNote, errorNote, filterParent, existsParent, successParent, errorParent, okUrl, okNote);
        }

        [NonAction]
        protected virtual ActionResult More(Func<string, bool> deleteCallback, Func<string, string, bool> callback, string url)
        {
            string identityIDList = this.Request["identityIDList"].ToString();
            string operaterType = this.Request["operaterType"].ToString();

            if (string.IsNullOrEmpty(identityIDList))
            {
                return this.RedirectToUrl(NULL_NOTE, url, false);
            }

            string note = "";

            bool result = false;
            if (operaterType == OperaterTypeEnum.DELETE)
            {
                if (deleteCallback != null) result = deleteCallback(identityIDList);
            }
            else
            {
                if (callback != null) result = callback(identityIDList, operaterType);
            }
            if (result)
            {
                if (operaterType == OperaterTypeEnum.DELETE)
                {
                    note = SUCCESS_DELETE_NOTE;
                }
                else if (operaterType == OperaterTypeEnum.CHECKED)
                {
                    note = SUCCESS_NOTE;
                }
            }
            else
            {
                if (operaterType == OperaterTypeEnum.DELETE)
                {
                    note = ERROR_DELETE_NOTE;
                }
                else if (operaterType == OperaterTypeEnum.CHECKED)
                {
                    note = ERROR_NOTE;
                }
            }
            return this.RedirectToUrl(note, url, result);
        }

        [NonAction]
        protected ActionResult UploadOperater(Func<bool> callback, string type, string fromType, string CKEditorFuncNum = null, string moduleName = "Topics")
        {
            try
            {
                string error = "";
                // 如果没有上传权限
                bool authStatus = false;
                // 验证上传权限
                if (callback != null) authStatus = callback();
                if (!authStatus)
                {
                    error = "您没有上传权限！";
                }
                // 如果未提交上传文件
                if (this.Request.Files.Count == 0)
                {
                    error = "未提交上传文件！";
                }

                HttpPostedFileBase upload = this.Request.Files[0];
                string suffix = FileHelper.GetSuffix(upload.FileName).ToLower();

                string extensionData = null;
                int maxSize = 0;

                // 根据类型读取配置
                if (type == UploadTypeEnum.COVER || type == UploadTypeEnum.IMAGE)
                {
                    extensionData = SettingHelper.UploadImageExt;
                    maxSize = SettingHelper.UploadImageMaxSize;
                }
                else if (type == UploadTypeEnum.VIDEO)
                {
                    extensionData = SettingHelper.UploadVideoExt;
                    maxSize = SettingHelper.UploadVideoMaxSize;
                }
                else
                {
                    extensionData = SettingHelper.UploadFileExt;
                    maxSize = SettingHelper.UploadFileMaxSize;
                }

                string maxSizeText = ((maxSize * 0.1f) / (1024 * 1024 * 0.1f)).ToString("f1");
                // 如果上传文件大小超过配置数据
                if (upload.ContentLength > maxSize)
                {
                    error = string.Format("上传文件大小不能超过 {0} M！", maxSizeText);
                }
                List<string> extensionDataList = StringHelper.ToList<string>(extensionData, "|");
                // 验证上传文件格式是否正确
                if (!UploadVerifyHelper.Verify(upload, extensionDataList.ToArray(), UploadVerifyHelper.GetSerialTypeListByString(extensionData).ToArray()))
                {
                    error = string.Format("只能上传 {0} 的文件！", extensionData.ToUpper());
                }

                if (!string.IsNullOrEmpty(error))
                {
                    if (fromType == UploadFromTypeEnum.FILE)
                    {
                        return this.Json(new { Error = error, Data = "" });
                    }
                    else
                    {
                        return this.Content("<script type=\"text/javascript\">alert('" + error + "！');</script>");
                    }
                }

                // 上传文件目录
                string directoryName = "";
                if (type == UploadTypeEnum.COVER)
                {
                    directoryName = "Covers";
                }
                else if (type == UploadTypeEnum.IMAGE)
                {
                    directoryName = "Images";
                }
                else if (type == UploadTypeEnum.VIDEO)
                {
                    directoryName = "Videos";
                }
                else
                {
                    directoryName = "Files";
                }

                string filePath = string.Format("/Upload/{0}/{1}/{2}/{3}{4}", moduleName, directoryName, DateTime.Now.ToString("yyyyMMdd"), System.Guid.NewGuid().ToString("N"), suffix);
                string uploadFilePath = Server.MapPath("~" + filePath);
                // 创建目录
                FileHelper.CreateDirectory(uploadFilePath);
                // 保存上传文件
                upload.SaveAs(uploadFilePath);

                // 记录到附件
                bool operaterResult = DALFactory.Attachment.Operater(new MODEL.DBAttachmentModel()
                {
                    AttachmentType = type,
                    AttachmentName = upload.FileName,
                    AttachmentSize = upload.ContentLength,
                    AttachmentPath = filePath
                });

                if (fromType != UploadFromTypeEnum.FILE)
                {
                    return Content("<script type=\"text/javascript\">window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + filePath + "\");</script>");
                }
                else
                {
                    return this.Json(new { Error = "", Data = filePath });
                }
            }
            catch (Exception ex)
            {
                if (fromType != UploadFromTypeEnum.FILE)
                {
                    return this.Content("<script type=\"text/javascript\">alert('" + ex.Message + "！');</script>");
                }
                else
                {
                    return this.Json(new { Error = ex.Message, Data = "" });
                }
            }
        }
    }
}