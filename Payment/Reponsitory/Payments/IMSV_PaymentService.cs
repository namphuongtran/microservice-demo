using System.Threading.Tasks;
using Framework.Common.DataPaging;
using Framework.Entities.Organization;
using Framework.Entities.Payments.Extensions;
using Framework.Entities.Request;

namespace Payment.Reponsitory.Payments
{
    public interface IMSV_PaymentService
    {
        void Payment(PaymentModel payment);
        Task<GridModel<Framework.Entities.Payments.Payment>> ViewHistory(RequestParams pr);
        Task<GridModel<Product>> MyOrder_GetListProduct(RequestParams pr);
    }
}