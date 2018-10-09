using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class TopicTypeDAL
    {
        private const string TABLE_NAME = "T_TopicType";

        public bool Operater(DBTopicTypeModel model)
        {
            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBTopicTypeModel>(model, p => p.IdentityID, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBTopicTypeModel>(model, p => p.IdentityID == p.IdentityID, p => p.IdentityID, TABLE_NAME);
            }
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBTopicTypeModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBTopicTypeModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }
        public DBTopicTypeModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBTopicTypeModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.ParentID, p.TypeName, p.TypeSort, p.ChannelCode }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public List<DBTopicTypeModel> List(int parentID)
        {
            System.Linq.Expressions.Expression<Func<DBTopicTypeModel, object>> queryLambda = p => new { p.IdentityID, p.ParentID, p.TypeName, p.ChannelCode };
            if (parentID == -1)
            {
                return DataBaseHelper.More<DBTopicTypeModel>(null, queryLambda, null, p => p.TypeSort, true, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.More<DBTopicTypeModel>(new { ParentID = parentID }, queryLambda, p => p.ParentID == p.ParentID, p => p.TypeSort, true, TABLE_NAME);
            }
        }
        public List<ViewTreeTopicTypeModel> ChannelList(string channelCode)
        {
            string commandText = "select IdentityID, ParentID, TypeName, TypeSort, ChannelCode from T_TopicType with(nolock) where ChannelCode=@ChannelCode order by TypeSort desc";
            return DataBaseHelper.ToEntityList<ViewTreeTopicTypeModel>(commandText, new { ChannelCode = channelCode });
        }
        public List<ViewTreeTopicTypeModel> TreeList()
        {
            string commandText = "select IdentityID, ParentID, TypeName, TypeSort, ChannelCode from T_TopicType with(nolock) order by TypeSort desc";
            return DataBaseHelper.ToEntityList<ViewTreeTopicTypeModel>(commandText);
        }

        public List<ViewTreeTopicTypeModel> All(string channelCode, string searchKey)
        {
            if (string.IsNullOrEmpty(searchKey))
            {
                string commandText = "select IdentityID, ParentID, TypeName, TypeSort from T_TopicType with(nolock) where ChannelCode=@ChannelCode order by TypeSort desc";
                return DataBaseHelper.ToEntityList<ViewTreeTopicTypeModel>(commandText, new { ChannelCode = channelCode });
            }
            else
            {
                string commandText = "select IdentityID, 0 as ParentID, TypeName, TypeSort from T_TopicType with(nolock) where ChannelCode=@ChannelCode and TypeName like '%{0}%' order by TypeSort desc";
                commandText = string.Format(commandText, searchKey);
                return DataBaseHelper.ToEntityList<ViewTreeTopicTypeModel>(commandText, new { ChannelCode = channelCode });
            }
        }
    }
}
