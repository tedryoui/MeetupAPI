var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();

app.MapDefaultControllerRoute();

app.Run();