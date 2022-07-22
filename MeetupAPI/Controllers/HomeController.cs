using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetupAPI;

[Authorize]
[Route("MeetupAPI")]
public class HomeController : ControllerBase
{
    [HttpGet]
    [Route("[action]")]
    public string GetInfo()
    {
        return "information from api!";
    }
}