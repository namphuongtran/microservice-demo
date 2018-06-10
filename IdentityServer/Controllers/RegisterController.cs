using Microsoft.AspNetCore.Mvc;
using IdentityServer.Library;
using System.Threading.Tasks;
using System.Collections.Generic;
using Framework.Common.Facache;
using Framework.Entities.Systems;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private readonly IAccountService _sv;

        public RegisterController(IAccountService sv)
        {
            this._sv = sv;
        }

        [HttpGet("CheckExistAccount/{Email}")]
        public async Task<string> CheckExistAccount(string email)
        {
            bool isExistAccount = await _sv.CheckExistAccount(email);

            return isExistAccount.ToString();
        }

        [HttpPost("CreateAccount")]
        public void CreateAccount(Account acc)
        {
            _sv.CreateAccount(acc);
        }

        [HttpGet("GetInforAccount/{Email}")]
        public async Task<Account> GetInforAccount(string email)
        {
            Account acc = await _sv.GetInforAccount(email);

            return acc;
        }

        [HttpPost("GetListInforAccount")]
        public async Task<List<Account>> GetListInforAccount(string listAccountString)
        {
            List<int> listUser = Helpers.Deserialize<List<int>>(listAccountString);
            List<Account> listAccount = await _sv.GetListInforAccount(listUser);

            return listAccount;
        }
    }
}