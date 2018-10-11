using Helper.Core.Library;
using MoreSuperManager.DAL;
using MoreSuperManager.FILTER;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class UserController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string searchKey = "", int roleID = -1, int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBUserFullModel> modelList = DALFactory.User.Page(searchKey, roleID, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, SearchKey = searchKey, RoleID = roleID }), null, null);
            ViewData["RoleID"] = roleID;
            ViewBag.RoleList = DALFactory.Role.List();
            return View(modelList);
        }

        [RoleActionFilter]
        public ActionResult Add()
        {
            return this.Edit(0);
        }

        [RoleActionFilter]
        public ActionResult Edit(int identityID = 0)
        {
            if (this.IsSuperManager)
            {
                ViewBag.RoleList = this.InitRoleKeyValueList(DALFactory.Role.List(), ConstHelper.GetChannelList(DALFactory.Channel.ChannelList()), this.viewUserModel.ChannelCode);
            }
            else
            {
                ViewBag.RoleList = this.InitRoleKeyValueList(DALFactory.Role.ChannelList(this.viewUserModel.ChannelCode), null, this.viewUserModel.ChannelCode);
            }
            return View("Edit", DALFactory.User.Select(identityID));
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.User.Delete(identityID);
            }, Url.Action("List"));
        }

        [RoleActionFilter]
        public ActionResult EditInfo()
        {
            DBUserModel userModel = DALFactory.User.Select(this.viewUserModel.UserCode);
            if (userModel == null)
            {
                return this.RedirectToLoginUrl();
            }
            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditInfoOperater(DBUserModel model)
        {
            return this.Operater(() =>
            {
                return FilterFactory.User.EditInfo(model);
            }, null, () =>
            {
                // 设置用户编号为当前用户名
                model.UserCode = this.viewUserModel.UserCode;
                return DALFactory.User.EditInfo(model);
            }, Url.Action("EditInfo"), Url.Action("EditInfo"), null, "数据保存成功！", "数据保存失败！");
        }

        [RoleActionFilter]
        public ActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditPasswordOperater(string oldPassword, string newPassword, string againPassword)
        {
            return this.Operater(() =>
            {
                return FilterFactory.User.EditPassword(oldPassword, newPassword, againPassword);
            }, null, () =>
            {
                oldPassword = EncryptHelper.MD5(oldPassword, ConfigHelper.ConfuseKey);
                newPassword = EncryptHelper.MD5(newPassword, ConfigHelper.ConfuseKey);

                return DALFactory.User.EditPassword(this.viewUserModel.UserCode, oldPassword, newPassword);
            }, Url.Action("Index", "Login"), Url.Action("EditPassword"), null, "数据保存成功，请重新登录！", "密码修改失败，请稍候再试！", false, false, true, false);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBUserModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBUserModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.User.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBUserModel model)
        {
            return this.OperaterConfirm(() =>
            {
                return FilterFactory.User.Operater(model);
            }, () =>
            {
                if (model.IdentityID == 0)
                {
                    model.UserPassword = EncryptHelper.MD5(model.UserPassword, ConfigHelper.ConfuseKey);
                }
                return DALFactory.User.Exists(model.UserCode, model.IdentityID);
            }, () =>
            {
                return DALFactory.User.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }), "用户名已存在！");
        }
        private List<DBKeyValueModel> InitRoleKeyValueList(List<DBRoleModel> modelList, List<DBChannelModel> channelModelList, string channelCode)
        {
            return ConstHelper.GetChannelKeyValueList<DBRoleModel>(channelModelList, modelList, channelCode, (DBRoleModel model) =>
            {
                return model.IdentityID;
            }, (DBRoleModel model) =>
            {
                return model.RoleName;
            });
        }
    }
}