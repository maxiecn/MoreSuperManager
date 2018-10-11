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
        public ActionResult List(string channelCode = "", string searchKey = "", int flowType = -1, int pageIndex = 1)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);

            List<DBFlowFullModel> modelList = DALFactory.Flow.Page(this.GetChannelCode(channelCode), searchKey, flowType, pageIndex, this.PageSize, ref this.totalCount, ref this.pageCount);
            List<DBChannelModel> channelModelList = this.IsSuperManager ? DALFactory.Channel.ChannelList() : null;

            this.InitViewData(searchKey, pageIndex, Url.Action("List", new { PageIndex = -999, ChannelCode = channelCode, SearchKey = searchKey, FlowType = flowType }), channelModelList, channelCode);
            
            ViewData["FlowType"] = flowType;
            if (this.IsSuperManager)
            {
                ViewBag.FlowTypeList = this.InitFlowTypeKeyValueList(DALFactory.FlowType.List(), channelModelList, this.viewUserModel.ChannelCode);
            }
            else
            {
                ViewBag.FlowTypeList = this.InitFlowTypeKeyValueList(DALFactory.FlowType.ChannelList(this.viewUserModel.ChannelCode), channelModelList, this.viewUserModel.ChannelCode);
            }
            return View(modelList);
        }

        [RoleActionFilter]
        public ActionResult Edit(int identityID = 0)
        {
            DBFlowModel model = DALFactory.Flow.Select(identityID);
            if(model == null)
            {
                return this.RedirectToUrl("流程数据不正确！", Url.Action("List"), false, false);
            }
            ViewBag.FlowTypeList = DALFactory.FlowType.ChannelList(model.ChannelCode);
            return View("Edit", model);
        }

        [RoleActionFilter]
        public ActionResult FlowAuth(int identityID = 0)
        {
            DBFlowModel model = DALFactory.Flow.Select(identityID);
            if (model == null)
            {
                return this.RedirectToUrl("流程数据不正确！", Url.Action("List"), false, false);
            }

            ViewBag.RoleList = DALFactory.Role.ChannelList(model.ChannelCode);
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
            DBFlowModel model = identityID > 0 ? DALFactory.Flow.Select(identityID) : null;

            string channelCode = null;
            List<DBChannelModel> channelModelList = null;

            this.InitChannelViewData<DBFlowModel>(model, (p, k) =>
            {
                channelCode = p;
                channelModelList = k;
            }, () =>
            {
                return DALFactory.Channel.ChannelList();
            });

            List<DBFlowTypeModel> flowTypeModelList = null;
            List<DBRoleModel> roleModelList = null;
            List<DBFlowSymbolTypeModel> flowSymbolTypeModelList = null;

            if (this.IsSuperManager)
            {
                flowTypeModelList = DALFactory.FlowType.List();
                roleModelList = DALFactory.Role.List();
                flowSymbolTypeModelList = DALFactory.FlowSymbolType.List();
            }
            else
            {
                flowTypeModelList = DALFactory.FlowType.ChannelList(channelCode);
                roleModelList = DALFactory.Role.ChannelList(channelCode);
                flowSymbolTypeModelList = DALFactory.FlowSymbolType.ChannelList(channelCode);
            }

            ViewBag.FlowTypeJsonText = this.GetFlowTypeJsonText(channelModelList, flowTypeModelList);
            ViewBag.RoleJsonText = this.GetRoleJsonText(channelModelList, roleModelList);
            ViewBag.FlowSymbolTypeJsonText = this.GetFlowSymbolTypeJsonText(channelModelList, flowSymbolTypeModelList);

            if (this.IsSuperManager)
            {
                ViewBag.RoleList = roleModelList.Where(p => p.ChannelCode == channelCode).ToList();
                ViewBag.SymbolTypeList = flowSymbolTypeModelList.Where(p => p.ChannelCode == channelCode).ToList();
                ViewBag.FlowTypeList = flowTypeModelList.Where(p => p.ChannelCode == channelCode).ToList();
            }
            else
            {
                ViewBag.RoleList = roleModelList;
                ViewBag.SymbolTypeList = flowSymbolTypeModelList;
                ViewBag.FlowTypeList = flowTypeModelList;
            }
            // 流程步骤数据
            ViewBag.FlowStepJsonText = this.GetFlowStepJsonText(identityID);

            return View("FlowDesignEdit", model);
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
        private string GetFlowStepJsonText(int identityID)
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
        private List<DBKeyValueModel> InitFlowTypeKeyValueList(List<DBFlowTypeModel> modelList, List<DBChannelModel> channelModelList, string channelCode)
        {
            return ConstHelper.GetChannelKeyValueList<DBFlowTypeModel>(channelModelList, modelList, channelCode, (DBFlowTypeModel model) =>
            {
                return model.IdentityID;
            }, (DBFlowTypeModel model) =>
            {
                return model.TypeName;
            });
        }
        
        [NonAction]
        private string GetFlowTypeJsonText(List<DBChannelModel> channelModelList, List<DBFlowTypeModel> modelList)
        {
            return ConstHelper.GetJsonText<DBFlowTypeModel>(channelModelList, modelList, (DBFlowTypeModel model) =>
            {
                return model.IdentityID;
            }, (DBFlowTypeModel model) =>
            {
                return model.TypeName;
            });
        }
        
        [NonAction]
        private string GetRoleJsonText(List<DBChannelModel> channelModelList, List<DBRoleModel> modelList)
        {
            return ConstHelper.GetJsonText<DBRoleModel>(channelModelList, modelList, (DBRoleModel model) =>
            {
                return model.IdentityID;
            }, (DBRoleModel model) =>
            {
                return model.RoleName;
            });
        }
        
        [NonAction]
        private string GetFlowSymbolTypeJsonText(List<DBChannelModel> channelModelList, List<DBFlowSymbolTypeModel> modelList)
        {
            return ConstHelper.GetJsonText<DBFlowSymbolTypeModel>(channelModelList, modelList, (DBFlowSymbolTypeModel model) =>
            {
                return model.TypeCode;
            }, (DBFlowSymbolTypeModel model) =>
            {
                return model.TypeCode;
            });
        }

        [NonAction]
        private ActionResult FlowDesignAddOrEditOperater(DBFlowModel model, string flowStepList)
        {
            this.SetChannelCode<DBFlowModel>(model);

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