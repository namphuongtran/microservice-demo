using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Core.SQL.SQLServer;
using Framework.Automap.SQLServer;
using Framework.Entities.Systems;

namespace IdentityServer.Library
{
    public class AccountService : IAccountService
    {
        private readonly IConnectSQL _db;
        public static IConfigurationRoot Configuration;

        public AccountService(IConnectSQL db)
        {
            _db = db;
        }

        public AccountService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            _db = new ConnectSQL(Configuration);
        }

        public Account GetInforAccount()
        {
            return new Account() {

            };
        }

        public bool IsValidAccount(string email, string passWord)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_Account", email);
            sqlParams.Add_Parameter("@_Password", passWord);

            var tbl = _db.ExecuteToTable("usp_Account_CheckLogin", sqlParams, ExecuteType.StoredProcedure);

            return (tbl != null && tbl.Rows.Count > 0);
        }

        public async Task<bool> IsValidUser(string email, string passWord)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_Account", email);
            sqlParams.Add_Parameter("@_Password", passWord);

            var tbl = _db.ExecuteToTable("usp_Account_CheckLogin", sqlParams, ExecuteType.StoredProcedure);

            await Task.FromResult(tbl);

            return (tbl != null && tbl.Rows.Count > 0);
        }

        public async Task<bool> CheckExistAccount(string email)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_Account", email);

            var tbl = _db.ExecuteToTable("usp_Account_CheckExistAccount", sqlParams, ExecuteType.StoredProcedure);

            await Task.FromResult(tbl);

            return (tbl != null && tbl.Rows.Count > 0);
        }

        public void CreateAccount(Account acc)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_AccountId", acc.AccountId);
            sqlParams.Add_Parameter("@_Email", acc.Email);
            sqlParams.Add_Parameter("@_Firstname", acc.Firstname);
            sqlParams.Add_Parameter("@_LastName", acc.LastName);
            sqlParams.Add_Parameter("@_Password", acc.Password);
            sqlParams.Add_Parameter("@_Phone", acc.Phone);
            sqlParams.Add_Parameter("@_Address", acc.Address);

            _db.ExecuteNonQuery("usp_Account_Create", sqlParams, ExecuteType.StoredProcedure);
        }

        public async Task<Account> GetInforAccount(string email)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_Email", email);

            var tbl = _db.ExecuteToTable("usp_Account_GetInfor", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            Account account = new Account();
            account = tbl != null ? AutoMapper<Account>.Map(tbl.Rows[0]) : account;

            return account;
        }

        public async Task<List<Account>> GetListInforAccount(List<int> listUser)
        {
            DataTable dtb = new DataTable();
            dtb.Columns.Add(new DataColumn("KeyId"));

            foreach(var item in listUser)
            {
                var dr = dtb.NewRow();
                dr["KeyId"] = item;
                dtb.Rows.Add(dr);
            }

            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_ListAccount", dtb);

            var tbl = _db.ExecuteToTable("usp_Account_GetListInfor", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            List<Account> listAccount = new List<Account>();
            listAccount = tbl != null ? AutoMapper<Account>.Map(tbl) : listAccount;

            return listAccount;
        }
    }
}
