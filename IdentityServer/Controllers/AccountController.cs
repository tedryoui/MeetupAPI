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

    public AccountController( IIdentityServerInteractionService interactionService,
                            UserManager<UserIdentity> userManager,
                            SignInManager<UserIdentity> signInManager)
    {
        _interactionService = interactionService;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    [Route("[action]")]
    public IActionResult Login(string returnUrl)
    {
        var viewModel = new LoginViewModel()
        {
            ReturnUrl = returnUrl
        };
        
        return View("Login", viewModel);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(viewModel.Username);

            var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, false, false);

            if (result.Succeeded)
            {
                return Redirect(viewModel.ReturnUrl);
            }
        }
        
        return View("Login", viewModel);
    }
    
    [Route("[action]")]
    public async Task<IActionResult> Logout(string logoutId)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _signInManager.SignOutAsync();
            var logoutResult = await _interactionService.GetLogoutContextAsync(logoutId);
            if (!string.IsNullOrEmpty(logoutResult.PostLogoutRedirectUri))
                return Redirect(logoutResult.PostLogoutRedirectUri);
        }
        
        return Redirect("httsp://localhost:7003/");
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Register(LoginViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = new UserIdentity()
            {
                UserName = viewModel.Username,
                Email = $"{viewModel.Username}@somemail.com"
            };

            await _userManager.CreateAsync(user, viewModel.Password);

            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "user"));

            var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, false, false);

            return Redirect(viewModel.ReturnUrl);
        }
        
        return View("Login", viewModel);
    }
}