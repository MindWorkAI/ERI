using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace DemoServer;

public sealed class TokenSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var requirements = new Dictionary<string, OpenApiSecurityScheme>
        {
            ["Token"] = new()
            {
                Type = SecuritySchemeType.ApiKey,
                Scheme = "token", // Name of the header field
                In = ParameterLocation.Header,
                BearerFormat = "GUID",
            }
        };
        
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = requirements;
        return Task.CompletedTask;
    }
}