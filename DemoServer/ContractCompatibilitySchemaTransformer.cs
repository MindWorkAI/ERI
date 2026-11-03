using System.Text.Json.Serialization.Metadata;

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace DemoServer;

/// <summary>
/// Keeps the meaning the schemas of the ERI specification had before ASP.NET Core generated them. ASP.NET Core
/// follows the nullable annotations and the JSON options of this server, which would change the contract.
/// </summary>
public sealed class ContractCompatibilitySchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        // The server reads numbers from strings, too, but the contract promises numbers only:
        if (schema.Type is { } type && type.HasFlag(JsonSchemaType.String) && (type & (JsonSchemaType.Integer | JsonSchemaType.Number)) != 0)
        {
            schema.Type = type & ~JsonSchemaType.String;
            schema.Pattern = null;
        }

        // Objects allow no further fields, and none of their fields is required:
        if (context.JsonTypeInfo.Kind is JsonTypeInfoKind.Object && schema.Properties is { Count: > 0 })
        {
            schema.AdditionalPropertiesAllowed = false;
            schema.Required = null;
        }

        // Fields of a reference type may be null; fields holding an object stay plain references:
        if (context is { JsonPropertyInfo.PropertyType.IsValueType: false, JsonTypeInfo.Kind: not JsonTypeInfoKind.Object })
            schema.Type |= JsonSchemaType.Null;

        return Task.CompletedTask;
    }
}
