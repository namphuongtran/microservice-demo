using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Common.DataPaging;
using Framework.Entities.Organization;
using Framework.Entities.Price;
using Framework.Entities.Request;

namespace Price.Reponsitory.Prices
{
    public interface IMSV_PriceService
    {
        void AddPrice(ProductPrice price);
        Task<GridModel<ProductPrice>> GetListPricePG(RequestParams pr);
        Task<List<Product>> GetPriceFromProduct(List<Product> listProduct);
        Task<GridModel<Product>> Shop_GetListProductPrice(RequestParams pr);
    }
}