using IdentityServer4.Models;
using System.Collections.Generic;
using Framework.Common.Facache;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("auth_api", "Microservice")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // Hybrid Flow = OpenId Connect + OAuth
                // To use both Identity and Access Tokens
                // Resource Owner Password Flow
                new Client
                {
                    ClientId = "fiver_auth_client_ro",
                    ClientName = "Fiver.Security.AuthServer.Client.RO",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    //Config time out
                    AccessTokenLifetime = StaticConfig.TimeOutToken,
                    IdentityTokenLifetime = StaticConfig.TimeOutIdentity,


                    AllowedScopes =
                    {
                        "auth_api"
                    },
                },
                new Client
                {
                    ClientId = "WebAppMVC_Client",
                    ClientName = "WebAppMVC_Client",
                    ClientSecrets = { new Secret("WebAppMVC".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    //Config time out
                    AccessTokenLifetime = StaticConfig.TimeOutToken,
                    IdentityTokenLifetime = StaticConfig.TimeOutIdentity,

                    AllowedScopes =
                    {
                        "auth_api"
                    },
                }
            };
        }
    }
}
