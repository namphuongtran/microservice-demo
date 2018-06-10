using System.ComponentModel.DataAnnotations;

namespace Framework.Entities.Organization
{
    public class Organization
    {
        public int OrganizationId { set; get; }

        public int AccountId { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập tên tổ chức")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        public string Phone { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập logo")]
        public string UrlLogo { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập địa chỉ")]
        public string Address { set; get; }

        public bool IsActive { set; get; }
    }
}
