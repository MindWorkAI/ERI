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

app.MapGet("/auth/methods", () => new List<AuthMethods> { AuthMethods.NONE })
    .WithDescription("Get the available authentication methods.")
    .WithTags("Authentication");

app.MapGet("/security/requirements", () => new SecurityRequirements(ProviderType.SELF_HOSTED))
    .WithDescription("Get the security requirements for this data source.")
    .WithTags("Security");

app.Run();