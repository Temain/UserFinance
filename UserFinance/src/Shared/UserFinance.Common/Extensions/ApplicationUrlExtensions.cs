using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace UserFinance.Common.Extensions;

public static class ApplicationUrlExtensions
{
    public static void ConfigureLocalHttpUrl(this WebApplicationBuilder builder, string portEnvironmentVariable)
    {
        if (!string.IsNullOrWhiteSpace(builder.Configuration["ASPNETCORE_URLS"]))
        {
            return;
        }

        var portValue = builder.Configuration[portEnvironmentVariable];

        if (!int.TryParse(portValue, out var port) || port <= 0)
        {
            return;
        }

        builder.WebHost.UseUrls($"http://localhost:{port}");
    }
}
