using System.Security.Claims;
using IdentityServer.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.DbContext;

public class AppDbContext : IdentityDbContext<UserIdentity>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public static void InitializeWithAdmin(IServiceProvider provider)
    {
        var userManager = provider.GetService<UserManager<UserIdentity>>();
        var admin = userManager.FindByNameAsync("admin").GetAwaiter().GetResult();

        if (admin == null)
        {
            userManager.CreateAsync(UserMock.Admin, "admin").GetAwaiter().GetResult();
            userManager.AddClaimAsync(UserMock.Admin,
                new Claim(ClaimTypes.Role, "admin")).GetAwaiter().GetResult();
        }
        else
        {
            var claims = userManager.GetClaimsAsync(admin).GetAwaiter().GetResult();

            if (!claims.Any(x => x.Type == ClaimTypes.Role &&
                                 x.Value == "admin"))
                userManager.AddClaimAsync(userManager.FindByNameAsync("admin").GetAwaiter().GetResult(),
                    new Claim(ClaimTypes.Role, "admin")).GetAwaiter().GetResult();
        }

    }
}