using System.Collections.Generic;
using System.Web;
using Framework.Entities.Personals;
using Framework.Entities.Systems;

namespace Framework.Security
{
    public class SystemSession
    {
        private static string UniqueKey = "DVG";

        #region Constructor create session
        public static void SetSession(string name, object value)
        {
            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session[UniqueKey + name] = value;
        }

        public static object GetSession(string name)
        {
            if (HttpContext.Current.Session != null)
                return HttpContext.Current.Session[UniqueKey + name];
            return null;
        }
        #endregion

        #region destroy session
        public static void DestroySession()
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.Clear();
            }
        }
        #endregion

        #region CurrentAccount
        public static Account CurrentAccount
        {
            set => SetSession("CurrentAccount", value);
            get
            {
                if (GetSession("CurrentAccount") == null)
                    return null;
                return (Account)GetSession("CurrentAccount");
            }
        }
        #endregion CurrentUser

        #region ShopCart
        public static List<Cart> ShopCart
        {
            set => SetSession("ShopCart", value);
            get
            {
                if (GetSession("ShopCart") == null)
                    return null;
                return (List<Cart>)GetSession("ShopCart");
            }
        }
        #endregion ShopCart
    }
}