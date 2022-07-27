using System.Security.Claims;
using System.Text;
using System.Text.Json;
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

    public HomeController(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<IActionResult> Index()
    {
        GetUserInfo();

        return View();
    }

    private async Task GetUserInfo()
    {
        ViewBag.UserName = User.FindFirst("name").Value;
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> ReceiveEvents()
    {
        HttpClient httpClient = _factory.CreateClient();
        httpClient.SetBearerToken(await GetAccessToken());
        
        var result = await httpClient.GetAsync("https://localhost:7000/api/getEvents");
        var resultDeserialized = new List<Event>();

        if (result.IsSuccessStatusCode)
            resultDeserialized = JsonSerializer.Deserialize<List<Event>>(await result.Content.ReadAsStringAsync());
        
        return View("Index", ConvertToViewModel(resultDeserialized));
    }
    
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> ReceiveMyEvents()
    {
        var me = User.FindFirst("name").Value;
        
        HttpClient httpClient = _factory.CreateClient();
        httpClient.SetBearerToken(await GetAccessToken());
        
        var result = await httpClient.GetAsync($"https://localhost:7000/api/getEvents?org={me}");
        var resultDeserialized = new List<Event>();

        if (result.IsSuccessStatusCode)
            resultDeserialized = JsonSerializer.Deserialize<List<Event>>(await result.Content.ReadAsStringAsync());
        
        return View("Index", ConvertToViewModel(resultDeserialized));
    }
    
    [HttpGet]
    [Route("[action]")]
    public async Task<JsonResult> ReceiveEvent()
    {
        var queryString = HttpContext.Request.Query;

        if (queryString.Any(x => x.Key == "id"))
        {
            var queryValue = queryString.First(x => x.Key == "id").Value;
            
            HttpClient httpClient = _factory.CreateClient();
            httpClient.SetBearerToken(await GetAccessToken());
        
            var result = await httpClient.GetAsync($"https://localhost:7000/api/getEvent?id={queryValue}");
            var resultDeserialized = new Event();
        
            if (result.IsSuccessStatusCode)
                resultDeserialized = JsonSerializer.Deserialize<Event>(await result.Content.ReadAsStringAsync());

            if (!resultDeserialized.Equals(null))
                return Json(new {success = true, info = JsonSerializer.Serialize(resultDeserialized)});
        }
        
        return Json(new {success = false});
    }

    [HttpPost]
    [Authorize]
    [Route("SendEvent")]
    public async Task<JsonResult> SendEvent([FromBody] EventViewModel eventViewModel)
    {
        eventViewModel.Orgonizer = User.FindFirst("name").Value;
        
        HttpClient httpClient = _factory.CreateClient();
        httpClient.SetBearerToken(await GetAccessToken());
        
        StringContent content = new StringContent(
            JsonSerializer.Serialize(ConvertToModel(eventViewModel)),
            Encoding.UTF8,
            "application/json");

        var result = httpClient.PostAsync(
        "https://localhost:7000/api/addEvent",
                content
            );
        
        return Json(new
        {
            success = true
        });
    }
    
    [HttpPost]
    [Authorize]
    [Route("UpdateEvent")]
    public async Task<JsonResult> UpdateEvent([FromBody] EventViewModel model)
    {
        model.Orgonizer = User.FindFirst("name").Value;
    
        HttpClient httpClient = _factory.CreateClient();
        httpClient.SetBearerToken(await GetAccessToken());
    
        StringContent content = new StringContent(
            JsonSerializer.Serialize(ConvertToModel(model)),
            Encoding.UTF8,
            "application/json");

        var result = httpClient.PutAsync(
            "https://localhost:7000/api/updateEvent",
            content
        );
        
        return Json(new
        {
            success = true
        });
    }

    [Route("Login")]
    [Authorize]
    public async Task<IActionResult> Login()
    {
        GetUserInfo();
        
        return View("Index");
    }
    
    [Route("Logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        GetUserInfo();
        
        return SignOut(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            OpenIdConnectDefaults.AuthenticationScheme
            );
    }
    
    [HttpDelete]
    [Route("RequestDelete")]
    [Authorize]
    public async Task<JsonResult> RequestDelete([FromBody]int eventId)
    {
        HttpClient httpClient = _factory.CreateClient();
        httpClient.SetBearerToken(await GetAccessToken());
        
        StringContent content = new StringContent(
            JsonSerializer.Serialize(eventId),
            Encoding.UTF8,
            "application/json");

        var result = httpClient.DeleteAsync(
            $"https://localhost:7000/api/deleteEvent?eventId={eventId}");
        
        return Json(true);
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
    
    private EventsViewModel ConvertToViewModel(List<Event> result)
    {
        var viewModel = new EventsViewModel();

        foreach (Event e in result)
        {
            viewModel.events.Add(new EventViewModel()
            {
                Id = e.Id,
                MeetupName = e.MeetupName,
                Theme = e.Theme,
                Description = e.Description,
                Location = e.Location,
                Orgonizer = e.Orgonizer,
                Schedule = JsonSerializer.Deserialize<List<string>>(e.Schedule),
                Speeker = e.Speeker,
                Time = e.Time
            });
        }

        return viewModel;
    }
    
    private EventsViewModel ConvertToViewModel(Event result)
    {
        var viewModel = new EventsViewModel();

        viewModel.events.Add(new EventViewModel()
        {
            Id = result.Id,
            MeetupName = result.MeetupName,
            Theme = result.MeetupName,
            Description = result.Description,
            Location = result.Location,
            Orgonizer = result.Orgonizer,
            Schedule = JsonSerializer.Deserialize<List<string>>(result.Schedule),
            Speeker = result.Speeker,
            Time = result.Time
        });

        return viewModel;
    }

    public Event ConvertToModel(EventViewModel vm) => 
        new ()
        {
            Id = vm.Id,
            MeetupName = vm.MeetupName,
            Theme = vm.Theme,
            Description = vm.Description,
            Schedule = JsonSerializer.Serialize(vm.Schedule),
            Orgonizer = vm.Orgonizer,
            Location = vm.Location,
            Speeker = vm.Speeker,
            Time = vm.Time
        };
}