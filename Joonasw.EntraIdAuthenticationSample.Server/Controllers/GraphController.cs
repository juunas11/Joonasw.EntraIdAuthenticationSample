using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web.Resource;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Joonasw.EntraIdAuthenticationSample.Server.Controllers;

[Authorize]
[RequiredScope("Forecasts.Read")]
[ApiController]
[Route("[controller]")]
public class GraphController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly GraphServiceClient _graphServiceClient;

    public GraphController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        GraphServiceClient graphServiceClient)
    {
        _httpClient = httpClientFactory.CreateClient("GraphApi");
        _configuration = configuration;
        _graphServiceClient = graphServiceClient;
    }

    [HttpGet("manualhttp")]
    public async Task<IActionResult> GetWithManualHttp()
    {
        // Check the result with debugger
        var accessToken = await GetAccessTokenAsync();

        var req = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var res = await _httpClient.SendAsync(req);

        if (res.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Failed to get user info");
        }

        var graphUserJson = await res.Content.ReadAsStringAsync();
        return Content(graphUserJson, "application/json");
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var authorizationHeaderValue = Request.Headers.Authorization.Single();
        var assertion = authorizationHeaderValue!.Substring("Bearer ".Length);
        var req = new HttpRequestMessage(HttpMethod.Post, $"https://login.microsoftonline.com/{_configuration["EntraId:TenantId"]}/oauth2/v2.0/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string?>
            {
                ["grant_type"] = "urn:ietf:params:oauth:grant-type:jwt-bearer",
                ["client_id"] = _configuration["EntraId:ClientId"],
                ["client_secret"] = _configuration["EntraId:ClientSecret"],
                ["assertion"] = assertion,
                ["scope"] = "https://graph.microsoft.com/.default", // Graph API with permissions that are defined in the app registration
                ["requested_token_use"] = "on_behalf_of"
            })
        };
        var res = await _httpClient.SendAsync(req);
        if (res.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Failed to get access token");
        }

        // You definitely want to cache this response
        // MSAL will do that for you
        var tokenResponse = await res.Content.ReadFromJsonAsync<EntraIdTokenResponse>();
        return tokenResponse!.AccessToken;
    }

    private class EntraIdTokenResponse
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public required string RefreshToken { get; set; }
        [JsonPropertyName("token_type")]
        public required string TokenType { get; set; }
        [JsonPropertyName("scope")]
        public required string Scope { get; set; }
        [JsonPropertyName("expires_in")]
        public required int ExpiresIn { get; set; }
        [JsonPropertyName("ext_expires_in")]
        public required int ExtExpiresIn { get; set; }
    }

    [HttpGet("msidweb")]
    public async Task<IActionResult> GetWithMsIdWeb()
    {
        // Check the result with debugger
        var response = await _graphServiceClient.Me.Request().GetResponseAsync();
        var graphUserJson = await response.Content.ReadAsStringAsync();
        return Content(graphUserJson, "application/json");
    }
}