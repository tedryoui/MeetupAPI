using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
        
        if (User.Identity.IsAuthenticated)
        {
            Console.WriteLine($"\n\n\n{User.Identity.Name}\n\n\n");
        }
    }

    [Authorize]
    public IActionResult SignIn()
    {
        return View("Index");
    }

    [Authorize]
    public IActionResult GetInfo()
    {
        return View("Index");
    }

    [Authorize]
    [Route("[action]")]
    public IActionResult Logout()
    {
        return new JsonResult(Json(new {Some = "HAHA"}));
    }
}