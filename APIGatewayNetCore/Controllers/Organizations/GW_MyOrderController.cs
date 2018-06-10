using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Framework.Common.DataPaging;
using Framework.Common.Facache;
using Framework.Common.Http;
using Framework.Entities.API;
using Framework.Entities.Organization;
using Framework.Entities.Request;
using Framework.Entities.Systems;

namespace APIGatewayNetCore.Controllers.Organizations
{
    [Route("api/[controller]")]
    [Authorize]
    public class GW_MyOrderController : Controller
    {
        [HttpGet("MyOrder_GetListProduct/{PageIndex}/{OrganizationId}")]
        public async Task<GridModel<Product>> MyOrder_GetListProduct(RequestParams pr)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlPaymentMyOrderGetListProduct + "/" + pr.PageIndex.ToString() + "/" + pr.OrganizationId.ToString()
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            var listProduct = new GridModel<Product>();
            if (responseData.Code == 200)
            {
                listProduct = Helpers.Deserialize<GridModel<Product>>(responseData.Data);
                string listProductString = Helpers.Serialize(listProduct.Data);

                //Get infor product
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
                    var listProductInfor = Helpers.Deserialize<List<Product>>(responseData.Data);

                    var newListProduct = new List<Product>();
                    foreach (var item in listProduct.Data)
                    {
                        item.Code = listProductInfor.FindLast(x => x.ProductId == item.ProductId).Code;
                        item.Name = listProductInfor.FindLast(x => x.ProductId == item.ProductId).Name;
                        newListProduct.Add(item);
                    }

                    listProduct.Data = newListProduct;
                }

                //Get infor account
                string listAccountString = Helpers.Serialize(listProduct.Data.Select(x => x.AccountId).Distinct().ToList());
                requestInfor = new RequestInfor()
                {
                    UrlBase = StaticConfig.UrlMyOrderGetListInforAccount,
                    FormValue = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("listAccountString",listAccountString)
                    }
                };
                responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.POST);

                if(responseData.Code == 200)
                {
                    var listAccountInfor = Helpers.Deserialize<List<Account>>(responseData.Data);

                    var newListProduct = new List<Product>();
                    foreach (var item in listProduct.Data)
                    {
                        var account = listAccountInfor.FindLast(x => x.AccountId == item.AccountId);
                        item.AccountName = account.Firstname + " " + account.LastName;
                        item.Phone = account.Phone;
                        newListProduct.Add(item);
                    }

                    listProduct.Data = newListProduct;
                }
            }

            return listProduct;
        }
    }
}