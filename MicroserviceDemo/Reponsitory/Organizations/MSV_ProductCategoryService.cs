using System.Collections.Generic;
using System.Threading.Tasks;
using Core.SQL.SQLServer;
using Framework.Automap.SQLServer;
using Framework.Entities.Organization;

namespace Catalog.Reponsitory.Organizations
{
    public class MSV_ProductCategoryService : IMSV_ProductCategoryService
    {
        private readonly IConnectSQL _db;

        public MSV_ProductCategoryService(IConnectSQL db)
        {
            _db = db;
        }

        public async Task<List<ProductCategory>> GetListProductCategory()
        {
            SQLParameters sqlParams = new SQLParameters();
            var tbl = _db.ExecuteToTable("usp_ProductCategory_GetList", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            List<ProductCategory> listProductCtg = new List<ProductCategory>();
            listProductCtg = (tbl != null && tbl.Rows.Count > 0) ? AutoMapper<ProductCategory>.Map(tbl) : listProductCtg;

            return listProductCtg;
        }
    }
}
