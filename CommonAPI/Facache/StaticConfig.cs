using System;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Configuration;

namespace Framework.Common.Facache
{
    public static class StaticConfig
    {
        // Config System
        public static int TimeOutToken = 1800;
        public static int TimeOutIdentity = 1800;

        // ServiceName
        public static string MsvAuth = "Auth";         //5000
        public static string MsvCatalog = "Catalog";   //5001
        public static string MsvPrice = "Price";       //5002
        public static string MsvPayment = "Payment";   //5003

        //----- Url Microservice
        // Catalog Service
        // Organization
        public static string UrlCatalogGetInforOrganization => GetUriFromConsul(MsvCatalog).Result + "/api/MSV_Organization/GetInforOrganization";

        public static string UrlCatalogGetInforOrganizationFromAcc => GetUriFromConsul(MsvCatalog).Result + "/api/MSV_Organization/GetInforOrganizationFromAcc";

        public static string UrlCatalogAddOrganization => GetUriFromConsul(MsvCatalog).Result + "/api/MSV_Organization/AddOrganization";

        // Product
        public static string UrlCatalogGetInforProduct => GetUriFromConsul(MsvCatalog).Result + "/api/MSV_Product/GetInforProduct";

        public static string UrlCatalogGetListProduct => GetUriFromConsul(MsvCatalog).Result + "/api/MSV_Product/GetListProduct";

        public static string UrlCatalogGetListProductPg => GetUriFromConsul(MsvCatalog).Result + "/api/MSV_Product/GetListProductPG";

        public static string UrlCatalogAddProduct => GetUriFromConsul(MsvCatalog).Result + "/api/MSV_Product/AddProduct";

        public static string UrlCatalogDeleteProduct => GetUriFromConsul(MsvCatalog).Result + "/api/MSV_Product/DeleteProduct";

        public static string UrlCatalogGetListProductCategory => GetUriFromConsul(MsvCatalog).Result + "/api/MSV_ProductCategory/GetListProductCategory";

        public static string UrlCatalogShopGetListProduct => GetUriFromConsul(MsvCatalog).Result + "/api/MSV_Product/Shop_GetListProduct";

        // Price Service
        public static string UrlPriceGetPriceFromProduct => GetUriFromConsul(MsvPrice).Result + "/api/MSV_Price/GetPriceFromProduct";

        public static string UrlPriceGetListPricePg => GetUriFromConsul(MsvPrice).Result + "/api/MSV_Price/GetListPricePG";

        public static string UrlPriceUpdatePrice => GetUriFromConsul(MsvPrice).Result + "/api/MSV_Price/AddPrice";

        public static string UrlPriceShopGetListProductPrice => GetUriFromConsul(MsvPrice).Result + "/api/MSV_Price/Shop_GetListProductPrice";

        // Payment Service
        public static string UrlPaymentPayment => GetUriFromConsul(MsvPayment).Result + "/api/MSV_Payment/Payment";

        public static string UrlPaymentViewHistory => GetUriFromConsul(MsvPayment).Result + "/api/MSV_Payment/ViewHistory";

        public static string UrlPaymentMyOrderGetListProduct => GetUriFromConsul(MsvPayment).Result + "/api/MSV_Payment/MyOrder_GetListProduct";

        // MyOrder
        public static string UrlMyOrderGetListInforAccount => GetUriFromConsul(MsvAuth).Result + "/api/Register/GetListInforAccount";

        public static IConfigurationRoot Configuration { get; set; }

        public static async Task<string> GetUriFromConsul(string serviceName)
        {
            var consulClient = new ConsulClient(c =>
            {
                var uri = new Uri("http://127.0.0.1:8500");
                c.Address = uri;
            });

            string address = string.Empty;
            try
            {
                var services = await consulClient.Agent.Services();
                foreach (var item in services.Response)
                {
                    if (item.Value.Service.Contains(serviceName))
                    {
                        return $"{item.Value.Address}:{item.Value.Port}";
                    }
                }

                return address;
            }
            catch (Exception)
            {
                return address;
            }
        }
    }
}
