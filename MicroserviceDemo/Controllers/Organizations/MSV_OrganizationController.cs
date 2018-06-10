using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityServer.Attr;
using Catalog.Reponsitory.Organizations;
using Framework.Entities.Organization;

namespace Catalog.Controllers.Organizations
{
    [Route("api/[controller]")]
    [Authorize]
    [HeaderAttribute]
    public class MSV_OrganizationController : Controller
    {
        private readonly IMSV_OrganizationService _sv;

        public MSV_OrganizationController(IMSV_OrganizationService sv)
        {
            this._sv = sv;
        }

        [HttpPost("AddOrganization")]
        public void AddOrganization(Organization ogt)
        {
            _sv.AddOrganization(ogt);
        }

        [HttpGet("GetInforOrganization/{organizationId}")]
        public async Task<Organization> GetInforOrganization(int organizationId)
        {
            return await _sv.GetInforOrganization(organizationId);
        }

        [HttpGet("GetInforOrganizationFromAcc/{accountId}")]
        public async Task<Organization> GetInforOrganizationFromAcc(int accountId)
        {
            return await _sv.GetInforOrganizationFromAcc(accountId);
        }
    }
}