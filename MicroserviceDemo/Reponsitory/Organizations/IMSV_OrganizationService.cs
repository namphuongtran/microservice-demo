using System.Threading.Tasks;
using Framework.Entities.Organization;

namespace Catalog.Reponsitory.Organizations
{
    public interface IMSV_OrganizationService
    {
        void AddOrganization(Organization ogt);

        Task<Organization> GetInforOrganization(int organizationId);

        Task<Organization> GetInforOrganizationFromAcc(int accountId);
    }
}
