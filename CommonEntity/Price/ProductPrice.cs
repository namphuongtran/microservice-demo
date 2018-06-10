using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.Entities.Price
{
    public class ProductPrice
    {
        public int PriceId { set; get; }

        public int ProductCategoryId { set; get; }

        public int ProductId { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập hạn giá")]
        public DateTime? Date { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
        public decimal Amount { set; get; }

        public bool IsActive { set; get; }
    }
}
