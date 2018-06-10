using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebAppMVC.Models.Systems
{
    public static class Configuarations
    {
        // Information client
        public static string ClientId = ConfigurationManager.AppSettings["ClientId"];
        public static string ClientSecrets = ConfigurationManager.AppSettings["ClientSecrets"];

        // Url Base
        public static string Url_Base_Auth = ConfigurationManager.AppSettings["Url_Base_Auth"];
        public static string Url_Base_APIGateway = ConfigurationManager.AppSettings["Url_Base_APIGateway"];

        // Information account
        public static string Url_GetToken = Url_Base_Auth + ConfigurationManager.AppSettings["Url_GetToken"];
        public static string Url_CheckExistAccount = Url_Base_Auth + ConfigurationManager.AppSettings["Url_CheckExistAccount"];
        public static string Url_CreateAccount = Url_Base_Auth + ConfigurationManager.AppSettings["Url_CreateAccount"];
        public static string Url_GetInforAccount = Url_Base_Auth + ConfigurationManager.AppSettings["Url_GetInforAccount"];

        // Organization
        public static string Url_GetInforOrganization = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_GetInforOrganization"];
        public static string Url_GetInforOrganizationFromAcc = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_GetInforOrganizationFromAcc"];
        public static string Url_AddOrganization = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_AddOrganization"];

        // Product
        public static string Url_Product_GetInfor = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_Product_GetInfor"];
        public static string Url_Product_GetList = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_Product_GetList"];
        public static string Url_Product_GetListByConditional = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_Product_GetListByConditional"];
        public static string Url_Product_Add = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_Product_Add"];
        public static string Url_Product_Delete = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_Product_Delete"];
        public static string Url_Product_GetListProductCategory = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_Product_GetListProductCategory"];
        public static string Url_Product_GetListPricePG = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_Product_GetListPricePG"];
        public static string Url_Product_UpdatePrice = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_Product_UpdatePrice"];

        public static string Url_Shop_GetListProductPrice = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_Shop_GetListProductPrice"];
        public static string Url_Shop_Payment = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_Shop_Payment"];
        public static string Url_Shop_ViewHistory = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_Shop_ViewHistory"];

        public static string Url_MyOrder_GetListProduct = Url_Base_APIGateway + ConfigurationManager.AppSettings["Url_MyOrder_GetListProduct"];
    }
}