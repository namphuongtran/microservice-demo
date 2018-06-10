using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Common.DataPaging;
using Framework.Common.Facache;
using Framework.Entities.Organization;
using Framework.Entities.Payments.Extensions;
using Framework.Entities.Request;
using Payment.Reponsitory.Payments;

namespace Payment.Controllers
{
    [Route("api/[controller]")]
    public class MSV_PaymentController : Controller
    {
        private readonly IMSV_PaymentService _sv;

        public MSV_PaymentController(IMSV_PaymentService sv)
        {
            this._sv = sv;
        }

        [HttpPost("Payment")]
        public void Payment(string paymentString)
        {
            PaymentModel payment = Helpers.Deserialize<PaymentModel>(paymentString);

            _sv.Payment(payment);
        }

        [HttpGet("ViewHistory/{PageIndex}/{AccountId}")]
        public async Task<GridModel<Framework.Entities.Payments.Payment>> ViewHistory(RequestParams pr)
        {
            return await _sv.ViewHistory(pr);
        }

        [HttpGet("MyOrder_GetListProduct/{PageIndex}/{OrganizationId}")]
        public async Task<GridModel<Product>> MyOrder_GetListProduct(RequestParams pr)
        {
            return await _sv.MyOrder_GetListProduct(pr);
        }
    }
}