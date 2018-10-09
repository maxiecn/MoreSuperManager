using Helper.Core.Library;
using MoreSuperManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class InitController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Init(string userCode, string nickName, string userPassword)
        {
            bool result = DALFactory.Init.Init(userCode, nickName, EncryptHelper.MD5(userPassword, ConfigHelper.ConfuseKey), Server.MapPath("~/Init/"));
            if(result)
            {
                return this.Redirect(Url.Action("Error", "Common", new { note = "初始化成功！", url = Url.Action("Index", "Home"), status = true, parent = false, hasParent = false }));
            }
            else
            {
                return this.Redirect(Url.Action("Error", "Common", new { note = "初始化失败！", url = Url.Action("Index", "Init"), status = false, parent = false, hasParent = false }));
            }
        }
    }
}