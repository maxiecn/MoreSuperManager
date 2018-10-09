using Helper.Core.Library;
using MoreSuperManager.ENUM;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MoreSuperManager.UI
{
    /// <summary>
    /// 菜单请求验证
    /// </summary>
    public class RoleMenuFilterAttribute : FilterAttribute, IActionFilter
    {
        protected string areaName;
        protected string controllerName;
        protected string actionName;
        protected IDictionary<string, object> paramDict;

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.areaName = (filterContext.RouteData.DataTokens["area"] == null ? "" : filterContext.RouteData.DataTokens["area"].ToString()).ToLower();
            this.controllerName = (filterContext.RouteData.Values["controller"]).ToString().ToLower();
            this.actionName = (filterContext.RouteData.Values["action"]).ToString().ToLower();
            this.paramDict = filterContext.ActionParameters;
            
            this.Valid(filterContext);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext) { }

        protected virtual void Valid(ActionExecutingContext filterContext)
        {
            ViewUserModel viewUserModel = null;
            if (ConfigHelper.TokenType == TokenTypeEnum.COOKIE)
            {
                viewUserModel = CookieHelper.GetCookieT<ViewUserModel>(ConfigHelper.TokenName);
            }
            else
            {
                viewUserModel = filterContext.HttpContext.Session[ConfigHelper.TokenName] as ViewUserModel;
            }
            if (viewUserModel == null)
            {
                this.RedirectToLoginUrl(filterContext);
            }
            // 如果未开启授权验证
            if (!SettingHelper.AuthOpenStatus) return;

            bool authStatus = DataHelper.AuthMenu(viewUserModel.RoleID.ToString(), string.Format("/{0}/{1}/{2}", this.areaName, this.controllerName, this.actionName), this.paramDict);
            if (!authStatus)
            {
                this.RedirectToLoginUrl(filterContext);
            }
        }

        protected virtual void RedirectToLoginUrl(ActionExecutingContext filterContext)
        {
            filterContext.Result = new EmptyResult();
            filterContext.HttpContext.Response.Redirect(string.Format("/Manager/Common/Error?note=权限不足&url={0}&status=false&parent=true", FormsAuthentication.LoginUrl));
        }
    }
}