using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Framework.Common.DataPaging;
using Framework.Common.Facache;
using Framework.Common.Http;
using Framework.Entities.API;
using Framework.Entities.Organization;
using Framework.Entities.Payments;
using Framework.Entities.Payments.Extensions;
using Framework.Entities.Personals;
using Framework.Security;
using WebAppMVC.Models.Systems;

namespace WebAppMVC.Controllers.Personals
{
    public class ShopController : Controller
    {
        // GET: Shop
        public async Task<ActionResult> Index()
        {
            // Get list ProductCategory
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = Configuarations.Url_Product_GetListProductCategory
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            if (responseData.Code != 200)
            {
                ViewBag.ResultData = null;

                if (responseData.Code == 401)
                {
                    ViewBag.ResultMessage = responseData.Message;
                }
            }
            else
            {
                ViewBag.ResultData = Helpers.Deserialize<List<ProductCategory>>(responseData.Data);
            }

            return View();
        }

        public async Task<ActionResult> LoadContent(int productCategoryId, int pageIndex)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = Configuarations.Url_Shop_GetListProductPrice + "/" + pageIndex.ToString() + "/" + productCategoryId.ToString()
            };

            GridModel<Product> listProduct = new GridModel<Product>();
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            if (responseData.Code != 200)
            {
                ViewBag.ResultData = null;
                return View("index");
            }

            listProduct = Helpers.Deserialize<GridModel<Product>>(responseData.Data);

            return PartialView("_Content", listProduct);
        }

        public int AddShop(Cart shopCart)
        {
            if (SystemSession.ShopCart == null)
            {
                SystemSession.ShopCart = new List<Cart>()
                {
                    shopCart
                };

                return 1;
            }
            
            if (SystemSession.ShopCart.Exists(x => x.ProductId == shopCart.ProductId))
            {
                var cart = SystemSession.ShopCart.First(x => x.ProductId == shopCart.ProductId);
                cart.Quantity++;
                int index = SystemSession.ShopCart.FindIndex(x => x.ProductId == cart.ProductId);
                SystemSession.ShopCart.RemoveAt(index);
                SystemSession.ShopCart.Insert(index, cart);
            } else
            {
                SystemSession.ShopCart.Add(shopCart);
            }

            return SystemSession.ShopCart.Count;
        }

        public PartialViewResult ShopCart()
        {
            List<Cart> listCart = SystemSession.ShopCart ?? new List<Cart>();

            return PartialView("_Cart", listCart);
        }

        [HttpPost]
        public async Task<PartialViewResult> ShopCart(List<Cart> cart)
        {
            if (SystemSession.ShopCart != null && SystemSession.ShopCart.Count > 0)
            {
                PaymentModel payment = new PaymentModel()
                {
                    Payment = new Payment()
                    {
                        AccountId = SystemSession.CurrentAccount.AccountId,
                        PaymentCode = Helpers.RandomString(8, false)
                    },
                    ListPaymentDetail = SystemSession.ShopCart
                };
                string paymentString = Helpers.Serialize(payment);

                RequestInfor requestInfor = new RequestInfor()
                {
                    UrlBase = Configuarations.Url_Shop_Payment,
                    FormValue = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("paymentString",paymentString)
                }
                };
                var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.POST);

                if (responseData.Code == 200)
                {
                    SystemSession.ShopCart = null;
                }
            }

            return PartialView("_Cart", cart);
        }

        public void EditQuantity(int productId, int quantity)
        {
            var cart = SystemSession.ShopCart.First(x => x.ProductId == productId);
            cart.Quantity = quantity;
            int index = SystemSession.ShopCart.FindIndex(x => x.ProductId == cart.ProductId);
            SystemSession.ShopCart.RemoveAt(index);
            SystemSession.ShopCart.Insert(index, cart);
        }

        public PartialViewResult RemoveItem(int productId)
        {
            SystemSession.ShopCart.RemoveAll(x => x.ProductId == productId);

            List<Cart> listCart = SystemSession.ShopCart ?? new List<Cart>();

            return PartialView("_Cart", listCart);
        }

        public async Task<PartialViewResult> ViewHistory()
        {
            var listHistory = new GridModel<Payment>();

            if (SystemSession.CurrentAccount != null)
            {
                RequestInfor requestInfor = new RequestInfor()
                {
                    UrlBase = Configuarations.Url_Shop_ViewHistory + "/0" + "/" + SystemSession.CurrentAccount.AccountId.ToString()
                };
                var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

                if (responseData.Code == 200)
                {
                    listHistory = Helpers.Deserialize<GridModel<Payment>>(responseData.Data);
                }
            }

            return PartialView("_History", listHistory);
        }
    }
}