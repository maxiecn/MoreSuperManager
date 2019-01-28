using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class VoteDAL
    {
        private const string TABLE_NAME = "T_Vote";

        public bool Operater(DBVoteModel model, List<DBVoteItemModel> modelList)
        {
            List<DataBaseTransactionItem> transactionItemList = new List<DataBaseTransactionItem>();
            if (model.IdentityID == 0)
            {
                transactionItemList.Add(new DataBaseTransactionItem()
                {
                    CommandText = "insert into T_Vote(VoteType, VoteTitle, VoteSummary, ChannelCode)values(@VoteType, @VoteTitle, @VoteSummary, @ChannelCode);select SCOPE_IDENTITY();",
                    ExecuteType = DataBaseExecuteTypeEnum.ExecuteScalar,
                    ParameterList = new { VoteType = model.VoteType, VoteTitle = model.VoteTitle, VoteSummary = model.VoteSummary, ChannelCode = model.ChannelCode },
                    OutputName = "VoteID"
                });
            }
            else
            {
                transactionItemList.Add(new DataBaseTransactionItem()
                {
                    CommandText = "delete from T_VoteItem where VoteID = @VoteID",
                    ExecuteType = DataBaseExecuteTypeEnum.ExecuteNonQuery,
                    ParameterList = new { VoteID = model.IdentityID }
                });
                transactionItemList.Add(new DataBaseTransactionItem()
                {
                    CommandText = "update T_Vote set VoteType=@VoteType,  VoteTitle=@VoteTitle, VoteSummary=@VoteSummary, ChannelCode=@ChannelCode where IdentityID=@IdentityID",
                    ExecuteType = DataBaseExecuteTypeEnum.ExecuteNonQuery,
                    ParameterList = new { VoteType = model.VoteType, VoteTitle = model.VoteTitle, VoteSummary = model.VoteSummary, ChannelCode = model.ChannelCode, IdentityID = model.IdentityID }
                });
            }
            if (modelList != null && modelList.Count > 0)
            {
                foreach (DBVoteItemModel itemModel in modelList)
                {
                    transactionItemList.Add(new DataBaseTransactionItem()
                    {
                        CommandText = "insert into T_VoteItem(ItemID, VoteID, ItemTitle, ItemType, ItemContent, ItemMaxCount)values(@ItemID, @VoteID, @ItemTitle, @ItemType, @ItemContent, @ItemMaxCount)",
                        ExecuteType = DataBaseExecuteTypeEnum.ExecuteNonQuery,
                        ParameterList = new { VoteID = model.IdentityID, ItemID = itemModel.ItemID, ItemTitle = itemModel.ItemTitle, ItemType = itemModel.ItemType, ItemContent = itemModel.ItemContent, ItemMaxCount = itemModel.ItemMaxCount },
                        InputList = model.IdentityID == 0 ? new string[] { "VoteID" } : null
                    });
                }
            }
            return DataBaseHelper.TransactionNonQuery(transactionItemList) > 0;
        }
        public bool Delete(int identityID)
        {
            return (bool)DataBaseHelper.Transaction((con, transaction) =>
            {
                DataBaseHelper.TransactionDelete<DBVoteItemModel>(con, transaction, new { VoteID = identityID }, p => p.VoteID == p.VoteID, "T_VoteItem");
                return DataBaseHelper.TransactionDelete<DBVoteModel>(con, transaction, new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
            });
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return (bool)DataBaseHelper.Transaction((con, transaction) =>
            {
                DataBaseHelper.TransactionDelete<DBVoteItemModel>(con, transaction, null, p => dataList.Contains(p.VoteID), "T_VoteItem");
                return DataBaseHelper.TransactionDelete<DBVoteModel>(con, transaction, null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
            });
        }
        public DBVoteModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBVoteModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.VoteType, p.VoteTitle, p.VoteSummary, p.ChannelCode }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }

        public List<DBVoteFullModel> Page(string channelCode, string searchKey, int voteType, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            channelCode = StringHelper.FilterSpecChar(channelCode);
            searchKey = StringHelper.FilterSpecChar(searchKey);

            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(channelCode) && channelCode != "-1")
            {
                stringBuilder.Append(" ChannelCode = '");
                stringBuilder.Append(channelCode);
                stringBuilder.Append("' and ");
            }
            if (!string.IsNullOrEmpty(searchKey))
            {
                stringBuilder.Append(string.Format(" VoteTitle like '%{0}%' ", searchKey));
                stringBuilder.Append(" and ");
            }
            if (voteType > 0)
            {
                stringBuilder.Append(" VoteType = ");
                stringBuilder.Append(voteType);
                stringBuilder.Append(" and ");
            }

            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });

            Dictionary<string, object> parameterList = new Dictionary<string, object>();
            parameterList.Add(DataBaseParameterEnum.FieldSql, "IdentityID, VoteTitle, VoteType, ChannelCode, (select TypeName from T_VoteType with(nolock) where T_VoteType.IdentityID=T.VoteType) as VoteTypeName, (select ChannelName from T_Channel with(nolock) where T_Channel.ChannelCode=T.ChannelCode) as ChannelName");
            parameterList.Add(DataBaseParameterEnum.Field, "IdentityID, VoteTitle, VoteType, ChannelCode");
            parameterList.Add(DataBaseParameterEnum.TableName, "T_Vote");
            parameterList.Add(DataBaseParameterEnum.PrimaryKey, "IdentityID");
            parameterList.Add(DataBaseParameterEnum.PageIndex, pageIndex);
            parameterList.Add(DataBaseParameterEnum.PageSize, pageSize);
            parameterList.Add(DataBaseParameterEnum.WhereSql, whereSql);
            parameterList.Add(DataBaseParameterEnum.OrderSql, "IdentityID asc");
            parameterList.Add(DataBaseParameterEnum.JoinSql, "");

            return DataBaseHelper.ToEntityList<DBVoteFullModel>("", parameterList, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
