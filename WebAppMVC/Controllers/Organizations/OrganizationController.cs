using System.Threading.Tasks;
using System.Web.Mvc;
using Framework.Common.Facache;
using Framework.Common.Http;
using Framework.Entities.API;
using Framework.Entities.Organization;
using Framework.Security;
using WebAppMVC.Controllers.Base;
using WebAppMVC.Models.Systems;

namespace WebAppMVC.Controllers.Organizations
{
    public class OrganizationController : BaseController
    {
        public OrganizationController()
        {
        }

        // GET: Organization
        public async Task<ActionResult> Index()
        {
            Organization ogt;
            if (OrganizationId <= 0 && SystemSession.CurrentAccount != null)
            {
                ogt = await GetInforOrganization();
                SystemSession.CurrentAccount.OrganizationId = ogt != null ? ogt.OrganizationId : OrganizationId;
                SystemSession.CurrentAccount.OrganizationName = ogt != null ? ogt.Name : OrganizationName;
            }

            return View();
        }

        public ActionResult Add()
        {
            Organization ogt = new Organization()
            {
                AccountId = AccountId
            };

            return View("_Add", ogt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<ActionResult> Add(Organization ogt)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = Configuarations.Url_AddOrganization,
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = AccessToken
                },
                FormValue = Helpers.ConvertObToKeyPair(ogt)
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.POST);

            if (responseData.Code == 200)
            {
                SystemSession.CurrentAccount.OrganizationName = ogt.OrganizationId != 0 ? ogt.Name : OrganizationName;
                return RedirectToAction("Index","Organization");
            }
            else
            {
                ModelState.AddModelError("", responseData.Message);
            }

            return View("_Add", ogt);
        }

        public async Task<ActionResult> Edit()
        {
            if (OrganizationId <= 0)
            {
                return View("Index");
            }

            Organization ogt = await GetInforOrganization();

            return View("_Add", ogt);
        }

        public async Task<Organization> GetInforOrganization()
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = OrganizationId > 0 ? Configuarations.Url_GetInforOrganization + "/" + OrganizationId.ToString() : Configuarations.Url_GetInforOrganizationFromAcc + "/" + AccountId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = AccessToken
                }
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            if (responseData.Code == 200)
            {
                return Helpers.Deserialize<Organization>(responseData.Data);
            }

            return new Organization() { OrganizationId = -1 };
        }
    }
}