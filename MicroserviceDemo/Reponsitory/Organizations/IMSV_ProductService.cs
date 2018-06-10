using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Common.DataPaging;
using Framework.Entities.Organization;
using Framework.Entities.Request;

namespace Catalog.Reponsitory.Organizations
{
    public interface IMSV_ProductService
    {
        void AddProduct(Product product);
        void DeleteProduct(int productId);
        Task<Product> GetInforProduct(int productId);
        Task<List<Product>> GetListProduct(int productCategoryId, int organizationId);
        Task<GridModel<Product>> GetListProductPG(RequestParams pr);
        Task<List<Product>> Shop_GetListProduct(List<Product> listProduct);
    }
}