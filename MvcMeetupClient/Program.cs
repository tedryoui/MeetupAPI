using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using MvcMeetupClient.ViewModels.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.Authority = builder.Configuration.GetSection("ServiceUris")["is4"];
        
        options.ClientId = "client_mvc";
        options.ClientSecret = "client_mvc_secret";
        options.ResponseType = "code";

        options.SaveTokens = true;

        options.GetClaimsFromUserInfoEndpoint = true;
        
        options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
        options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
        options.Scope.Add(IdentityServerConstants.StandardScopes.Email);
    });

var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();