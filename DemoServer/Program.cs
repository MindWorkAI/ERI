using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddOpenApi();

var app = builder.Build();
app.MapOpenApi();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/openapi";
    settings.DocumentPath = "/openapi/v1.json";
    settings.DocumentTitle = "(E)xternal (D)ata AP(I) for AI Studio";
});

// This is a simple demonstration for a data source. In fact, you would want to
// connect a graph database or a document database or a server's file system instead:
var exampleData = new[]
{
    new WikipediaArticle("Strategic foresight", "https://en.wikipedia.org/wiki/Strategic_foresight"),
    new WikipediaArticle("Scenario planning", "https://en.wikipedia.org/wiki/Scenario_planning"),
    new WikipediaArticle("Futures studies", "https://en.wikipedia.org/wiki/Futures_studies"),
    new WikipediaArticle("Futures techniques", "https://en.wikipedia.org/wiki/Futures_techniques"),
    new WikipediaArticle("Delphi method", "https://en.wikipedia.org/wiki/Delphi_method"),       
};

app.MapGet("/auth/methods", () => new List<AuthMethods> { AuthMethods.NONE })
    .WithDescription("Get the available authentication methods.")
    .WithTags("Authentication");

app.MapGet("/security/requirements", () => new SecurityRequirements(ProviderType.SELF_HOSTED))
    .WithDescription("Get the security requirements for this data source.")
    .WithTags("Security");

app.Run();