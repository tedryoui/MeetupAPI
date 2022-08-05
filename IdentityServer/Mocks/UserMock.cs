using IdentityServer.DbContext;

namespace IdentityServer.Mocks;

public static class UserMock
{
    public static UserIdentity Admin =>
        new UserIdentity()
        {
            UserName = "admin",
            Email = "admin@admin.com"
        };
}