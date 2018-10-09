using Helper.Core.Library;
using Newtonsoft.Json;
using MoreSuperManager.DAL;
using MoreSuperManager.ENUM;
using MoreSuperManager.FILTER;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class FlowController : BaseManagerListController
    {
        [RoleMenuFilter]
        public ActionResult List(string searchKey = "", int flowType = -1, int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);
            List<DBFlowFullModel> modelList = DALFactory.Flow.Page(searchKey, flowType, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, SearchKey = searchKey, FlowType = flowType }), null, null);
            ViewData["FlowType"] = flowType;
            ViewBag.FlowTypeList = DALFactory.FlowType.List();
            return View(modelList);
        }

        [RoleActionFilter]
        public ActionResult Edit(int identityID = 0)
        {
            ViewBag.FlowTypeList = DALFactory.FlowType.List();
            return View("Edit", DALFactory.Flow.Select(identityID));
        }

        [RoleActionFilter]
        public ActionResult FlowAuth(int identityID = 0)
        {
            ViewBag.RoleList = DALFactory.Role.List();
            ViewBag.FlowID = identityID;
            return View(DALFactory.FlowStep.List(identityID));
        }

        [RoleActionFilter]
        public ActionResult FlowDesignAdd()
        {
            return FlowDesignEdit(0);
        }

        [RoleActionFilter]
        public ActionResult FlowDesignEdit(int identityID = 0)
        {
            // 角色列表
            ViewBag.RoleList = DALFactory.Role.List();
            // 可选符号列表
            ViewBag.SymbolTypeList = DALFactory.FlowSymbolType.List();
            // 流程类别列表
            ViewBag.FlowTypeList = (identityID == 0 ? DALFactory.FlowType.List() : null);
            // 流程步骤数据
            ViewBag.FlowStepData = this.GetFlowJsonText(identityID);
            return View("FlowDesignEdit", identityID > 0 ? DALFactory.Flow.Select(identityID) : null);
        }

        [RoleActionFilter]
        public ActionResult DeleteOperater(int identityID)
        {
            return this.Delete(() =>
            {
                return DALFactory.Flow.Delete(identityID);
            }, Url.Action("List"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult FlowDesignAddOperater(DBFlowModel model, string flowStepList)
        {
            return this.FlowDesignAddOrEditOperater(model, flowStepList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult FlowDesignEditOperater(DBFlowModel model, string flowStepList)
        {
            return this.FlowDesignAddOrEditOperater(model, flowStepList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleActionFilter]
        public ActionResult EditOperater(DBFlowModel model)
        {
            return this.Operater(() =>
            {
                return FilterFactory.Flow.Operater(model.IdentityID, model.FlowName, "");
            }, null, () =>
            {
                return DALFactory.Flow.Edit(model);
            }, Url.Action("List"), Url.Action("Edit", new { identityID = model.IdentityID }));
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult MoreOperater()
        {
            return this.More((string identityIDList) =>
            {
                return DALFactory.Flow.DeleteMore(identityIDList);
            }, null, Url.Action("List"));
        }

        [HttpPost]
        [RoleActionFilter]
        public ActionResult FlowAuthOperater(int flowID)
        {
            string roleList = Request.Form["roleList"].ToString();

            List<string> roleDataList = StringHelper.ToList<string>(roleList, ",");

            List<DBFlowStepModel> roleModelList = new List<DBFlowStepModel>();

            #region 处理可操作权限
            if (roleDataList != null && roleDataList.Count > 0)
            {
                Dictionary<string, List<string>> roleDataDict = new Dictionary<string, List<string>>();
                foreach(string roleData in roleDataList)
                {
                    StringKeyValueData<string, string> keyValueData = StringHelper.ToKeyValueData<string, string>(roleData, "_");
                    if (keyValueData != null)
                    {
                        if (!roleDataDict.ContainsKey(keyValueData.Key))
                        {
                            roleDataDict.Add(keyValueData.Key, new List<string>());
                        }
                        roleDataDict[keyValueData.Key].Add(keyValueData.Value);
                    }
                }
                if (roleDataDict != null && roleDataDict.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> keyValueItem in roleDataDict)
                    {
                        roleModelList.Add(new DBFlowStepModel() { IdentityID = int.Parse(keyValueItem.Key), RoleList = StringHelper.ToString<string>(keyValueItem.Value, ",") });
                    }
                }
            }
            #endregion

            return this.Operater(null, null, () =>
            {
                return DALFactory.FlowStep.Operater(flowID, roleModelList);
            }, Url.Action("List"), Url.Action("FlowAuth", new { identityID = flowID }));
        }

        [NonAction]
        public string GetFlowJsonText(int identityID)
        {
            string format = "{{ \\\"total\\\": {0}, \\\"list\\\": [{1}] }}";
            if (identityID == 0) return string.Format(format, 0, "");

            List<DBFlowStepModel> modelList = DALFactory.FlowStep.List(identityID);
            if (modelList == null || modelList.Count == 0) return string.Format(format, 0, "");

            StringBuilder stringBuilder = new StringBuilder();

            int index = 0;
            foreach (DBFlowStepModel model in modelList)
            {
                stringBuilder.Append(string.Format("{{\\\"id\\\":\\\"{0}\\\",\\\"step_name\\\":\\\"{1}\\\", \\\"step_addr_name\\\":\\\"{2}\\\", \\\"flow_id\\\":{3}, \\\"role_list\\\":\\\"{4}\\\", \\\"process_to\\\":\\\"{5}\\\",\\\"icon\\\":\\\"{6}\\\",\\\"style\\\":\\\"left:{7}px;top:{8}px;\\\", \\\"next_step\\\":\\\"{9}\\\",\\\"process_name\\\":\\\"", model.StepCode, model.StepName, model.StepAddrName, model.FlowID, model.RoleList, model.StepList, model.StepSymbol, model.PositionLeft, model.PositionTop, model.NextStep));
                if (!string.IsNullOrEmpty(model.StepCode))
                {
                    stringBuilder.Append("[");
                    stringBuilder.Append(model.StepCode);
                    stringBuilder.Append("]");
                    stringBuilder.Append(model.StepName);
                }
                if (!string.IsNullOrEmpty(model.StepAddrName))
                {
                    stringBuilder.Append("(");
                    stringBuilder.Append(model.StepAddrName);
                    stringBuilder.Append(")");
                }
                stringBuilder.Append("\\\"}");
                if (index < modelList.Count - 1) stringBuilder.Append(",");
                index++;
            }

            return string.Format(format, modelList.Count, stringBuilder.ToString());
        }

        [NonAction]
        private ActionResult FlowDesignAddOrEditOperater(DBFlowModel model, string flowStepList)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            StringReader stringReader = new StringReader(flowStepList);
            List<DBFlowStepModel> dataList = jsonSerializer.Deserialize(new JsonTextReader(stringReader), typeof(List<DBFlowStepModel>)) as List<DBFlowStepModel>;

            return this.Operater(() =>
            {
                if (dataList == null || dataList.Count == 0) return "流程步骤数据错误！";
                return FilterFactory.Flow.Operater(model.IdentityID, model.FlowName, flowStepList);
            }, null, () =>
            {
                return DALFactory.Flow.Operater(model, dataList);
            }, Url.Action("List"), Url.Action("FlowDesign", new { identityID = model.IdentityID }));
        }
    }
}