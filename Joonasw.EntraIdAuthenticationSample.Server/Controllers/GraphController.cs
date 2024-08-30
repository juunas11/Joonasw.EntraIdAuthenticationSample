using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web.Resource;

namespace Joonasw.EntraIdAuthenticationSample.Server.Controllers;

[Authorize]
[RequiredScope("Forecasts.Read")]
[ApiController]
[Route("[controller]")]
public class GraphController : ControllerBase
{
    private readonly GraphServiceClient _graphServiceClient;

    public GraphController(
        GraphServiceClient graphServiceClient)
    {
        _graphServiceClient = graphServiceClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // Check the result with debugger
        var user = await _graphServiceClient.Me.Request().GetAsync();
        return NoContent();
    }
}