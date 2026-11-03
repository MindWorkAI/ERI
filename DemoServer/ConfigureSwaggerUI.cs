using Asp.Versioning.ApiExplorer;

using Swashbuckle.AspNetCore.SwaggerUI;

namespace DemoServer;

public static class ConfigureSwaggerUI
{
    extension(SwaggerUIOptions options)
    {
        public void Configure(IReadOnlyCollection<ApiVersionDescription> apiVersions)
        {
            foreach (var apiVersion in apiVersions)
            {
                var url = $"/specification/v{apiVersion.ApiVersion}.json";
                options.SwaggerEndpoint(url, apiVersion.GroupName);
            }
            
            options.RoutePrefix = "specification";
        }
    }
}