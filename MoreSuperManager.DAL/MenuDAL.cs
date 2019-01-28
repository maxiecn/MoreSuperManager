using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class MenuDAL
    {
        private const string TABLE_NAME = "T_Menu";

        public bool Operater(DBMenuModel model)
        {
            model.ActionList = StringHelper.PadChar(model.ActionList, ",");

            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBMenuModel>(model, p => p.IdentityID, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBMenuModel>(model, p => p.IdentityID == p.IdentityID, p => p.IdentityID, TABLE_NAME);
            }
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBMenuModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBMenuModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public DBMenuModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBMenuModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.ParentID, p.MenuIcon, p.MenuName, p.MenuUrl, p.BelongModule, p.ActionList, p.MenuSort, p.ChannelCode }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public List<DBMenuModel> AuthList()
        {
            return DataBaseHelper.More<DBMenuModel>(null, p => new { p.MenuUrl, p.BelongModule, p.ActionList }, null, null, true, TABLE_NAME);
        }
        public List<DBMenuModel> List(string identityIDList)
        {
            if (string.IsNullOrEmpty(identityIDList)) identityIDList = "0";

            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");

            System.Linq.Expressions.Expression<Func<DBMenuModel, object>> queryLambda = p => new { p.IdentityID, p.ParentID, p.MenuName, p.MenuUrl, p.BelongModule, p.ActionList, p.MenuSort, p.MenuIcon };
            System.Linq.Expressions.Expression<Func<DBMenuModel, object>> orderLambda = p => p.MenuSort;

            return DataBaseHelper.More<DBMenuModel>(null, queryLambda, p => dataList.Contains(p.IdentityID), orderLambda, true, TABLE_NAME);
        }
        public List<ViewTreeMenuModel> TreeList()
        {
            string commandText = "select IdentityID, ParentID, MenuName, MenuUrl, MenuIcon, BelongModule, ActionList, MenuSort,ChannelCode from T_Menu with(nolock) order by MenuSort desc";
            return DataBaseHelper.ToEntityList<ViewTreeMenuModel>(commandText);
        }
        public List<ViewTreeMenuModel> ChannelList(string channelCode)
        {
            string commandText = "select IdentityID, ParentID, MenuName, MenuUrl, MenuIcon, BelongModule, ActionList, MenuSort from T_Menu with(nolock) where ChannelCode=@ChannelCode order by MenuSort desc";
            return DataBaseHelper.ToEntityList<ViewTreeMenuModel>(commandText, new { ChannelCode = channelCode });
        }
        public List<DBMenuModel> CloneList(string channelCode)
        {
            return DataBaseHelper.More<DBMenuModel>(new { ChannelCode = channelCode }, p => new { p.ChannelCode, p.ParentID, p.MenuName, p.MenuUrl, p.BelongModule, p.ActionList, p.MenuSort, p.MenuIcon }, p => p.ChannelCode == p.ChannelCode, null, true, TABLE_NAME);
        }
        public List<ViewTreeMenuModel> All(string channelCode, string searchKey)
        {
            channelCode = StringHelper.FilterSpecChar(channelCode);
            searchKey = StringHelper.FilterSpecChar(searchKey);

            if (string.IsNullOrEmpty(searchKey))
            {
                string commandText = "select IdentityID, ParentID, MenuName, MenuUrl, MenuIcon, BelongModule, ActionList, MenuSort from T_Menu with(nolock) where ChannelCode=@ChannelCode order by MenuSort desc";
                return DataBaseHelper.ToEntityList<ViewTreeMenuModel>(commandText, new { ChannelCode = channelCode });
            }
            else
            {
                string commandText = "select IdentityID, 0 as ParentID, MenuName, MenuUrl, MenuIcon, BelongModule, ActionList, MenuSort from T_Menu with(nolock) where ChannelCode=@ChannelCode and MenuName like '%{0}%' order by MenuSort desc";
                commandText = string.Format(commandText, searchKey);
                return DataBaseHelper.ToEntityList<ViewTreeMenuModel>(commandText, new { ChannelCode = channelCode });
            }
        }
    }
}
