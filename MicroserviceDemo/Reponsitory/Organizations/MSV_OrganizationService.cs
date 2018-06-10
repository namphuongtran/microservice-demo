using System.Threading.Tasks;
using Core.SQL.SQLServer;
using Framework.Automap.SQLServer;
using Framework.Entities.Organization;

namespace Catalog.Reponsitory.Organizations
{
    public class MSV_OrganizationService : IMSV_OrganizationService
    {
        private readonly IConnectSQL _db;

        public MSV_OrganizationService(IConnectSQL db)
        {
            _db = db;
        }

        public void AddOrganization(Organization ogt)
        {
            SQLParameters p = new SQLParameters();
            p.Add_Parameter("@_OrganizationId", ogt.OrganizationId);
            p.Add_Parameter("@_AccountId", ogt.AccountId);
            p.Add_Parameter("@_Name", ogt.Name);
            p.Add_Parameter("@_Phone", ogt.Phone);
            p.Add_Parameter("@_UrlLogo", ogt.UrlLogo);
            p.Add_Parameter("@_Address", ogt.Address);

            _db.ExecuteNonQuery("usp_Organization_InsertOrUpdate", p, ExecuteType.StoredProcedure);
        }

        public async Task<Organization> GetInforOrganization(int organizationId)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_OrganizationId", organizationId);

            var tbl = _db.ExecuteToTable("usp_Organization_GetInfor", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            Organization ogt = new Organization();
            ogt = (tbl != null && tbl.Rows.Count > 0) ? AutoMapper<Organization>.Map(tbl.Rows[0]) : ogt;

            return ogt;
        }

        public async Task<Organization> GetInforOrganizationFromAcc(int accountId)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_AccountId", accountId);

            var tbl = _db.ExecuteToTable("usp_Organization_GetInforFromAcc", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            Organization ogt = new Organization();
            ogt = (tbl != null && tbl.Rows.Count > 0) ? AutoMapper<Organization>.Map(tbl.Rows[0]) : ogt;

            return ogt;
        }
    }
}
