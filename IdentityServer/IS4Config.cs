using IdentityServer4.Models;

namespace IdentityServer;

public static class IS4Config
{
    public static IEnumerable<ApiScope> Scopes =>
        new List<ApiScope>
        {
            new("meetupApi", "Meetup API")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new()
            {
                ClientId = "MVCClient",

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                ClientSecrets =
                {
                    new Secret("MVCClientSecret".Sha256())
                },

                AllowedScopes = {"meetupApi"}
            }
        };
}