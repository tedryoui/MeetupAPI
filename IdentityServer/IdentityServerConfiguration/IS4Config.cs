using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer;

public static class IS4Config
{
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client()
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("client_secret".Sha256())},
                AllowedScopes =
                {
                    "api"
                }
            },
            new Client()
            {
                ClientId = "client_mvc",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = {new Secret("client_mvc_secret".Sha256())},
                AllowedScopes =
                {
                    "api",
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email
                },
                RedirectUris =
                {
                    "https://localhost:7003/signin-oidc"
                },
                PostLogoutRedirectUris =
                {
                    "https://localhost:7003/signout-callback-oidc"  
                },
                RequireConsent = false
            },
            new Client()
            {
                ClientId = "client_swagger",
                ClientSecrets = { new Secret("client_swagger_secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedCorsOrigins = { "https://localhost:7000" },
                AllowedScopes =
                {
                    "api",
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OpenId
                }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>()
        {
            new ApiScope() {Name = "api"}
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };
}