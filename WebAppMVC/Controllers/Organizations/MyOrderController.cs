using System.Threading.Tasks;
using System.Web.Mvc;
using Framework.Common.DataPaging;
using Framework.Common.Facache;
using Framework.Common.Http;
using Framework.Entities.API;
using Framework.Entities.Organization;
using WebAppMVC.Controllers.Base;
using WebAppMVC.Models.Systems;

namespace WebAppMVC.Controllers.Organizations
{
    public class MyOrderController : BaseController
    {
        // GET: MyOrder
        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> LoadContent(int pageIndex)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = Configuarations.Url_MyOrder_GetListProduct + "/" + pageIndex.ToString() + "/" + OrganizationId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = AccessToken
                },
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            var listProduct = new GridModel<Product>();
            if (responseData.Code == 200)
            {
                listProduct = Helpers.Deserialize<GridModel<Product>>(responseData.Data);
            }

            return PartialView("_Content", listProduct);
        }
    }
}