using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.Entities.Organization
{
    public class Product
    {
        public int ProductId { set; get; }

        public int ProductCategoryId { set; get; }

        public int OrganizationId { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập Mã sản phẩm")]
        public string Code { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập đường dẫn ảnh")]
        public string UrlPicture { set; get; }

        public string Description { set; get; }

        public bool IsActive { set; get; }

        public decimal Price { set; get; }

        public DateTime? Date { set; get; }

        public string OrganizationName { set; get; }

        public string UrlLogo { set; get; }

        public int Quantity { set; get; }

        public DateTime? CreatedAtDate { set; get; }

        public int AccountId { set; get; }

        public string AccountName { set; get; }

        public string Phone { set; get; }
    }
}
