using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Framework.Common.DataPaging;
using Framework.Common.Facache;
using Framework.Entities.Organization;
using Framework.Entities.Price;
using Framework.Entities.Request;
using Microsoft.AspNetCore.Authorization;
using Price.Reponsitory.Prices;

namespace Price.Controllers
{
    [Route("api/[controller]")]
    public class MsvPriceController : Controller
    {
        private readonly IMSV_PriceService _sv;

        public MsvPriceController(IMSV_PriceService sv)
        {
            this._sv = sv;
        }

        [Authorize]
        [HttpPost("AddPrice")]
        public void AddPrice (ProductPrice price)
        {
            _sv.AddPrice(price);
        }

        [Authorize]
        [HttpGet("GetListPricePG/{PageIndex}/{ProductId}/{ProductCategoryId}")]
        public async Task<GridModel<ProductPrice>> GetListPricePg(RequestParams pr)
        {
            return await _sv.GetListPricePG(pr);
        }

        [Authorize]
        [HttpPost("GetPriceFromProduct")]
        public async Task<List<Product>> GetPriceFromProduct(string listProductString)
        {
            List<Product> listProduct = Helpers.Deserialize<List<Product>>(listProductString);

            return await _sv.GetPriceFromProduct(listProduct);
        }

        [HttpGet("Shop_GetListProductPrice/{PageIndex}/{ProductCategoryId}")]
        public async Task<GridModel<Product>> Shop_GetListProductPrice(RequestParams pr)
        {
            return await _sv.Shop_GetListProductPrice(pr);
        }
    }
}