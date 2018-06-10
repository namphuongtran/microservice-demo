using IdentityServer.Library;
using IdentityServer4.Validation;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace IdentityServer.Configuration
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IAccountService _sv;

        public ResourceOwnerPasswordValidator()
        {
            this._sv = new AccountService();
        }

        // Here we validate our users        
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (!_sv.IsValidAccount(context.UserName, context.Password))
            {
                context.Result = new GrantValidationResult()
                {
                    IsError = true,
                    Error = TokenErrors.InvalidRequest,
                    ErrorDescription = "Sai thông tin tài khoản hoặc mật khẩu."
                };

                return Task.FromResult(0);
            }

            context.Result = new GrantValidationResult(context.UserName, "username");
            return Task.FromResult(0);
        }
    }
}
