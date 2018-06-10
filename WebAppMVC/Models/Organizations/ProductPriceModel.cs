using Framework.Common.DataPaging;
using Framework.Entities.Price;

namespace WebAppMVC.Models.Organizations
{
    public class ProductPriceModel
    {
        public ProductPrice ProductPrice { set; get; }

        public GridModel<ProductPrice> ListProductPrice { set; get; }
    }
}