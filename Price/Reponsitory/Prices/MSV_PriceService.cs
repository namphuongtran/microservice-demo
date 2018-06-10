using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Core.SQL.SQLServer;
using Framework.Automap.SQLServer;
using Framework.Common.DataPaging;
using Framework.Entities.Organization;
using Framework.Entities.Price;
using Framework.Entities.Request;
using Price.Reponsitory.Prices;

namespace Price.Library.Prices
{
    public class MSV_PriceService : IMSV_PriceService
    {
        private readonly IConnectSQL _db;

        public MSV_PriceService(IConnectSQL db)
        {
            _db = db;
        }

        public void AddPrice(ProductPrice price)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_PriceId", price.PriceId);
            sqlParams.Add_Parameter("@_ProductCategoryId", price.ProductCategoryId);
            sqlParams.Add_Parameter("@_ProductId", price.ProductId);
            sqlParams.Add_Parameter("@_Date", price.Date);
            sqlParams.Add_Parameter("@_Amount", price.Amount);

            _db.ExecuteNonQuery("usp_Price_InsertOrUpdate", sqlParams, ExecuteType.StoredProcedure);
        }

        public async Task<GridModel<ProductPrice>> GetListPricePG(RequestParams pr)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_ProductCategoryId", pr.ProductCategoryId);
            sqlParams.Add_Parameter("@_ProductId", pr.ProductId);
            sqlParams.Add_Parameter("@_Conditional", pr.Conditional);
            sqlParams.Add_Parameter("@_PageIndex", pr.PageIndex);
            sqlParams.Add_Parameter("@_PageSize", pr.PageSize);
            var tbl = _db.ExecuteToDataset("usp_Price_GetListByConditional", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            GridModel<ProductPrice> listProduct = new GridModel<ProductPrice>();
            listProduct.Data = (tbl.Tables[0] != null && tbl.Tables[0].Rows.Count > 0) ? AutoMapper<ProductPrice>.Map(tbl.Tables[0]) : new List<ProductPrice>();
            listProduct.TotalPage = (tbl.Tables[1] != null && tbl.Tables[1].Rows.Count > 0) ? (int)tbl.Tables[1].Rows[0][0] : 0;
            listProduct.CurrentPage = pr.PageIndex;
            listProduct.SizePage = pr.PageSize;

            return listProduct;
        }

        public async Task<List<Product>> GetPriceFromProduct(List<Product> listProduct)
        {
            DataTable dtb = new DataTable();
            dtb.Columns.Add(new DataColumn("KeyId", typeof(int)));

            foreach (var item in listProduct)
            {
                var dr = dtb.NewRow();
                dr[0] = item.ProductId;
                dtb.Rows.Add(dr);
            }

            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_ListProduct", dtb);
            var tbl = _db.ExecuteToTable("usp_Price_GetListPriceFromProduct", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            listProduct = new List<Product>();
            if (tbl != null && tbl.Rows.Count > 0)
            {
                listProduct = AutoMapper<Product>.Map(tbl);
            }

            return listProduct;
        }

        public async Task<GridModel<Product>> Shop_GetListProductPrice(RequestParams pr)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_ProductCategoryId", pr.ProductCategoryId);
            sqlParams.Add_Parameter("@_Conditional", pr.Conditional);
            sqlParams.Add_Parameter("@_PageIndex", pr.PageIndex);
            sqlParams.Add_Parameter("@_PageSize", pr.PageSize);
            var tbl = _db.ExecuteToDataset("usp_Shop_GetListByConditional", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            GridModel<Product> listProduct = new GridModel<Product>();
            listProduct.Data = (tbl.Tables[0] != null && tbl.Tables[0].Rows.Count > 0) ? AutoMapper<Product>.Map(tbl.Tables[0]) : new List<Product>();
            listProduct.TotalPage = (tbl.Tables[1] != null && tbl.Tables[1].Rows.Count > 0) ? (int)tbl.Tables[1].Rows[0][0] : 0;
            listProduct.CurrentPage = pr.PageIndex;
            listProduct.SizePage = pr.PageSize;

            return listProduct;
        }
    }
}
