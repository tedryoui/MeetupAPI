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
    
    [HttpGet("getUser")]
    public async Task<JsonResult> GetUser()
    {
        var query = HttpContext.Request.Query;

        if (query.ContainsKey("id"))
        {
            var id = query["id"].ToString();
            
            var user = await _userManager.FindByIdAsync(id);

            HttpContext.Response.StatusCode = 200;
            return new JsonResult(user);
        }
        
        HttpContext.Response.StatusCode = 404;
        return new JsonResult(null);
    }
}