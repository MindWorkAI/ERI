using Asp.Versioning.ApiExplorer;

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace DemoServer;

public sealed class DocumentInfoTransformer(IApiVersionDescriptionProvider apiDescriptionProvider) : IOpenApiDocumentTransformer
{
    private const string TOKEN_SECURITY_SCHEME = "ERI_Token";

    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var apiVersion = apiDescriptionProvider.ApiVersionDescriptions
            .Single(description => string.Equals(description.GroupName, context.DocumentName, StringComparison.OrdinalIgnoreCase))
            .ApiVersion;

        document.Info = new()
        {
            Title = $"ERI - (E)xternal (R)etrieval (I)nterface ⸺ {apiVersion}",
            Version = apiVersion.ToString(),
            Description = """
                          This API serves as a contract between LLM tools like AI Studio and any external data sources for RAG
                          (retrieval-augmented generation). The tool, e.g., AI Studio acts as the client (the augmentation and
                          generation parts) and the data sources act as the server (the retrieval part). The data
                          sources implement some form of data retrieval and return a suitable context to the LLM tool.
                          The LLM tool, in turn, handles the integration of appropriate LLMs (augmentation & generation).
                          Data sources can be document or graph databases, or even a file system, for example. They
                          will likely implement an appropriate retrieval process by using some kind of embedding.
                          However, this API does not inherently require any embedding, as data processing is
                          implemented decentralized by the data sources.

                          The client expects that all fields in the JSON responses from an ERI server are named according
                          to camel case or Pascal case conventions. The client's JSON objects for requests use camel case
                          for the field names.
                          """
        };

        // The specification describes the contract of every ERI server, not the address of this demo server:
        document.Servers = null;

        document.Components ??= new();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes[TOKEN_SECURITY_SCHEME] = new OpenApiSecurityScheme
        {
            Name = "token",
            Description = "Enter the ERI token yielded by the authentication process at /auth.",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "token",
        };

        document.Security =
        [
            new() { [new OpenApiSecuritySchemeReference(TOKEN_SECURITY_SCHEME, document)] = [] }
        ];

        return Task.CompletedTask;
    }
}
