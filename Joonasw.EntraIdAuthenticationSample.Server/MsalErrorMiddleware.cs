using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Net;

namespace Joonasw.EntraIdAuthenticationSample.Server;

public class MsalErrorMiddleware
{
    private readonly RequestDelegate _next;

    public MsalErrorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenAcquisition tokenAcquisition)
    {
        try
        {
            await _next(context);
        }
        catch (MsalException e)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("An authentication error occurred while acquiring a token for downstream API\n" + e.ErrorCode + "\n" + e.Message);
        }
        catch (Exception e)
        {
            if (e.InnerException is MicrosoftIdentityWebChallengeUserException challengeException)
            {
                await tokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeaderAsync(["User.Read"], challengeException.MsalUiRequiredException);
            }
        }
    }
}
