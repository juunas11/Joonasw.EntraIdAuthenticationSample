using Microsoft.Identity.Web;

namespace Joonasw.EntraIdAuthenticationSample.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddCors(o =>
        {
            o.AddPolicy("default", cors =>
            {
                cors
                    .WithOrigins("https://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration)
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(builder.Configuration.GetSection("MsGraph"))
            .AddInMemoryTokenCaches();

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseCors("default");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<MsalErrorMiddleware>();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
