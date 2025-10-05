using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

// ReSharper disable ClassNeverInstantiated.Global

namespace DemoServer;

public class AddGlobalVersionHeader : IDocumentFilter
{
    #region Implementation of IDocumentFilter

    public void Apply(OpenApiDocument apiDocument, DocumentFilterContext context)
    {
        // Read the selected API version:
        var version = apiDocument.Info.Version;
        
        // Iterate through all paths and operations in the OpenAPI document:
        foreach (var path in apiDocument.Paths.Values)
        foreach (var operation in path.Operations)
        {
            // Add the version header parameter to each operation:
            var versionParameter = new OpenApiParameter
            {
                Name = Versions.VERSION_HEADER_FIELD_NAME,
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema { Type = "string" },
                Description = $"The API version to use. This must be set to '{version}'.",
                Example = new OpenApiString(version)
            };

            // When the version header already exists, remove it first:
            if (operation.Value.Parameters.FirstOrDefault(p => p is { Name: Versions.VERSION_HEADER_FIELD_NAME, In: ParameterLocation.Header }) is { } existingParameter)
                operation.Value.Parameters.Remove(existingParameter);

            // Add the version header parameter:
            operation.Value.Parameters.Add(versionParameter);
        }
    }

    #endregion
}