using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Reponsitory.Organizations;
using Framework.Entities.Organization;

namespace Catalog.Controllers.Organizations
{
    [Route("api/[controller]")]
    public class MSV_ProductCategoryController : Controller
    {
        private readonly IMSV_ProductCategoryService _sv;

        public MSV_ProductCategoryController(IMSV_ProductCategoryService sv)
        {
            this._sv = sv;
        }

        [HttpGet("GetListProductCategory")]
        public async Task<List<ProductCategory>> GetInforOrganization()
        {
            return await _sv.GetListProductCategory();
        }
    }
}