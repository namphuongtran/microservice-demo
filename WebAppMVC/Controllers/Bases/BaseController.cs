using System.Web.Mvc;
using Framework.Security;
namespace WebAppMVC.Controllers.Base
{
    public class BaseController : Controller, IAuthorizationFilter
    {
        // GET: Base
        public BaseController()
        {
        }

        public static int AccountId
        {
            get
            {
                if (SystemSession.CurrentAccount != null)
                {
                    return SystemSession.CurrentAccount.AccountId;
                }

                return 0;
            }
        }

        public static string Email
        {
            get
            {
                if (SystemSession.CurrentAccount != null)
                {
                    return SystemSession.CurrentAccount.Email;
                }

                return string.Empty;
            }
        }

        public static string AccessToken
        {
            get
            {
                if (SystemSession.CurrentAccount != null)
                {
                    return SystemSession.CurrentAccount.AccessToken;
                }

                return string.Empty;
            }
        }

        public static int OrganizationId
        {
            get
            {
                if (SystemSession.CurrentAccount != null)
                {
                    return SystemSession.CurrentAccount.OrganizationId;
                }

                return 0;
            }
        }

        public static string OrganizationName
        {
            get
            {
                if (SystemSession.CurrentAccount != null)
                {
                    return SystemSession.CurrentAccount.OrganizationName;
                }

                return string.Empty;
            }
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SystemSession.CurrentAccount == null)
            {
                filterContext.Result = RedirectToAction("Index", "Login", new { targetUrl = Request.Url.ToString() });
            }

            base.OnAuthorization(filterContext);
        }
    }
}