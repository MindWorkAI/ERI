using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace DemoServer;

public static class OpenApiDocuments
{
    extension(IServiceCollection services)
    {
        public void AddOpenApiDocuments()
        {
            //
            // One document per API version, named like the group names of the API explorer.
            // The names must stay literals: Only then does the source generator of
            // Microsoft.AspNetCore.OpenApi add the XML documentation to the documents.
            //
            services.AddOpenApi("v1.0-production", Configure);
            services.AddOpenApi("v1.1-design", Configure);
            services.AddOpenApi("v2.0-design", Configure);
        }
    }

    extension(WebApplication app)
    {
        public void MapOpenApiDocuments()
        {
            // Fail at the start when an API version lacks its document:
            foreach (var apiVersion in app.DescribeApiVersions())
                if (app.Services.GetKeyedService<IOpenApiDocumentProvider>(apiVersion.GroupName) is null)
                    throw new InvalidOperationException($"The API version '{apiVersion.ApiVersion}' has no OpenAPI document named '{apiVersion.GroupName}'. Add it to {nameof(AddOpenApiDocuments)}.");

            app.MapOpenApi("/specification/{documentName}.json");
        }
    }

    private static void Configure(OpenApiOptions options)
    {
        options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
        options.AddDocumentTransformer<DocumentInfoTransformer>();
        options.AddOperationTransformer<VersionHeaderTransformer>();
        options.AddSchemaTransformer<EnumSchemaTransformer>();
        options.AddSchemaTransformer<ContractCompatibilitySchemaTransformer>();
    }
}
