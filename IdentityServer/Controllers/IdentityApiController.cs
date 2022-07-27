using System.Text.Json;
using IdentityServer.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;

[Route("api")]
public class IdentityApiController : Controller
{
    private readonly UserManager<UserIdentity> _userManager;

    public IdentityApiController(UserManager<UserIdentity> userManager)
    {
        _userManager = userManager;
    }
    
    [HttpGet]
    [Route("getUser")]
    public async Task<string> GetUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return JsonSerializer.Serialize(user);
    }
}