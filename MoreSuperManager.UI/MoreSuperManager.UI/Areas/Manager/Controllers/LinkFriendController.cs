using Helper.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoreSuperManager.DAL;
using MoreSuperManager.FILTER;
using MoreSuperManager.MODEL;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class LinkFriendController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string channelCode = "", string searchKey = "", int linkFriendType = -1, int pageIndex = 1)
        {
            searchKey = this.FilterSpecChar(searchKey);

            List<DBLinkFriendFullModel> modelList = DALFactory.LinkFriend.Page(this.GetChannelCode(channelCode), searchKey, linkFriendType, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);
            List<DBChannelModel> channelModelList = this.IsSuperManager ? DALFactory.Channel.ChannelList() : null;

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, ChannelCode = channelCode, SearchKey = searchKey, LinkFriendType = linkFriendType }), channelModelList, channelCode);
            
            ViewData["LinkFriendType"] = linkFriendType;
            ViewBag.LinkFriendTypeList = this.InitLinkFriendTypeKeyValueList(DALFactory.LinkFriendType.List(), channelModelList, this.viewUserModel.ChannelCode);
            
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
            DBLinkFriendModel model = identityID > 0 ? DALFactory.LinkFriend.Select(identityID) : null;

            string channelCode = null;
            List<DBChannelModel> channelModelList = null;

            this.InitChannelViewData<DBLinkFriendModel>(model, (p, k) =>
            {
                channelCode = p;
                channelModelList = k;
            }, () =>
            {
                return DALFactory.Channel.ChannelList();
            });

            List<DBLinkFriendTypeModel> linkFriendTypeModelList = null;
            if (this.IsSuperManager)
            {
                linkFriendTypeModelList = DALFactory.LinkFriendType.List();
            }
            else
            {
                linkFriendTypeModelList = DALFactory.LinkFriendType.ChannelList(channelCode);
            }

            ViewBag.LinkFriendTypeJsonText = this.GetLinkFriendTypeJsonText(channelModelList, linkFriendTypeModelList);
            ViewBag.LinkFriendTypeList = linkFriendTypeModelList;

            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult UploadOperater(string type, string fromType, string CKEditorFuncNum = null)
        {
            // 上传文件
            return this.UploadOperater(() =>
            {
                return DataHelper.AuthAction(this.viewUserModel.RoleID.ToString(), "LinkFriend", "Upload");
            }, type, fromType, CKEditorFuncNum, "LinkFriends");
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.LinkFriend.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBLinkFriendModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBLinkFriendModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.LinkFriend.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBLinkFriendModel model)
        {
            this.SetChannelCode<DBLinkFriendModel>(model);

            return this.OperaterConfirm(() =>
            {
                return FilterFactory.LinkFriend.Operater(model);
            }, null, () =>
            {
                return DALFactory.LinkFriend.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }));
        }
        private List<DBKeyValueModel> InitLinkFriendTypeKeyValueList(List<DBLinkFriendTypeModel> modelList, List<DBChannelModel> channelModelList, string channelCode)
        {
            return ConstHelper.GetChannelKeyValueList<DBLinkFriendTypeModel>(channelModelList, modelList, channelCode, (DBLinkFriendTypeModel model) =>
            {
                return model.IdentityID;
            }, (DBLinkFriendTypeModel model) =>
            {
                return model.TypeName;
            });
        }
        private string GetLinkFriendTypeJsonText(List<DBChannelModel> channelModelList, List<DBLinkFriendTypeModel> modelList)
        {
            return ConstHelper.GetJsonText<DBLinkFriendTypeModel>(channelModelList, modelList, (DBLinkFriendTypeModel model) =>
            {
                return model.IdentityID;
            }, (DBLinkFriendTypeModel model) =>
            {
                return model.TypeName;
            });
        }
    }
}