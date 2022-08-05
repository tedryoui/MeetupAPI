using System.Security.Claims;
using System.Text.Json;
using IdentityServer.DbContext;
using IdentityServer.ViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;


[Route("[controller]")]
public class AccountController : Controller
{
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly SignInManager<UserIdentity> _signInManager;
    private readonly IConfiguration _configuration;

    public AccountController( IIdentityServerInteractionService interactionService,
                            UserManager<UserIdentity> userManager,
                            SignInManager<UserIdentity> signInManager,
                            IConfiguration configuration)
    {
        _interactionService = interactionService;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }
    
    [HttpGet("[action]")]
    public IActionResult Login(string returnUrl) => View("Login");

    [HttpPost("[action]")]
    public async Task<JsonResult> Login([FromBody] LoginViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(viewModel.Email);

            var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.IsRemember, false);

            if (result.Succeeded)
            {
                return Json(new {success = true});
            }
        }
        
        return Json(new {success = false});
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> Logout(string logoutId)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _signInManager.SignOutAsync();
            var logoutResult = await _interactionService.GetLogoutContextAsync(logoutId);
            if (!string.IsNullOrEmpty(logoutResult.PostLogoutRedirectUri))
                return Redirect(logoutResult.PostLogoutRedirectUri);
        }
        
        return Redirect(_configuration.GetSection("ServiceUris")["mvc"]);
    }

    [HttpPost("[action]")]
    public async Task<JsonResult> Register([FromBody] RegisterViewModel viewModel)
    {
            var user = new UserIdentity()
            {
                UserName = viewModel.Username,
                Email = viewModel.Email
            };

            await _userManager.CreateAsync(user, viewModel.Password);

            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "user"));

            var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.IsRemember, false);

            return Json(new {success = true});
    }
}