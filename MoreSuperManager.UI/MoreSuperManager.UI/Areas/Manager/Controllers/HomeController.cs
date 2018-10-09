using Helper.Core.Library;
using MoreSuperManager.DAL;
using MoreSuperManager.ENUM;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class HomeController : BaseManagerController
    {
        public ActionResult Index()
        {
            List<DBMenuModel> modelList = DataHelper.GetRoleMenuModelList(this.viewUserModel.RoleID.ToString());
            // 测试 List<DBMenuModel> modelList = DALFactory.Menu.RoleList(MenuStatusTypeEnum.SHOW);
            if (modelList == null)
            {
                return this.RedirectToLoginUrl();
            }
            ViewBag.MenuData = this.GetMenuHtmlText(modelList, modelList.Where(p => p.ParentID == 0).ToList());
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        #region 获取菜单 HTML
        public string GetMenuHtmlText(List<DBMenuModel> modelList, List<DBMenuModel> parentModelList, int layerIndex = 1)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (parentModelList != null && parentModelList.Count > 0)
            {
                // 按 MenuSort 排序
                parentModelList = parentModelList.OrderByDescending(p => p.MenuSort).ToList();
                foreach (DBMenuModel model in parentModelList)
                {
                    List<DBMenuModel> childModelList = modelList.Where(p => p.ParentID == model.IdentityID).ToList();
                    stringBuilder.Append("<li id=\"tree_trunk_");
                    stringBuilder.Append(model.IdentityID);
                    stringBuilder.Append("\" data-layer=\"");
                    stringBuilder.Append(layerIndex);
                    stringBuilder.Append("\">");
                    if ((layerIndex <= 1) || (childModelList != null && childModelList.Count > 0))
                    {
                        stringBuilder.Append("<a ");
                        if (layerIndex > 1 && childModelList != null && childModelList.Count > 0)
                        {
                            stringBuilder.Append(" style = \"padding-bottom:0px;\" ");
                        }
                        if (childModelList != null && childModelList.Count > 0)
                        {
                            stringBuilder.Append(" data-trunk=\"");
                            stringBuilder.Append(model.IdentityID);
                            stringBuilder.Append("\"");
                        }
                        stringBuilder.Append(" class=\"frame-nav-trunk\">");
                    }
                    else
                    {
                        stringBuilder.Append("<a data-url=\"");
                        stringBuilder.Append(model.MenuUrl);
                        stringBuilder.Append("\" data-title=\"");
                        stringBuilder.Append(Server.HtmlEncode(model.MenuName));
                        stringBuilder.Append("\" data-node=\"");
                        stringBuilder.Append(model.IdentityID);
                        stringBuilder.Append("\" class=\"frame-nav-node\">");
                    }
                    if (layerIndex <= 1 && !string.IsNullOrEmpty(model.MenuIcon))
                    {
                        stringBuilder.Append("<i class=\"");
                        stringBuilder.Append(model.MenuIcon);
                        stringBuilder.Append("\"></i>");
                    }
                    stringBuilder.Append(Server.HtmlEncode(model.MenuName));
                    if (childModelList != null && childModelList.Count > 0)
                    {
                        stringBuilder.Append("<b id=\"tree_icon_");
                        stringBuilder.Append(model.IdentityID);
                        stringBuilder.Append("\" class=\"glyphicon glyphicon-chevron-left pull-right\"></b>");
                    }
                    stringBuilder.Append("</a>");
                    if (childModelList != null && childModelList.Count > 0)
                    {
                        stringBuilder.Append("\r\n<ul class=\"list-unstyled frame-nav-ul\" id=\"tree_node_");
                        stringBuilder.Append(model.IdentityID);
                        stringBuilder.Append("\" style=\"");
                        if (layerIndex <= 1)
                        {
                            stringBuilder.Append("margin-top:-15px;");
                        }
                        stringBuilder.Append("\">\r\n");
                        // 递归遍历子元素
                        stringBuilder.Append(this.GetMenuHtmlText(modelList, childModelList, layerIndex + 1));
                        stringBuilder.Append("</ul>\r\n");
                    }
                    stringBuilder.Append("</li>\r\n");
                }
            }
            return stringBuilder.ToString();
        }
        #endregion
    }
}