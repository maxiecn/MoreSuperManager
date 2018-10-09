using Newtonsoft.Json;
using MoreSuperManager.DAL;
using MoreSuperManager.FILTER;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class ApplicationController : BaseManagerListController
    {
        [RoleActionFilter]
        public ActionResult App()
        {
            ViewBag.ApplicationList = DALFactory.Application.List();
            return View();
        }

        [RoleActionFilter]
        public ActionResult AppOperater(string gridItemList)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            List<DBApplicationModel> modelList = jsonSerializer.Deserialize(new JsonTextReader(new StringReader(gridItemList)), typeof(List<DBApplicationModel>)) as List<DBApplicationModel>;

            modelList = modelList.OrderBy(p => p.ApplicationY).ThenBy(p => p.ApplicationX).ToList();

            return this.Operater(() =>
            {
                if (modelList == null || modelList.Count == 0) return "应用数据错误！";
                return FilterFactory.Application.Operater(gridItemList);
            }, null, () =>
            {
                return DALFactory.Application.Operater(modelList);
            }, Url.Action("App"), Url.Action("App"));
        }
    }
}