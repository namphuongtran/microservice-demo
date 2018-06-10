using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Entities.Systems;

namespace IdentityServer.Library
{
    public interface IAccountService
    {
        Account GetInforAccount();
        bool IsValidAccount(string email, string passWord);
        Task<bool> IsValidUser(string email, string passWord);
        Task<bool> CheckExistAccount(string email);
        void CreateAccount(Account acc);
        Task<Account> GetInforAccount(string email);
        Task<List<Account>> GetListInforAccount(List<int> listUser);
    }
}
