using MoreSuperManager.DAL;
using MoreSuperManager.ENUM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class SettingController : BaseManagerController
    {
        [RoleActionFilter]
        public ActionResult Set()
        {
            return View();
        }

        [RoleActionFilter]
        public ActionResult Bak()
        {
            return this.Operater(null, null, () =>
            {
                return TaskHelper.ExecuteBakDataBase();
            }, Url.Action("Set"), Url.Action("Set"), null, "备份成功！", "备份失败！");
        }

        [RoleActionFilter]
        public ActionResult RoleClone()
        {
            ViewBag.SourceRoleList = DALFactory.Role.List();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult SetOperater(string bakCron, string bakPath, string version, string managerTitle, string managerPageSize, string uploadImageExt, string uploadImageMaxSize, string uploadVideoExt, string uploadVideoMaxSize, string uploadFileExt, string uploadFileMaxSize, string logOpenStatus, string authOpenStatus, string attachOpenStatus)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                { SettingTypeEnum.BAKCRON, bakCron },
                { SettingTypeEnum.BAKPATH, bakPath },
                { SettingTypeEnum.VERSION, version },
                { SettingTypeEnum.MANAGERTITLE, managerTitle },
                { SettingTypeEnum.MANAGERPAGESIZE, managerPageSize },
                { SettingTypeEnum.UPLOADIMAGEEXT, uploadImageExt },
                { SettingTypeEnum.UPLOADIMAGEMAXSIZE, uploadImageMaxSize },
                { SettingTypeEnum.UPLOADVIDEOEXT, uploadVideoExt },
                { SettingTypeEnum.UPLOADVIDEOMAXSIZE, uploadVideoMaxSize },
                { SettingTypeEnum.UPLOADFILEEXT, uploadFileExt },
                { SettingTypeEnum.UPLOADFILEMAXSIZE, uploadFileMaxSize },
            };
            if (!string.IsNullOrEmpty(logOpenStatus))
            {
                dict.Add(SettingTypeEnum.LOGOPENSTATUS, "1");
            }
            if (!string.IsNullOrEmpty(authOpenStatus))
            {
                dict.Add(SettingTypeEnum.AUTHOPENSTATUS, "1");
            }
            if (!string.IsNullOrEmpty(attachOpenStatus))
            {
                dict.Add(SettingTypeEnum.ATTACHOPENSTATUS, "1");
            }
            return this.Operater(null, null, () =>
            {
                return SettingHelper.Set(dict);
            }, Url.Action("Set"), Url.Action("Set"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult RoleCloneOperater(int sourceRoleID, int targetRoleID)
        {
            return this.Operater(null, () =>
            {
                if (sourceRoleID == targetRoleID) return true;
                return false;
            }, () =>
            {
                return DALFactory.Role.Clone(sourceRoleID, targetRoleID);
            }, Url.Action("RoleClone"), Url.Action("RoleClone"), "原始角色与目标角色不能相同！");
        }
    }
}