using Asp.Versioning.ApiExplorer;

using Swashbuckle.AspNetCore.SwaggerUI;

namespace DemoServer;

public static class ConfigureSwaggerUI
{
    public static void Configure(this SwaggerUIOptions options, IReadOnlyCollection<ApiVersionDescription> apiVersions)
    {
        foreach (var apiVersion in apiVersions)
        {
            var url = $"/specification/v{apiVersion.ApiVersion}.json";
            options.SwaggerEndpoint(url, apiVersion.GroupName);
        }
        
        options.RoutePrefix = "specification";
    }
}