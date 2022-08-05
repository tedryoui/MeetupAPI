using IdentityServer;
using IdentityServer.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});

builder.Services.AddIdentity<UserIdentity, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 4;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDbContext>();
    
builder.Services.AddIdentityServer()
    .AddAspNetIdentity<UserIdentity>()
    .AddInMemoryClients(IS4Config.Clients)
    .AddInMemoryApiScopes(IS4Config.ApiScopes)
    .AddInMemoryIdentityResources(IS4Config.IdentityResources)
    .AddDeveloperSigningCredential();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    AppDbContext.InitializeWithAdmin(scope.ServiceProvider);
}

app.UseStaticFiles();

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityServer();

app.MapControllers();

app.Run();