using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Framework.Common.DataPaging;
using Framework.Common.Facache;
using Framework.Common.Http;
using Framework.Entities.API;
using Framework.Entities.Organization;
using Framework.Entities.Payments;
using Framework.Entities.Request;

namespace APIGatewayNetCore.Controllers.Personals
{
    [Route("api/[controller]")]
    public class GW_ShopController : Controller
    {
        [HttpGet("Shop_GetListProductPrice/{PageIndex}/{ProductCategoryId}")]
        public async Task<GridModel<Product>> Shop_GetListProductPrice(RequestParams pr)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlPriceShopGetListProductPrice + "/" + pr.PageIndex.ToString() + "/" + pr.ProductCategoryId.ToString()
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            GridModel<Product> listProductModel = new GridModel<Product>();
            if (responseData.Code == 200)
            {
                listProductModel = Helpers.Deserialize<GridModel<Product>>(responseData.Data);
                string listProductString = Helpers.Serialize(listProductModel.Data);

                requestInfor = new RequestInfor()
                {
                    UrlBase = StaticConfig.UrlCatalogShopGetListProduct,
                    FormValue = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("listProductString",listProductString)
                    }
                };
                responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.POST);

                if (responseData.Code == 200)
                {
                    var listProductResult = Helpers.Deserialize<List<Product>>(responseData.Data);
                    var newListProduct = new List<Product>();

                    foreach(var item in listProductResult)
                    {
                        item.Price = listProductModel.Data.First(x => x.ProductId == item.ProductId).Price;
                        item.Date = listProductModel.Data.First(x => x.ProductId == item.ProductId).Date;
                        newListProduct.Add(item);
                    }

                    listProductModel.Data = newListProduct;
                }
            }

            return listProductModel;
        }

        [HttpPost("Payment")]
        public async Task Payment(string paymentString)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlPaymentPayment,
                FormValue = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("paymentString", paymentString)
                }
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.POST);
        }

        [HttpGet("ViewHistory/{PageIndex}/{AccountId}")]
        public async Task<GridModel<Payment>> ViewHistory(RequestParams pr)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlPaymentViewHistory + "/" + pr.PageIndex.ToString() + "/" + pr.AccountId.ToString()
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            var listHistory = new GridModel<Payment>();
            if (responseData.Code == 200)
            {
                listHistory = Helpers.Deserialize<GridModel<Payment>>(responseData.Data);
            }

            return listHistory;
        }
    }
}