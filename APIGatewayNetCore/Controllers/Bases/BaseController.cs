using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APIGatewayNetCore.Controllers.Bases
{
    public class BaseController : Controller, IAuthorizationFilter
    {
        string _token = string.Empty;

        public BaseController()
        {
        }

        public string AccessToken => _token;

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var isAuthHeader = filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationToken);

            if (isAuthHeader)
            {
                _token = authorizationToken.ToString().Substring("Bearer ".Length);
            }
        }
    }
}