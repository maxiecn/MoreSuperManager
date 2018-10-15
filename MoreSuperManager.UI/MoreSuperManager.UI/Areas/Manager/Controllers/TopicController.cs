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
    public class TopicController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string channelCode = "", string searchKey = "", int topicType = -1, int topicPositionType = -1, int topicStatus = -1, int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBTopicFullModel> modelList = DALFactory.Topic.Page(channelCode, searchKey, topicType, topicPositionType, topicStatus, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);
            List<DBChannelModel> channelModelList = this.IsSuperManager ? DALFactory.Channel.ChannelList() : null;

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, ChannelCode = channelCode, SearchKey = searchKey, TopicType = topicType, TopicPosition = topicPositionType, TopicStatus = topicStatus }), channelModelList, channelCode);

            if (this.IsSuperManager)
            {
                ViewBag.TopicTypeList = InitTopicTypeKeyValueList(TreeHelper.ToMenuList<ViewTreeTopicTypeModel>(DALFactory.TopicType.TreeList()), channelModelList, this.viewUserModel.ChannelCode);
            }
            else
            {
                ViewBag.TopicTypeList = InitTopicTypeKeyValueList(TreeHelper.ToMenuList<ViewTreeTopicTypeModel>(DALFactory.TopicType.ChannelList(this.viewUserModel.ChannelCode)), channelModelList, this.viewUserModel.ChannelCode);
            }
            ViewBag.PositionTypeList = DALFactory.TopicPositionType.List();
            ViewBag.StatusTypeList = new List<DBKeyValueModel>()
            {
                new DBKeyValueModel(){ Key = OperaterTypeEnum.DEFAULT, Value = "未审核" },
                new DBKeyValueModel(){ Key = OperaterTypeEnum.DELETE, Value = "已删除" },
                new DBKeyValueModel(){ Key = OperaterTypeEnum.CHECKED, Value = "已审核" },
            };
            ViewData["TopicType"] = topicType;
            ViewData["TopicPositionType"] = topicPositionType;
            ViewData["TopicStatus"] = topicStatus;
            return View(modelList);
        }

        [RoleActionFilter]
        public ActionResult Add()
        {
            return this.Edit();
        }

        [RoleActionFilter]
        public ActionResult Edit(int identityID = 0)
        {
            DBTopicModel model = identityID > 0 ? DALFactory.Topic.Select(identityID) : null;

            string channelCode = null;
            List<DBChannelModel> channelModelList = null;

            this.InitChannelViewData<DBTopicModel>(model, (p, k) =>
            {
                channelCode = p;
                channelModelList = k;
            }, () =>
            {
                return DALFactory.Channel.ChannelList();
            });

            List<DBTopicPositionTypeModel> topicPositionTypeModelList = null;
            List<ViewTreeTopicTypeModel> topicTypeModelList = null;

            if (this.IsSuperManager)
            {
                topicPositionTypeModelList = DALFactory.TopicPositionType.List();
                topicTypeModelList = TreeHelper.ToMenuList<ViewTreeTopicTypeModel>(DALFactory.TopicType.TreeList());
            }
            else
            {
                topicPositionTypeModelList = DALFactory.TopicPositionType.ChannelList(channelCode);
                topicTypeModelList = TreeHelper.ToMenuList<ViewTreeTopicTypeModel>(DALFactory.TopicType.ChannelList(channelCode));
            }

            ViewBag.TopicTypeJsonText = this.GetTopicTypeJsonText(channelModelList, topicTypeModelList);
            ViewBag.TopicPositionTypeJsonText = this.GetTopicPositionTypeJsonText(channelModelList, topicPositionTypeModelList);

            if (this.IsSuperManager)
            {
                ViewBag.TopicTypeList = topicTypeModelList.Where(p => p.ChannelCode == channelCode).ToList();
                ViewBag.PositionTypeList = topicPositionTypeModelList.Where(p => p.ChannelCode == channelCode).ToList();
            }
            else
            {
                ViewBag.TopicTypeList = topicTypeModelList;
                ViewBag.PositionTypeList = topicPositionTypeModelList;
            }
            return View("Edit", model);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.Topic.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        public ActionResult UploadOperater(string type, string fromType, string CKEditorFuncNum = null)
        {
            // 上传文件
            return this.UploadOperater(() =>
            {
                return DataHelper.AuthAction(this.viewUserModel.RoleID.ToString(), "Topic", "Upload");
            }, type, fromType, CKEditorFuncNum, "Topics", (string attachmentType, string attachmentName, int attachmentSize, string attachmentPath) =>
            {
                return DALFactory.Attachment.Operater(new MODEL.DBAttachmentModel()
                {
                    AttachmentType = attachmentType,
                    AttachmentName = attachmentName,
                    AttachmentSize = attachmentSize,
                    AttachmentPath = attachmentPath
                });
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBTopicModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBTopicModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.Topic.DeleteMore(identityIDList);
            }, (string identityIDList, string operaterType) =>
            {
                // 审核
                if (operaterType == OperaterTypeEnum.CHECKED)
                {
                    return DALFactory.Topic.StatusMore(identityIDList, int.Parse(operaterType));
                }
                return false;
            }, Url.Action("List"));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBTopicModel model)
        {
            model.PositionTypeList = StringHelper.PadChar(model.PositionTypeList, ",");
            return this.OperaterConfirm(() =>
            {
                return FilterFactory.Topic.Operater(model);
            }, null, () =>
            {
                return DALFactory.Topic.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }));
        }

        private List<DBKeyValueModel> InitTopicTypeKeyValueList(List<ViewTreeTopicTypeModel> modelList, List<DBChannelModel> channelModelList, string channelCode)
        {
            return ConstHelper.GetChannelKeyValueList<ViewTreeTopicTypeModel>(channelModelList, modelList, channelCode, (ViewTreeTopicTypeModel model) =>
            {
                return model.IdentityID;
            }, (ViewTreeTopicTypeModel model) =>
            {
                return model.LayerName;
            });
        }
        private string GetTopicTypeJsonText(List<DBChannelModel> channelModelList, List<ViewTreeTopicTypeModel> modelList)
        {
            return ConstHelper.GetJsonText<ViewTreeTopicTypeModel>(channelModelList, modelList, (ViewTreeTopicTypeModel model) =>
            {
                return model.IdentityID;
            }, (ViewTreeTopicTypeModel model) =>
            {
                return model.LayerName;
            });
        }
        private string GetTopicPositionTypeJsonText(List<DBChannelModel> channelModelList, List<DBTopicPositionTypeModel> modelList)
        {
            return ConstHelper.GetJsonText<DBTopicPositionTypeModel>(channelModelList, modelList, (DBTopicPositionTypeModel model) =>
            {
                return model.IdentityID;
            }, (DBTopicPositionTypeModel model) =>
            {
                return model.TypeName;
            });
        }
    }
}