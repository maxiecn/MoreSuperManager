using Helper.Core.Library;
using MoreSuperManager.DAL;
using MoreSuperManager.ENUM;
using MoreSuperManager.FILTER;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class MenuController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string channelCode = "", string searchKey = "")
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);

            List<ViewTreeMenuModel> dataList = TreeHelper.ToMenuList<ViewTreeMenuModel>(DALFactory.Menu.All(this.GetChannelCode(channelCode), searchKey));
            this.InitViewData(searchKey, 0, "", ConstHelper.ChannelList(DALFactory.Channel.ChannelList()), channelCode);

            ViewBag.ActionTypeList = DALFactory.ActionType.List();
            ViewBag.ModuleList = DALFactory.Module.List();

            return View(dataList);
        }

        [RoleActionFilter]
        public ActionResult Add(int parentID = 0)
        {
            return this.Edit(0, parentID);
        }

        [RoleActionFilter]
        public ActionResult Edit(int identityID = 0, int parentID = 0)
        {
            DBMenuModel model = identityID > 0 ? DALFactory.Menu.Select(identityID) : null;

            int menuID = model != null ? model.IdentityID : 0;
            string channelCode = null;
            List<DBChannelModel> channelModelList = null;

            List<ViewTreeMenuModel> menuModelList = TreeHelper.ToMenuList<ViewTreeMenuModel>(DALFactory.Menu.TreeList());
            List<DBModuleModel> moduleModelList = DALFactory.Module.List();

            this.InitChannelViewData<DBMenuModel>(model, (p, k) =>
            {
                channelCode = p;
                channelModelList = k;
            }, () =>
            {
                return DALFactory.Channel.ChannelList();
            });

            List<DBModuleModel> moduleList = moduleModelList.Where(p => p.ChannelCode == channelCode).ToList();

            ViewBag.MenuJsonText = this.GetMenuJsonText(channelModelList, menuModelList, menuID);
            ViewBag.TreeMenuList = menuModelList.Where(p => p.ChannelCode == channelCode && p.IdentityID != menuID).ToList();

            ViewBag.ModuleJsonText = this.GetModuleJsonText(channelModelList, moduleModelList);
            ViewBag.ModuleList = moduleList;

            if (model != null && !string.IsNullOrEmpty(model.BelongModule) && moduleList.Count > 0)
            {
                DBModuleModel moduleModel = moduleList.Where(p => p.ModuleCode == model.BelongModule).FirstOrDefault();
                if (moduleModel != null)
                {
                    ViewBag.ActionTypeList = DALFactory.ActionType.List(moduleModel.ActionList);
                }
            }
            ViewBag.FlowStepList = DALFactory.FlowStep.List();
            ViewBag.ParentID = parentID;

            return View("Edit", model);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.Menu.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBMenuModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBMenuModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.Menu.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        #region 获取页面菜单 JSON 数据
        private string GetMenuJsonText(List<DBChannelModel> channelModelList, List<ViewTreeMenuModel> modelList, int identityID)
        {
            return ConstHelper.GetJsonText<ViewTreeMenuModel>(channelModelList, modelList, (ViewTreeMenuModel model) =>
            {
                return model.IdentityID;
            }, (ViewTreeMenuModel model) =>
            {
                return model.LayerName;
            }, true, (ViewTreeMenuModel model) =>
            {
                return model.IdentityID == identityID;
            }, new DBKeyValueModel() { Key = "0", Value = "根级菜单" });
        }
        private string GetModuleJsonText(List<DBChannelModel> channelModelList, List<DBModuleModel> modelList)
        {
            return ConstHelper.GetJsonText<DBModuleModel>(channelModelList, modelList, (DBModuleModel model) =>
            {
                return model.ModuleCode;
            }, (DBModuleModel model) =>
            {
                return model.ModuleName;
            }, true, null, new DBKeyValueModel() { Key = "-1", Value = "无" });
        }
        public ActionResult GetActionTypeJsonText()
        {
            List<DBModuleModel> moduleList = DALFactory.Module.List();
            if (moduleList == null) return this.Content("{}");

            List<DBActionTypeModel> actionList = DALFactory.ActionType.List();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            int menuIndex = 0;
            foreach (DBModuleModel moduleModel in moduleList)
            {
                stringBuilder.Append("\\\"");
                stringBuilder.Append(moduleModel.ModuleCode);
                stringBuilder.Append("-");
                stringBuilder.Append(moduleModel.ChannelCode);
                stringBuilder.Append("\\\":{");
                if (!string.IsNullOrEmpty(moduleModel.ActionList))
                {
                    int actionIndex = 0;
                    List<string> actionDataList = StringHelper.ToList<string>(moduleModel.ActionList, ",", true);
                    foreach (string actionData in actionDataList)
                    {
                        DBActionTypeModel actionModel = actionList.Where(p => p.TypeCode == actionData).FirstOrDefault();
                        if (actionModel != null)
                        {
                            stringBuilder.Append("\\\"");
                            stringBuilder.Append(actionData);
                            stringBuilder.Append("\\\":\\\"");
                            stringBuilder.Append(actionModel.TypeName);
                            stringBuilder.Append("\\\"");
                            if (actionIndex < actionDataList.Count - 1)
                            {
                                stringBuilder.Append(",");
                            }
                        }
                        actionIndex++;
                    }
                }
                stringBuilder.Append("}");
                if (menuIndex < moduleList.Count - 1)
                {
                    stringBuilder.Append(",");
                }
                menuIndex++;
            }
            stringBuilder.Append("}");

            return this.Content(stringBuilder.ToString());
        }
        #endregion

        [NonAction]
        private ActionResult AddOrEditOperater(DBMenuModel model)
        {
            string menuActionList = this.Request.Form["menuActionList"];
            model.ActionList = menuActionList;
            if (model.ParentID != 0) model.MenuIcon = "";

            this.SetChannelCode<DBMenuModel>(model);

            return this.OperaterConfirm(() =>
            {
                return FilterFactory.Menu.Operater(model, menuActionList);
            }, null, () =>
            {
                return DALFactory.Menu.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }));
        }
    }
}