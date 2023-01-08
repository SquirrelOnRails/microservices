using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mango.Services.IdentityServer
{
    public static class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>() 
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("mango", "Admin scope"),
                new ApiScope("read", "Scope to read the data"),
                new ApiScope("write", "Scope to write the data"),
                new ApiScope("delete", "Scope to delete the data")
            };

        public static IEnumerable<Client> Clients => 
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "read", "write", "profile" }
                },
                new Client
                {
                    ClientId = "mango",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = new List<string> 
                    {
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "mango"
                    },
                    RedirectUris = { "https://localhost:7135/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:7135/signout-callback-oidc" }
                }
            };
    }
}
