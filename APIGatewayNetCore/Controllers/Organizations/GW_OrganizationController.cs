using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using APIGatewayNetCore.Controllers.Bases;
using Framework.Common.Facache;
using Framework.Common.Http;
using Framework.Entities.API;
using Framework.Entities.Organization;

namespace APIGatewayNetCore.Controllers.Organizations
{
    [Route("api/[controller]")]
    [Authorize]
    //[HeaderAttribute]
    public class GW_OrganizationController : BaseController
    {
        private string _accessToken;
        private StringValues _authorizationToken;

        public GW_OrganizationController()
        {
        }

        [HttpPost("AddOrganization")]
        public void AddOrganization(Organization ogt)
        {
            if (Request.Headers.TryGetValue("Authorization", out _authorizationToken))
            {
                this._accessToken = _authorizationToken.ToString().Substring("Bearer ".Length);
            }

            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlCatalogAddOrganization,
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = _accessToken
                },
                FormValue = Helpers.ConvertObToKeyPair(ogt)
            };
            var responseData = ConnectAPI.ConnectRestAPI(requestInfor, MethodType.POST);
        }

        [HttpGet("GetInforOrganization/{organizationId}")]
        public async Task<Organization> GetInforOrganization(int organizationId)
        {
            if (Request.Headers.TryGetValue("Authorization", out _authorizationToken))
            {
                this._accessToken = _authorizationToken.ToString().Substring("Bearer ".Length);
            }

            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlCatalogGetInforOrganization + "/" + organizationId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = _accessToken
                }
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            if (responseData.Code == 200)
            {
                return Helpers.Deserialize<Organization>(responseData.Data);
            }

            return new Organization() { OrganizationId = -1 };
        }

        [HttpGet("GetInforOrganizationFromAcc/{accountId}")]
        public async Task<Organization> GetInforOrganizationFromAcc(int accountId)
        {
            if (Request.Headers.TryGetValue("Authorization", out _authorizationToken))
            {
                this._accessToken = _authorizationToken.ToString().Substring("Bearer ".Length);
            }

            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = StaticConfig.UrlCatalogGetInforOrganizationFromAcc + "/" + accountId.ToString(),
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Bearer",
                    AuthorizationValue = _accessToken
                }
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            if (responseData.Code == 200)
            {
                return Helpers.Deserialize<Organization>(responseData.Data);
            }

            return new Organization() { OrganizationId = -1 };
        }

        //[HttpGet("Test_Consul")]
        //public async Task<string> Test_Consul()
        //{
        //    RequestInfor requestInfor = new RequestInfor()
        //    {
        //        UrlBase = StaticConfig.Service_Test_Consul
        //    };
        //    var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

        //    if (responseData.Code == 200)
        //    {
        //        return responseData.Data;
        //    }

        //    return "Not OK!";
        //}
    }
}