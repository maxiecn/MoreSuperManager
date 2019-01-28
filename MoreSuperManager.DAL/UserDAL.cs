using Helper.Core.Library;
using MoreSuperManager.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreSuperManager.DAL
{
    public class UserDAL
    {
        private const string TABLE_NAME = "T_User";

        public bool Operater(DBUserModel model)
        {
            if (model.IdentityID == 0)
            {
                return DataBaseHelper.Insert<DBUserModel>(model, p => p.IdentityID, TABLE_NAME);
            }
            else
            {
                return DataBaseHelper.Update<DBUserModel>(model, p => p.IdentityID == p.IdentityID, p => new { p.IdentityID, p.UserPassword }, TABLE_NAME);
            }
        }
        public bool EditInfo(DBUserModel model)
        {
            return DataBaseHelper.Update<DBUserModel>(new { NickName = model.NickName, UserCode = model.UserCode }, p => p.UserCode == p.UserCode, null, TABLE_NAME);
        }
        public bool EditPassword(string userCode, string oldPassword, string newPassword)
        {
            string commandText = "update T_User set UserPassword=@NewPassword where UserCode=@UserCode and UserPassword=@OldPassword";
            return DataBaseHelper.ExecuteNonQuery(commandText, new { UserCode = userCode, NewPassword = newPassword, OldPassword = oldPassword }) > 0;
        }
        public bool Exists(string userCode, int identityID)
        {
            return DataBaseHelper.Exists<DBUserModel>(new { UserCode = userCode }, p => p.IdentityID, p => p.UserCode == p.UserCode, identityID, TABLE_NAME);
        }
        public bool Delete(int identityID)
        {
            return DataBaseHelper.Delete<DBUserModel>(new { IdentityID = identityID }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public bool DeleteMore(string identityIDList)
        {
            List<int> dataList = StringHelper.ToList<int>(identityIDList, ",");
            return DataBaseHelper.Delete<DBUserModel>(null, p => dataList.Contains(p.IdentityID), TABLE_NAME);
        }

        public DBUserFullModel Select(string userCode, string userPassword)
        {
            string commandText = "select T_User.IdentityID, UserCode, RoleID, NickName, T_Role.RoleName, T_Role.MenuList, T_Role.ActionList, T_Role.ChannelCode from T_User with(nolock) left join T_Role with(nolock) on T_Role.IdentityID=T_User.RoleID where UserCode=@UserCode and UserPassword=@UserPassword";
            return DataBaseHelper.ToEntity<DBUserFullModel>(commandText, new { UserCode = userCode, UserPassword = userPassword });
        }

        public DBUserModel Select(int identityID)
        {
            return DataBaseHelper.Single<DBUserModel>(new { IdentityID = identityID }, p => new { p.IdentityID, p.UserCode, p.NickName }, p => p.IdentityID == p.IdentityID, TABLE_NAME);
        }
        public DBUserModel Select(string userCode)
        {
            return DataBaseHelper.Single<DBUserModel>(new { UserCode = userCode }, p => new { p.IdentityID, p.UserCode, p.NickName }, p => p.UserCode == p.UserCode, TABLE_NAME);
        }

        public List<DBUserFullModel> Page(string searchKey, int roleID, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            searchKey = StringHelper.FilterSpecChar(searchKey);

            StringBuilder stringBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(searchKey))
            {
                stringBuilder.Append(string.Format(" (UserCode like '%{0}%' or NickName like '%{0}%') ", searchKey));
                stringBuilder.Append(" and ");
            }
            if(roleID > 0)
            {
                stringBuilder.Append(" RoleID = ");
                stringBuilder.Append(roleID);
                stringBuilder.Append(" and ");
            }

            string whereSql = stringBuilder.ToString().TrimEnd().TrimEnd(new char[] { 'a', 'n', 'd' });

            Dictionary<string, object> parameterList = new Dictionary<string, object>();
            parameterList.Add(DataBaseParameterEnum.FieldSql, "IdentityID, UserCode, NickName, RoleID, (select RoleName from T_Role with(nolock) where T_Role.IdentityID=T.RoleID) as RoleName");
            parameterList.Add(DataBaseParameterEnum.Field, "IdentityID, UserCode, NickName, RoleID");
            parameterList.Add(DataBaseParameterEnum.TableName, "T_User");
            parameterList.Add(DataBaseParameterEnum.PrimaryKey, "IdentityID");
            parameterList.Add(DataBaseParameterEnum.PageIndex, pageIndex);
            parameterList.Add(DataBaseParameterEnum.PageSize, pageSize);
            parameterList.Add(DataBaseParameterEnum.WhereSql, whereSql);
            parameterList.Add(DataBaseParameterEnum.OrderSql, "IdentityID asc");
            parameterList.Add(DataBaseParameterEnum.JoinSql, "");

            return DataBaseHelper.ToEntityList<DBUserFullModel>("", parameterList, ref pageCount, ref totalCount, null, "PageCount", "TotalCount");
        }
    }
}
