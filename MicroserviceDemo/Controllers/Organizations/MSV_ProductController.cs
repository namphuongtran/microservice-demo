using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Catalog.Reponsitory.Organizations;
using Microsoft.AspNetCore.Authorization;
using Framework.Common.DataPaging;
using Framework.Common.Facache;
using Framework.Entities.Organization;
using Framework.Entities.Request;

namespace Catalog.Controllers.Organizations
{
    [Route("api/[controller]")]
    public class MSV_ProductController : Controller
    {
        private readonly IMSV_ProductService _sv;
        public MSV_ProductController(IMSV_ProductService sv)
        {
            this._sv = sv;
        }

        [Authorize]
        [HttpGet("GetListProductPG/{PageIndex}/{ProductCategoryId}/{OrganizationId}")]
        public async Task<GridModel<Product>> GetListProductPG(RequestParams pr)
        {
            return await _sv.GetListProductPG(pr);
        }

        [Authorize]
        [HttpGet("GetListProduct/{productCategoryId}/{organizationId}")]
        public async Task<List<Product>> GetListProduct(int productCategoryId, int organizationId)
        {
            return await _sv.GetListProduct(productCategoryId, organizationId);
        }

        [Authorize]
        [HttpGet("GetInforProduct/{productId}")]
        public async Task<Product> GetInforProduct(int productId)
        {
            return await _sv.GetInforProduct(productId);
        }

        [Authorize]
        [HttpPost("AddProduct")]
        public void GetInforProduct(Product product)
        {
            _sv.AddProduct(product);
        }

        [Authorize]
        [HttpDelete("DeleteProduct/{productId}")]
        public void DeleteProduct(int productId)
        {
            _sv.DeleteProduct(productId);
        }

        [HttpPost("Shop_GetListProduct")]
        public async Task<List<Product>> Shop_GetListProduct(string listProductString)
        {
            List<Product> listProduct = Helpers.Deserialize<List<Product>>(listProductString);

            return await _sv.Shop_GetListProduct(listProduct);
        }
    }
}