using System.ComponentModel.DataAnnotations;

namespace Framework.Entities.Systems
{
    public class Account
    {
        public int AccountId { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập tài khoản")]
        public string Email { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập họ và họ đệm")]
        public string Firstname { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập tên")]
        public string LastName { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập địa chỉ")]
        public string Address { set; get; }
        
        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        public string Phone { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        public string Password { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập lại mật khẩu")]
        public string RePassword { set; get; }

        public bool IsActive { set; get; }

        public string AccessToken { set; get; }

        public string RefreshToken { set; get; }

        public int OrganizationId { set; get; }

        public string OrganizationName { set; get; }
    }
}
