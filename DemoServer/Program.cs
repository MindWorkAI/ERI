using System.Text.Json.Serialization;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "EDI - (E)xternal (D)ata AP(I) for AI Studio",
            Version = "v1",
            Description = """
                          This API serves as a contract between AI Studio and any external data sources. AI Studio
                          acts as the client and the data sources act as the server. The data sources implement some
                          form of data retrieval and return a suitable context to AI Studio. AI Studio, in turn,
                          handles the integration of appropriate LLMs. Data sources can be document or graph databases
                          or file systems, for example. They will likely implement an appropriate RAG process. However,
                          this API does not inherently require RAG, as data processing is implemented decentralized by
                          the data sources.
                          """
        };
        return Task.CompletedTask;
    });
});
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "EDI - (E)xternal (D)ata AP(I) for AI Studio";
    options.HiddenClients = true;
});

app.MapGet("/auth/methods", () => new List<AuthMethods> { AuthMethods.NONE })
    .WithDescription("Get the available authentication methods.")
    .WithName("GetAuthMethods")
    .WithTags("Authentication");

app.MapGet("/security/requirements", () => new SecurityRequirements(ProviderType.SELF_HOSTED))
    .WithDescription("Get the security requirements for this data source.")
    .WithName("GetSecurityRequirements")
    .WithTags("Security");

app.Run();