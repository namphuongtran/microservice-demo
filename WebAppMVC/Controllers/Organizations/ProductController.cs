using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Framework.Common.DataPaging;
using Framework.Common.Facache;
using Framework.Common.Http;
using Framework.Entities.API;
using Framework.Entities.Organization;
using Framework.Entities.Price;
using WebAppMVC.Controllers.Base;
using WebAppMVC.Models.Organizations;
using WebAppMVC.Models.Systems;

namespace WebAppMVC.Controllers.Organizations
{
    public class ProductController : BaseController
    {
        public ProductController()
        {
        }

        // GET: Product
        public async Task<ActionResult> Index()
        {
            // Get list ProductCategory
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = Configuarations.Url_Product_GetListProductCategory,
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = AccessToken
                },
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
                UrlBase = Configuarations.Url_Product_GetListByConditional + "/" + pageIndex.ToString() + "/" + productCategoryId.ToString() + "/" + OrganizationId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = AccessToken
                },
            };

            GridModel<Product> listProduct = new GridModel<Product>();
            if (productCategoryId > 0)
            {
                var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

                if (responseData.Code != 200)
                {
                    ViewBag.ResultData = null;
                    return View("index");
                }

                listProduct = Helpers.Deserialize<GridModel<Product>>(responseData.Data);
            }

            return PartialView("_Content", listProduct);
        }

        public PartialViewResult Add(int productCategoryId)
        {
            Product product = new Product()
            {
                ProductCategoryId = productCategoryId,
                OrganizationId = OrganizationId
            };

            return PartialView("_Add", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(Product product)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = Configuarations.Url_Product_Add,
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = AccessToken
                },
                FormValue = Helpers.ConvertObToKeyPair(product)
            };

            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.POST);

            if (responseData.Code != 200)
            {
                ViewBag.ResultData = null;
                return View("index");
            }

            return PartialView("_Add", product);
        }

        public async Task<ActionResult> Edit(int productId)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = Configuarations.Url_Product_GetInfor + "/" + productId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = AccessToken
                },
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            if (responseData.Code != 200)
            {
                return RedirectToAction("Index", "Product");
            }

            Product product = Helpers.Deserialize<Product>(responseData.Data);

            return PartialView("_Add", product);
        }

        public async Task Delete(int productId)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = Configuarations.Url_Product_Delete + "/" + productId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = AccessToken
                },
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.DELETE);
        }

        public async Task<PartialViewResult> UpdatePrice(int productId, int productCategoryId)
        {
            var listProductPrice = await GetListPriceFromProduct(productId, productCategoryId);

            ProductPriceModel model = new ProductPriceModel();
            model.ProductPrice = listProductPrice.Data[0];

            if (listProductPrice.Data != null && listProductPrice.Data.Count > 0)
            {
                listProductPrice.Data.RemoveAt(0);
            }              

            model.ListProductPrice = listProductPrice;

            return PartialView("_UpdatePrice", model);
        }

        [HttpPost]
        public async Task<PartialViewResult> UpdatePrice(ProductPriceModel model)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = Configuarations.Url_Product_UpdatePrice,
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = AccessToken
                },
                FormValue = Helpers.ConvertObToKeyPair(model.ProductPrice)
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.POST);

            return PartialView("_UpdatePrice", model);
        }

        public async Task<GridModel<ProductPrice>> GetListPriceFromProduct(int productId, int productCategoryId)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = Configuarations.Url_Product_GetListPricePG + "/0/" + productId.ToString() + "/" + productCategoryId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = AccessToken
                },
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            GridModel<ProductPrice> listProductPrice = new GridModel<ProductPrice>();
            if (responseData.Code == 200)
            {
                listProductPrice = Helpers.Deserialize<GridModel<ProductPrice>>(responseData.Data);
            }

            return listProductPrice;
        }
    }
}