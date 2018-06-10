using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Framework.Common.Facache;
using Framework.Common.Http;
using Framework.Entities.API;
using Framework.Entities.Systems;
using Framework.Security;
using WebAppMVC.Models.Systems;

namespace WebAppMVC.Controllers.System
{
    public class LoginController : Controller
    {
        public LoginController()
        {
        }

        // GET: Login
        public ActionResult Index()
        {
            if (SystemSession.CurrentAccount != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<ActionResult> Index(Account acc)
        {
            string values = string.Empty;

            var tokenInfor = await GetAccessToken(acc.Email, acc.Password);

            if (tokenInfor.Code == 200)
            {
                return await CreateSessionAccount(acc.Email, tokenInfor.Data);
            }
            else
            {
                ModelState.AddModelError("", tokenInfor.Message);
            }

            #region Data Test
            //values = "Respon data:" + "<br/>";
            //values += "- Code: " + tokenInfor.Code.ToString() + "<br/>";
            //values += "- Data: " + tokenInfor.Data + "<br/>";
            //values += "- Message: " + tokenInfor.Message;
            #endregion

            ViewBag.Values = values;

            return View();
        }

        public ActionResult Register()
        {
            if (SystemSession.CurrentAccount != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("_Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<ActionResult> Register(Account acc)//async Task<ActionResult>
        {
            bool isValid = true;

            //Validate form data
            if (acc.Password != acc.RePassword)
            {
                isValid = false;
                ModelState.AddModelError("RePassword", "Nhập lại Password chưa đúng");
            }

            //Validate logic data
            if (acc.AccountId == 0)
            {
                RequestInfor requestInfor = new RequestInfor()
                {
                    UrlBase = Configuarations.Url_CheckExistAccount + "/" + acc.Email
                };
                var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

                if (responseData.Code == 200 && bool.Parse(responseData.Data ?? "False"))
                {
                    isValid = false;
                    ModelState.AddModelError("Email", "Tài khoản của bạn đã được đăng ký trên hệ thống");
                }

                if (responseData.Code != 200)
                {
                    isValid = false;
                    ModelState.AddModelError("", "Hệ thống đang bảo trì kỹ thuật, xin vui lòng quay lại sau!");
                }
            }

            if (isValid)
            {
                acc.Password = acc.Password != "......" ? Helpers.EncryptPassWord(acc.Password) : "";

                RequestInfor requestInfor = new RequestInfor()
                {
                    UrlBase = Configuarations.Url_CreateAccount,
                    FormValue = Helpers.ConvertObToKeyPair(acc)
                };

                var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.POST);

                if (responseData.Code != 200)
                {
                    ModelState.AddModelError("", "Hệ thống đang bảo trì kỹ thuật, xin vui lòng quay lại sau!");
                }
                else if (acc.AccountId == 0)
                {
                    var tokenInfor = await GetAccessToken(acc.Email, acc.Password);

                    if (tokenInfor.Code != 200)
                    {
                        return await CreateSessionAccount(acc.Email, "");
                    }

                    return await CreateSessionAccount(acc.Email, tokenInfor.Data);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View("_Register", acc);
        }

        public async Task<ActionResult> EditAccount()
        {
            if (SystemSession.CurrentAccount == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Account acc = await GetInforAccount(SystemSession.CurrentAccount.Email);
            acc.Password = "......";
            acc.RePassword = "......";

            return View("_Register", acc);
        }

        public async Task<ResponseData> GetAccessToken(string email, string password)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = Configuarations.Url_GetToken,
                HeaderValue = new HeaderValue()
                {
                    AuthorizationType = "Basic",
                    SecretUser = Configuarations.ClientId,
                    SecretPass = Configuarations.ClientSecrets
                },
                FormValue = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string,string>("grant_type","password"),
                    new KeyValuePair<string,string>("username",email),
                    new KeyValuePair<string,string>("password",Helpers.EncryptPassWord(password))
                }
            };

            return await ConnectAPI.GetToken(requestInfor);
        }

        public async Task<ActionResult> CreateSessionAccount(string email, string token)
        {
            Account acc = await GetInforAccount(email);
            acc.AccessToken = token;

            SystemSession.CurrentAccount = acc;

            return RedirectToAction("Index", "Home");
        }

        public async Task<Account> GetInforAccount(string email)
        {
            RequestInfor requestInfor = new RequestInfor()
            {
                UrlBase = Configuarations.Url_GetInforAccount + "/" + email
            };
            var responseData = await ConnectAPI.ConnectRestAPI(requestInfor, MethodType.GET);

            if (responseData.Code == 200)
            {
                return Helpers.Deserialize<Account>(responseData.Data);
            }

            return new Account();
        }

        public ActionResult LogOut()
        {
            SystemSession.DestroySession();

            return View("Index");
        }
    }
}