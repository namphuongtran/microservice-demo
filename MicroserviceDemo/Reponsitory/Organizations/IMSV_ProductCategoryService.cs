using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Entities.Organization;

namespace Catalog.Reponsitory.Organizations
{
    public interface IMSV_ProductCategoryService
    {
        Task<List<ProductCategory>> GetListProductCategory();
    }
}