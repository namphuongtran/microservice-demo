using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Core.SQL.SQLServer;
using Framework.Automap.SQLServer;
using Framework.Common.DataPaging;
using Framework.Entities.Organization;
using Framework.Entities.Payments.Extensions;
using Framework.Entities.Request;

namespace Payment.Reponsitory.Payments
{
    public class MSV_PaymentService : IMSV_PaymentService
    {
        private readonly IConnectSQL _db;

        public MSV_PaymentService(IConnectSQL db)
        {
            _db = db;
        }

        public void Payment(PaymentModel payment)
        {
            DataTable dtb = new DataTable();
            dtb.Columns.Add(new DataColumn("OrganizationId", typeof(int)));
            dtb.Columns.Add(new DataColumn("ProductId", typeof(int)));
            dtb.Columns.Add(new DataColumn("Quantity", typeof(int)));
            dtb.Columns.Add(new DataColumn("Price", typeof(decimal)));

            foreach (var item in payment.ListPaymentDetail)
            {
                var dr = dtb.NewRow();
                dr["OrganizationId"] = item.OrganizationId;
                dr["ProductId"] = item.ProductId;
                dr["Quantity"] = item.Quantity;
                dr["Price"] = item.Price;
                dtb.Rows.Add(dr);
            }

            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_AccountId", payment.Payment.AccountId);
            sqlParams.Add_Parameter("@_PaymentCode", payment.Payment.PaymentCode);
            sqlParams.Add_Parameter("@_ListPayment", dtb);
            _db.ExecuteNonQuery("usp_Payment_InsertOrUpdate", sqlParams, ExecuteType.StoredProcedure);
        }

        public async Task<GridModel<Framework.Entities.Payments.Payment>> ViewHistory (RequestParams pr)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_AccountId", pr.AccountId);
            sqlParams.Add_Parameter("@_PageSize", pr.PageSize);
            sqlParams.Add_Parameter("@_PageIndex", pr.PageIndex);

            var tbl = _db.ExecuteToDataset("usp_Payment_ViewHistory", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            GridModel<Framework.Entities.Payments.Payment> listHistory = new GridModel<Framework.Entities.Payments.Payment>();
            listHistory.Data = (tbl.Tables[0] != null && tbl.Tables[0].Rows.Count > 0) ? AutoMapper<Framework.Entities.Payments.Payment>.Map(tbl.Tables[0]) : new List<Framework.Entities.Payments.Payment>();
            listHistory.TotalPage = (tbl.Tables[1] != null && tbl.Tables[1].Rows.Count > 0) ? (int)tbl.Tables[1].Rows[0][0] : 0;
            listHistory.CurrentPage = pr.PageIndex;
            listHistory.SizePage = pr.PageSize;

            return listHistory;
        }

        public async Task<GridModel<Product>> MyOrder_GetListProduct(RequestParams pr)
        {
            SQLParameters sqlParams = new SQLParameters();
            sqlParams.Add_Parameter("@_OrganizationId", pr.OrganizationId);
            sqlParams.Add_Parameter("@_PageSize", pr.PageSize);
            sqlParams.Add_Parameter("@_PageIndex", pr.PageIndex);

            var tbl = _db.ExecuteToDataset("usp_MyOrder_GetListByConditional", sqlParams, ExecuteType.StoredProcedure);
            await Task.FromResult(tbl);

            GridModel<Product> listHistory = new GridModel<Product>();
            listHistory.Data = (tbl.Tables[0] != null && tbl.Tables[0].Rows.Count > 0) ? AutoMapper<Product>.Map(tbl.Tables[0]) : new List<Product>();
            listHistory.TotalPage = (tbl.Tables[1] != null && tbl.Tables[1].Rows.Count > 0) ? (int)tbl.Tables[1].Rows[0][0] : 0;
            listHistory.CurrentPage = pr.PageIndex;
            listHistory.SizePage = pr.PageSize;

            return listHistory;
        }
    }
}
