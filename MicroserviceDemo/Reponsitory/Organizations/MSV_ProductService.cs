using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Core.SQL.SQLServer;
using Framework.Automap.SQLServer;
using Framework.Common.DataPaging;
using Framework.Entities.Organization;
using Framework.Entities.Request;

namespace Catalog.Reponsitory.Organizations
{
    public class MSV_ProductService : IMSV_ProductService
    {
        private readonly IConnectSQL _db;

        public MSV_ProductService(IConnectSQL db)
        {
            _db = db;
        }

        public void AddProduct(Product product)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_ProductId", product.ProductId);
            sqlParams.Add_Parameter("@_ProductCategoryId", product.ProductCategoryId);
            sqlParams.Add_Parameter("@_OrganizationId", product.OrganizationId);
            sqlParams.Add_Parameter("@_Code", product.Code);
            sqlParams.Add_Parameter("@_Name", product.Name);
            sqlParams.Add_Parameter("@_UrlPicture", product.UrlPicture);
            sqlParams.Add_Parameter("@_Description", product.Description);

            _db.ExecuteNonQuery("usp_Product_InsertOrUpdate", sqlParams, ExecuteType.StoredProcedure);
        }

        public void DeleteProduct(int productId)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_ProductId", productId);

            _db.ExecuteNonQuery("usp_Product_Delete", sqlParams, ExecuteType.StoredProcedure);
        }

        public async Task<Product> GetInforProduct(int productId)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_ProductId", productId);
            var tbl = _db.ExecuteToTable("usp_Product_GetInfor", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            Product product = new Product();
            product = (tbl != null && tbl.Rows.Count > 0) ? AutoMapper<Product>.Map(tbl.Rows[0]) : product;

            return product;
        }

        public async Task<List<Product>> GetListProduct(int productCategoryId, int organizationId)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_ProductCategoryId", productCategoryId);
            sqlParams.Add_Parameter("@_OrganizationId", organizationId);
            var tbl = _db.ExecuteToTable("usp_Product_GetList", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            List<Product> listProduct = new List<Product>();
            listProduct = (tbl != null && tbl.Rows.Count > 0) ? AutoMapper<Product>.Map(tbl) : listProduct;

            return listProduct;
        }

        public async Task<GridModel<Product>> GetListProductPG(RequestParams pr)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_ProductCategoryId", pr.ProductCategoryId);
            sqlParams.Add_Parameter("@_OrganizationId", pr.OrganizationId);
            sqlParams.Add_Parameter("@_Conditional", pr.Conditional);
            sqlParams.Add_Parameter("@_PageIndex", pr.PageIndex);
            sqlParams.Add_Parameter("@_PageSize", pr.PageSize);
            var tbl = _db.ExecuteToDataset("usp_Product_GetListByConditional", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            GridModel<Product> listProduct = new GridModel<Product>();
            listProduct.Data = (tbl.Tables[0] != null && tbl.Tables[0].Rows.Count > 0) ? AutoMapper<Product>.Map(tbl.Tables[0]) : new List<Product>();
            listProduct.TotalPage = (tbl.Tables[1] != null && tbl.Tables[1].Rows.Count > 0) ? (int)tbl.Tables[1].Rows[0][0] : 0;
            listProduct.CurrentPage = pr.PageIndex;
            listProduct.SizePage = pr.PageSize;

            return listProduct;
        }

        public async Task<GridModel<Product>> Shop_GetListProductPG(RequestParams pr)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_ProductCategoryId", pr.ProductCategoryId);
            sqlParams.Add_Parameter("@_Conditional", pr.Conditional);
            sqlParams.Add_Parameter("@_PageIndex", pr.PageIndex);
            sqlParams.Add_Parameter("@_PageSize", pr.PageSize);
            var tbl = _db.ExecuteToDataset("usp_Product_GetListByConditional", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            GridModel<Product> listProduct = new GridModel<Product>();
            listProduct.Data = (tbl.Tables[0] != null && tbl.Tables[0].Rows.Count > 0) ? AutoMapper<Product>.Map(tbl.Tables[0]) : new List<Product>();
            listProduct.TotalPage = (tbl.Tables[1] != null && tbl.Tables[1].Rows.Count > 0) ? (int)tbl.Tables[1].Rows[0][0] : 0;
            listProduct.CurrentPage = pr.PageIndex;
            listProduct.SizePage = pr.PageSize;

            return listProduct;
        }

        public async Task<List<Product>> Shop_GetListProduct(List<Product> listProduct)
        {
            DataTable dtb = new DataTable();
            dtb.Columns.Add("ProductId", typeof(int));

            foreach(var item in listProduct)
            {
                var dr = dtb.NewRow();
                dr[0] = item.ProductId;
                dtb.Rows.Add(dr);
            }

            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_ListProduct", dtb);
            var tbl = _db.ExecuteToTable("usp_Shop_GetListProduct", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            List<Product> listProductResult = new List<Product>();
            listProductResult = (tbl != null && tbl.Rows.Count > 0) ? AutoMapper<Product>.Map(tbl) : listProductResult;

            return listProductResult;
        }
    }
}
