using System.Text.Json.Nodes;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace DemoServer;

public sealed class VersionHeaderTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        // Read the API version of this operation; version-neutral operations need no version header:
        if (context.Description.ApiVersion is not { } apiVersion)
            return Task.CompletedTask;

        var version = apiVersion.ToString();
        var versionParameter = new OpenApiParameter
        {
            Name = Versions.VERSION_HEADER_FIELD_NAME,
            In = ParameterLocation.Header,
            Required = true,
            Schema = new OpenApiSchema { Type = JsonSchemaType.String },
            Description = $"The API version to use. This must be set to '{version}'.",
            Example = JsonValue.Create(version)
        };

        operation.Parameters ??= [];

        // When the version header already exists, remove it first:
        if (operation.Parameters.FirstOrDefault(p => p is { Name: Versions.VERSION_HEADER_FIELD_NAME, In: ParameterLocation.Header }) is { } existingParameter)
            operation.Parameters.Remove(existingParameter);

        // Add the version header parameter:
        operation.Parameters.Add(versionParameter);
        return Task.CompletedTask;
    }
}
