using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace MvcMeetupClient.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    // Consts
    private readonly string _meetupApiUrl = "https://localhost:7000";
    private readonly string _identityServerUrl = "https://localhost:7001";

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> API_Request()
    {
        // Get access token
        var accessToken = await GetAccessToken();
        if (string.IsNullOrEmpty(accessToken)) return View("Index");

        // Get information from the api
        var client = _httpClientFactory.CreateClient();
        client.SetBearerToken(accessToken);

        var response = await client.GetAsync(_meetupApiUrl + "/MeetupAPI/GetInfo");

        ViewBag.Info = await response.Content.ReadAsStringAsync();
        return View("Index");
    }

    private async Task<string> GetAccessToken()
    {
        var authClient = _httpClientFactory.CreateClient();
        var disDoc = await authClient.GetDiscoveryDocumentAsync(_identityServerUrl);

        if (disDoc.IsError)
        {
            ViewBag.Info = "An error occured while connecting to identity server";
            return string.Empty;
        }

        var tokenResponse = await authClient.RequestClientCredentialsTokenAsync(
            CreateClientCredentialsTokenRequest(disDoc.TokenEndpoint));

        if (tokenResponse.IsError)
        {
            ViewBag.Info = "Identity server closed connection for that source!";
            return string.Empty;
        }

        return tokenResponse.AccessToken;
    }

    private ClientCredentialsTokenRequest CreateClientCredentialsTokenRequest(string tokenEndpoint)
    {
        return new()
        {
            Address = tokenEndpoint,

            ClientId = "MVCClient",
            ClientSecret = "MVCClientSecret",
            Scope = "meetupApi"
        };
    }
}