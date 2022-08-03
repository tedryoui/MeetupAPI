using System.Security.Claims;
using System.Text;
using System.Text.Json;
using AutoMapper;
using IdentityModel.Client;
using IdentityServer.DbContext;
using MeetupAPI.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcMeetupClient.ViewModels;

namespace MvcMeetupClient.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _factory;
    private readonly IMapper _mapper;

    public HomeController(IHttpClientFactory factory,
        IMapper mapper)
    {
        _factory = factory;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    [Authorize]
    [Route("[action]")]
    public async Task<IActionResult> Login() => View("Index");

    [Authorize]
    [Route("Logout")]
    public async Task<IActionResult> Logout() => SignOut(CookieAuthenticationDefaults.AuthenticationScheme,
        OpenIdConnectDefaults.AuthenticationScheme);
    
    private async Task<UserIdentity> GetUserInfo(string nameIdentifier)
    {
        HttpClient authClient = _factory.CreateClient();
        var response = await authClient.GetAsync($"https://localhost:7001/api/getUser?id={nameIdentifier}");
        var user = await response.Content.ReadFromJsonAsync<UserIdentity>();

        return user;
    }

    [HttpGet("[action]")]
    public async Task<JsonResult> RequestEvents()
    {
        var queryString = HttpContext.Request.Query;

        if (!queryString.ContainsKey("amount")) return new JsonResult(new {success = false});
        var amount = queryString["amount"].ToString();

        var page = (queryString.ContainsKey("page")) ? queryString["page"].ToString() : "0";
        
        HttpClient httpClient = _factory.CreateClient();
        httpClient.SetBearerToken(await GetAccessToken());
        
        var result = await httpClient.GetAsync($"https://localhost:7000/api/getEvents?page={page}&amount={amount}");
        var events = await result.Content.ReadFromJsonAsync<List<Event>>();
        
        if (result.IsSuccessStatusCode)
            return Json(new
            {
                success = true,
                info = _mapper.Map<List<FullEventViewModel>>(events)
            });
        
        return Json(new
            {
                success = false,
                info = new List<Event>()
            });
    }
    
    [Authorize]
    [HttpGet("[action]")]
    public async Task<JsonResult> RequestMyEvents()
    {
        var userEmail = (await GetUserInfo(User.FindFirst(ClaimTypes.NameIdentifier).Value)).Email;
        
        var queryString = HttpContext.Request.Query;
        
        if (!queryString.ContainsKey("amount")) return new JsonResult(new {success = false});
        var amount = queryString["amount"].ToString();

        var page = (queryString.ContainsKey("page")) ? queryString["page"].ToString() : "0";

        HttpClient httpClient = _factory.CreateClient();
        httpClient.SetBearerToken(await GetAccessToken());

        var result = await httpClient.GetAsync($"https://localhost:7000/api/getEvents?page={page}&amount={amount}&org={userEmail}");
        var events = await result.Content.ReadFromJsonAsync<List<Event>>();

        if (result.IsSuccessStatusCode)
            return Json(new
            {
                success = true,
                info = _mapper.Map<List<FullEventViewModel>>(events)
            });
        
        return Json(new
            {
                success = false,
                info = new List<Event>()
            });;
    }
    
    [HttpGet("[action]")]
    public async Task<JsonResult> RequestEvent()
    {
        var queryString = HttpContext.Request.Query;
        
        HttpClient httpClient = _factory.CreateClient();
        httpClient.SetBearerToken(await GetAccessToken());

        if (queryString.ContainsKey("id"))
        {
            var id = queryString["id"].ToString();
            
            var result = await httpClient.GetAsync($"https://localhost:7000/api/getEvent?id={id}");
            var ev = await result.Content.ReadFromJsonAsync<Event>();
        
            if (result.IsSuccessStatusCode)
                return Json(new {
                    success = true, 
                    info = _mapper.Map<FullEventViewModel>(ev)
                });
        }
        else if (queryString.ContainsKey("name"))
        {
            var name = queryString["name"].ToString();
            
            var result = await httpClient.GetAsync($"https://localhost:7000/api/getEvent?name={name}");
            var ev = await result.Content.ReadFromJsonAsync<Event>();
        
            if (result.IsSuccessStatusCode)
                return Json(new {
                    success = true, 
                    info = _mapper.Map<FullEventViewModel>(ev)
                });
        }
        
        return Json(new {
            success = false, 
            info = new FullEventViewModel()
        });
    }

    [Authorize]
    [HttpPost("[action]")]
    public async Task<JsonResult> RequestAdd([FromBody] FullEventViewModel eventViewModel)
    {
        Event completeEvent = _mapper.Map<Event>(eventViewModel);
        completeEvent.OrgonizerEmail = (await GetUserInfo(User.FindFirst(ClaimTypes.NameIdentifier).Value)).Email;

        HttpClient httpClient = _factory.CreateClient();
        httpClient.SetBearerToken(await GetAccessToken());

        var result = 
            httpClient.PostAsJsonAsync("https://localhost:7000/api/addEvent", completeEvent);
        
        return Json(new {success = true});
    }

    [Authorize]
    [HttpPost("[action]")]
    public async Task<JsonResult> UpdateEvent([FromBody] FullEventViewModel eventViewModel)
    {
        var completeEvent = _mapper.Map<Event>(eventViewModel);
        completeEvent.OrgonizerEmail = (await GetUserInfo(User.FindFirst(ClaimTypes.NameIdentifier).Value)).Email;
        
        HttpClient httpClient = _factory.CreateClient();
        httpClient.SetBearerToken(await GetAccessToken());

        var result = httpClient.PutAsJsonAsync(
            "https://localhost:7000/api/updateEvent",
            completeEvent
        );
        
        return Json(new {success = true});
    }
    
    [Authorize]
    [HttpDelete("[action]")]
    public async Task<JsonResult> RequestDelete()
    {
        var queryString = HttpContext.Request.Query;
        if (queryString.ContainsKey("id"))
        {
            var id = queryString["id"].ToString();
            
            HttpClient httpClient = _factory.CreateClient();
            httpClient.SetBearerToken(await GetAccessToken());

            var result = httpClient.DeleteAsync(
                $"https://localhost:7000/api/deleteEvent?id={id}");
        
            return Json(new {success = true});
        }
        
        return Json(new {success = false});
    }

    public async Task<string> GetAccessToken()
    {
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7001");
        if (disco.IsError)
            return "";
        
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,

            ClientId = "client",
            ClientSecret = "client_secret",
            Scope = "api"
        });

        if (tokenResponse.IsError)
            return "";

        return tokenResponse.AccessToken;
    }
}