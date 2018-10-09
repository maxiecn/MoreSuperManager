using Helper.Core.Library;
using MoreSuperManager.ENUM;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            ViewUserModel model = null;
            // 如果登录方式是 Cookie
            if (ConfigHelper.TokenType == TokenTypeEnum.COOKIE)
            {
                model = CookieHelper.GetCookieT<ViewUserModel>(ConfigHelper.TokenName);
            }
            else
            {
                model = httpContext.Session[ConfigHelper.TokenName] as ViewUserModel;
            }
            // 如果未登录
            if (model != null)
            {
                return true;
            }
            return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}