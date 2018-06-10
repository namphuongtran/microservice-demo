using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authorization;
using Framework.Common.DataPaging;
using Framework.Common.Facache;
using Framework.Common.Http;
using Framework.Entities.API;
using Framework.Entities.Organization;
using Framework.Entities.Price;
using Framework.Entities.Request;

namespace APIGatewayNetCore.Controllers.Organizations
{
    [Route("api/[controller]")]
    //[HeaderAttribute]
    public class GW_ProductController : Controller
    {
        private StringValues _authorizationToken;
        private string _accessToken;

        public GW_ProductController()
        {
        }

        [HttpGet("GetListProductCategory")]
        public async Task<List<ProductCategory>> GetListProductCategory()
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlCatalogGetListProductCategory
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            if (responseData.Code == 200)
            {
                return Helpers.Deserialize<List<ProductCategory>>(responseData.Data);
            }

            return new List<ProductCategory>();
        }

        [Authorize]
        [HttpGet("GetInforProduct/{productId}")]
        public async Task<Product> GetInforProduct(int productId)
        {
            if (Request.Headers.TryGetValue("Authorization", out _authorizationToken))
            {
                this._accessToken = _authorizationToken.ToString().Substring("Bearer ".Length);
            }

            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlCatalogGetInforProduct + "/" + productId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = _accessToken
                }
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            if (responseData.Code == 200)
            {
                return Helpers.Deserialize<Product>(responseData.Data);
            }

            return new Product() { ProductId = -1 };
        }

        [Authorize]
        [HttpGet("GetListProduct/{productCategoryId}/{organizationId}")]
        public async Task<List<Product>> GetListProduct(int productCategoryId, int organizationId)
        {
            if (Request.Headers.TryGetValue("Authorization", out _authorizationToken))
            {
                this._accessToken = _authorizationToken.ToString().Substring("Bearer ".Length);
            }

            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlCatalogGetListProduct + "/" + productCategoryId.ToString() + "/" + organizationId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = _accessToken
                }
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            if (responseData.Code == 200)
            {
                return Helpers.Deserialize<List<Product>>(responseData.Data);
            }

            return new List<Product>();
        }

        [Authorize]
        [HttpGet("GetListProductPG/{PageIndex}/{ProductCategoryId}/{OrganizationId}")]
        public async Task<GridModel<Product>> GetListProductPG(RequestParams pr)
        {
            if (Request.Headers.TryGetValue("Authorization", out _authorizationToken))
            {
                this._accessToken = _authorizationToken.ToString().Substring("Bearer ".Length);
            }

            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlCatalogGetListProductPg + "/" + pr.PageIndex.ToString() + "/" + pr.ProductCategoryId.ToString() + "/" + pr.OrganizationId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = _accessToken
                }
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);
            
            GridModel<Product> listProduct = new GridModel<Product>();

            if (responseData.Code == 200)
            {
                listProduct = Helpers.Deserialize<GridModel<Product>>(responseData.Data);
                string listProductString = Helpers.Serialize(listProduct.Data);

                requestInfor = new RequestInfor()
                {
                    UrlBase = StaticConfig.UrlPriceGetPriceFromProduct,
                    HeaderValue = new HeaderValue()
                    {
                        AuthorizationType = "Bearer",
                        AuthorizationValue = _accessToken
                    },
                    FormValue = new List<KeyValuePair<string, string>>() {
                        new KeyValuePair<string, string>("listProductString", listProductString)
                    }
                };
                responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.POST);

                var listProductPrice = new List<Product>();
                var oldListProduct = listProduct.Data;
                var newListProduct = new List<Product>();
                
                if (responseData.Code == 200)
                {
                    listProductPrice = Helpers.Deserialize<List<Product>>(responseData.Data);

                    foreach (var item in oldListProduct)
                    {
                        item.Price = listProductPrice.FirstOrDefault(x => x.ProductId == item.ProductId).Price;
                        newListProduct.Add(item);
                    }

                    listProduct.Data = newListProduct;
                }
            }

            return listProduct;
        }

        [Authorize]
        [HttpPost("AddProduct")]
        public async Task AddProduct(Product product)
        {
            if (Request.Headers.TryGetValue("Authorization", out _authorizationToken))
            {
                this._accessToken = _authorizationToken.ToString().Substring("Bearer ".Length);
            }

            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlCatalogAddProduct,
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = _accessToken
                },
                FormValue = Helpers.ConvertObToKeyPair(product)
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.POST);
        }

        [Authorize]
        [HttpDelete("DeleteProduct/{productId}")]
        public async Task DeleteProduct(int productId)
        {
            if (Request.Headers.TryGetValue("Authorization", out _authorizationToken))
            {
                this._accessToken = _authorizationToken.ToString().Substring("Bearer ".Length);
            }

            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlCatalogDeleteProduct + "/" + productId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = _accessToken
                }
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.DELETE);
        }

        [Authorize]
        [HttpGet("GetListPricePG/{PageIndex}/{ProductId}/{ProductCategoryId}")]
        public async Task<GridModel<ProductPrice>> GetListPricePG(RequestParams pr)
        {
            if (Request.Headers.TryGetValue("Authorization", out _authorizationToken))
            {
                this._accessToken = _authorizationToken.ToString().Substring("Bearer ".Length);
            }

            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlPriceGetListPricePg + "/" + pr.PageIndex.ToString() + "/" + pr.ProductId.ToString() + "/" + pr.ProductCategoryId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = _accessToken
                }
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            GridModel<ProductPrice> listPrice = new GridModel<ProductPrice>();
            if (responseData.Code == 200)
            {
                listPrice = Helpers.Deserialize<GridModel<ProductPrice>>(responseData.Data);
            }

            return listPrice;
        }

        [Authorize]
        [HttpPost("UpdatePrice")]
        public async Task UpdatePrice(ProductPrice productPrice)
        {
            if (Request.Headers.TryGetValue("Authorization", out _authorizationToken))
            {
                this._accessToken = _authorizationToken.ToString().Substring("Bearer ".Length);
            }

            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlPriceUpdatePrice,
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = _accessToken
                },
                FormValue = Helpers.ConvertObToKeyPair(productPrice)
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.POST);
        }
    }
}