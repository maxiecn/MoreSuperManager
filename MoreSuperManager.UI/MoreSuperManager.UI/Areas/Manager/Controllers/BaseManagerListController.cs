using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoreSuperManager.MODEL;

namespace MoreSuperManager.UI.Areas.Manager.Controllers
{
    public class BaseManagerListController : BaseManagerController
    {
        #region 列表变量
        protected int totalCount = 0;
        protected int pageCount = 0;
        #endregion

        protected virtual int PageSize
        {
            get
            {
                return SettingHelper.ManagerPageSize;
            }
        }

        protected virtual string GetChannelCode(string channelCode)
        {
            if (string.IsNullOrEmpty(channelCode)) return this.viewUserModel.ChannelCode;
            return channelCode;
        }

        protected virtual void SetChannelCode<T>(T t) where T : IChannelModel
        {
            if (!string.IsNullOrEmpty(this.viewUserModel.ChannelCode) && this.viewUserModel.ChannelCode != "-1")
            {
                t.ChannelCode = this.viewUserModel.ChannelCode;
            }
        }

        protected virtual bool IsSuperManager
        {
            get
            {
                if (string.IsNullOrEmpty(this.viewUserModel.ChannelCode) || this.viewUserModel.ChannelCode == "-1") return true;
                return false;
            }
        }

        protected virtual void InitChannelViewData<T>(T t, Action<string, List<DBChannelModel>> callback, Func<List<DBChannelModel>> func) where T : IChannelModel
        {
            string channelCode = this.viewUserModel.ChannelCode;

            List<DBChannelModel> channelModelList = null;
            if (this.IsSuperManager)
            {
                channelModelList = func != null ? func() : null;
                if (channelModelList != null && channelModelList.Count > 0) channelCode = channelModelList[0].ChannelCode;
            }
            if (t != null) channelCode = t.ChannelCode;

            if (channelModelList != null)
            {
                ViewBag.ChannelList = channelModelList;
            }
            if (callback != null) callback(channelCode, channelModelList);
        }

        protected virtual void InitViewData(string searchKey, int pageIndex, string pageUrl, List<DBChannelModel> channelModelList, string channelCode)
        {
            ViewData.Add("SearchKey", searchKey);
            ViewData.Add("PageModel", new ViewPageModel()
            {
                PageIndex = pageIndex,
                PageCount = this.pageCount,
                TotalCount = this.totalCount,
                PageUrl = pageUrl
            });
            if (this.IsSuperManager && channelModelList != null && channelModelList.Count > 0)
            {
                ViewBag.ChannelList = channelModelList;
            }
            ViewData.Add("ChannelCode", channelCode);
        }
        /// <summary>
        /// 过滤关键字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected virtual string FilterSpecChar(string str)
        {
            return Helper.Core.Library.StringHelper.FilterSpecChar(str);
        }
    }
}