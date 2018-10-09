using Helper.Core.Library;
using MoreSuperManager.ENUM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class CommonController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="note">提示内容</param>
        /// <param name="url">跳转 URL</param>
        /// <param name="status">操作结果</param>
        /// <param name="parent">是否框架跳转</param>
        /// <param name="hasParent">是否有父级页面</param>
        /// <returns></returns>
        public ActionResult Error(string note = null, string url = null, bool status = true, bool parent = false, bool hasParent = true)
        {
            ViewBag.Note = note;
            ViewBag.Url = url;
            ViewBag.Status = status;
            ViewBag.Parent = parent;
            ViewBag.HasParent = hasParent;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="note">提示内容</param>
        /// <param name="okUrl">确定跳转 URL</param>
        /// <param name="cancelUrl">取消跳转 URL</param>
        /// <param name="status">操作结果</param>
        /// <param name="parent">是否框架跳转</param>
        /// <param name="hasParent">是否有父级页面</param>
        /// <returns></returns>
        public ActionResult Confirm(string note = null, string okUrl = null, string cancelUrl = null, bool status = true, bool parent = false, bool hasParent = true)
        {
            ViewBag.Note = note;
            ViewBag.OkUrl = okUrl;
            ViewBag.CancelUrl = cancelUrl;
            ViewBag.Parent = parent;
            ViewBag.HasParent = hasParent;
            return View();
        }
    }
}