using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Attr
{
    public class HeaderAttribute: ActionFilterAttribute
    {
        private StringValues authorizationToken;

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var isAuthHeader = actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);

            string token = string.Empty;
            if (isAuthHeader)
            {
                token = authorizationToken.ToString().Substring("Bearer ".Length);
            }

            JwtSecurityToken secToken = new JwtSecurityToken(token);
            var a = secToken.RawPayload;

            base.OnActionExecuting(actionContext);
        }
    }
}
