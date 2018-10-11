using Helper.Core.Library;
using MoreSuperManager.DAL;
using MoreSuperManager.ENUM;
using MoreSuperManager.FILTER;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class ProjectController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string channelCode = "", string searchKey = "", int projectType = -1, int flowID = -1, int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBProjectFullModel> modelList = DALFactory.Project.Page(this.GetChannelCode(channelCode), searchKey, projectType, flowID, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);
            List<DBChannelModel> channelModelList = this.IsSuperManager ? DALFactory.Channel.ChannelList() : null;

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, ChannelCode = channelCode, SearchKey = searchKey, ProjectType = projectType, FlowID = flowID }), channelModelList, channelCode);
            ViewData["ProjectType"] = projectType;
            ViewData["FlowID"] = flowID;
            ViewBag.ProjectTypeList = this.InitProjectTypeKeyValueList(DALFactory.ProjectType.List(), channelModelList, this.viewUserModel.ChannelCode);
            ViewBag.FlowList = this.InitFlowKeyValueList(DALFactory.Flow.List(), channelModelList, this.viewUserModel.ChannelCode);
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
            DBProjectModel model = identityID > 0 ? DALFactory.Project.Select(identityID) : null;

            string channelCode = null;
            List<DBChannelModel> channelModelList = null;

            this.InitChannelViewData<DBProjectModel>(model, (p, k) =>
            {
                channelCode = p;
                channelModelList = k;
            }, () =>
            {
                return DALFactory.Channel.ChannelList();
            });

            List<DBProjectTypeModel> projectTypeModelList = null;
            List<DBFlowModel> flowModelList = null;

            if(this.IsSuperManager)
            {
                projectTypeModelList = DALFactory.ProjectType.List();
                flowModelList = DALFactory.Flow.List();
            }
            else
            {
                projectTypeModelList = DALFactory.ProjectType.ChannelList(channelCode);
                flowModelList = DALFactory.Flow.ChannelList(channelCode);
            }

            ViewBag.ProjectTypeJsonText = this.GetProjectTypeJsonText(channelModelList, projectTypeModelList);
            ViewBag.FlowJsonText = this.GetFlowJsonText(channelModelList, flowModelList);

            ViewBag.ProjectTypeList = projectTypeModelList;
            ViewBag.FlowList = flowModelList;

            return View("Edit", model);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.Project.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult AddOperater(DBProjectModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBProjectModel model)
        {
            return this.AddOrEditOperater(model);
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.Project.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [NonAction]
        private ActionResult AddOrEditOperater(DBProjectModel model)
        {
            return this.OperaterConfirm(() =>
            {
                return FilterFactory.Project.Operater(model);
            }, null, () =>
            {
                return DALFactory.Project.Operater(model);
            }, Url.Action("List"), model.IdentityID == 0 ? Url.Action("Add") : "", Url.Action("Edit", new { identityID = model.IdentityID }));
        }

        private List<DBKeyValueModel> InitProjectTypeKeyValueList(List<DBProjectTypeModel> modelList, List<DBChannelModel> channelModelList, string channelCode)
        {
            return ConstHelper.GetChannelKeyValueList<DBProjectTypeModel>(channelModelList, modelList, channelCode, (DBProjectTypeModel model) =>
            {
                return model.IdentityID;
            }, (DBProjectTypeModel model) =>
            {
                return model.TypeName;
            });
        }
        private List<DBKeyValueModel> InitFlowKeyValueList(List<DBFlowModel> modelList, List<DBChannelModel> channelModelList, string channelCode)
        {
            return ConstHelper.GetChannelKeyValueList<DBFlowModel>(channelModelList, modelList, channelCode, (DBFlowModel model) =>
            {
                return model.IdentityID;
            }, (DBFlowModel model) =>
            {
                return model.FlowName;
            });
        }

        private string GetProjectTypeJsonText(List<DBChannelModel> channelModelList, List<DBProjectTypeModel> modelList)
        {
            return ConstHelper.GetJsonText<DBProjectTypeModel>(channelModelList, modelList, (DBProjectTypeModel model) =>
            {
                return model.IdentityID;
            }, (DBProjectTypeModel model) =>
            {
                return model.TypeName;
            });
        }
        private string GetFlowJsonText(List<DBChannelModel> channelModelList, List<DBFlowModel> modelList)
        {
            return ConstHelper.GetJsonText<DBFlowModel>(channelModelList, modelList, (DBFlowModel model) =>
            {
                return model.IdentityID;
            }, (DBFlowModel model) =>
            {
                return model.FlowName;
            });
        }
    }
}