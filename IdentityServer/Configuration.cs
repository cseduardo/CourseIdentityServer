using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("ApiOne"){ Scopes={"ApiOne"} },
                new ApiResource("ApiTwo"){ Scopes={"ApiTwo"} },
            };

        public static IEnumerable<ApiScope> GetScopes() =>
            new List<ApiScope>

            {
                new ApiScope("ApiOne"),
                new ApiScope("ApiTwo"),
                //new ApiScope(IdentityServerConstants.StandardScopes.OpenId),
                //new ApiScope(IdentityServerConstants.StandardScopes.Profile),
            };

        public static IEnumerable<Client> GetClients() => new List<Client>
        {
            new Client
            {
                ClientId="client_id",
                ClientSecrets=
                {
                    new Secret("client_secret".ToSha256())
                },

                AllowedGrantTypes=GrantTypes.ClientCredentials,

                AllowedScopes={"ApiOne" }
            },
            new Client
            {
                ClientId="client_id_mvc",
                ClientSecrets=
                {
                    new Secret("client_secret_mvc".ToSha256())
                },

                RedirectUris={ "https://localhost:44344/signin-oidc" },

                AllowedGrantTypes=GrantTypes.Code,

                AllowedScopes=
                {
                    "ApiOne", 
                    "ApiTwo",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                },
                RequireConsent=default(bool)
            }
        };
    }
}
