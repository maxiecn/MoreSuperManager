using Helper.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MoreSuperManager.ENUM;
using MoreSuperManager.MODEL;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    [RoleAuthorize]
    public class BaseController : Controller
    {
        protected ViewUserModel viewUserModel;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            // 如果登录方式是 Cookie
            if (ConfigHelper.TokenType == TokenTypeEnum.COOKIE)
            {
                this.viewUserModel = CookieHelper.GetCookieT<ViewUserModel>(ConfigHelper.TokenName);
            }
            else
            {
                this.viewUserModel = this.Session[ConfigHelper.TokenName] as ViewUserModel;
            }
            // 设置登录信息供 VIEW 使用
            ViewBag.LoginUserModel = this.viewUserModel;
        }

        protected virtual ActionResult RedirectToUrl(string content, string url, bool status = true, bool parent = false)
        {
            return this.Redirect(Url.Action("Error", "Common", new { note = content, url = url, status = status, parent = parent }));
        }

        protected virtual ActionResult RedirectToUrl(string content, string okUrl, string cancelUrl, bool status = true, bool parent = false)
        {
            return this.Redirect(Url.Action("Confirm", "Common", new { note = content, okUrl = okUrl, cancelUrl = cancelUrl, status = status, parent = parent }));
        }

        protected virtual ActionResult RedirectToLoginUrl()
        {
            return this.Redirect(FormsAuthentication.LoginUrl);
        }
    }
}