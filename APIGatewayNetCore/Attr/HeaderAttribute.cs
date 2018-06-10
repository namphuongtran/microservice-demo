using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace APIGatewayNetCore.Attr
{
    public class HeaderAttribute : ActionFilterAttribute
    {
        private StringValues _authorizationToken;

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var isAuthHeader = actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out _authorizationToken);

            string token = string.Empty;
            if (isAuthHeader)
            {
                token = _authorizationToken.ToString().Substring("Bearer ".Length);
            }

            JwtSecurityToken secToken = new JwtSecurityToken(token);

            var username = secToken.Claims.First(claim => claim.Type == "sub").Value;

            base.OnActionExecuting(actionContext);
        }
    }
}
