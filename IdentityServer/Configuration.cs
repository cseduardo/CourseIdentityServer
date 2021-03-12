using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> 
            { 
                new ApiResource("ApiOne"){ Scopes={"ApiOne"} },
            };

        public static IEnumerable<ApiScope> GetScopes() =>
            new List<ApiScope>

            {
                new ApiScope("ApiOne"),
                new ApiScope("ApiTwo"),
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
            }
        };
    }
}
