using DemoServer.Handler;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(Json.Configure);
builder.Services.AddAPIVersioning();
builder.Services.AddOpenApiDocuments();

var app = builder.Build();
app.AddServerHandlers();
app.AddAuthHandlers();
app.AddSecurityHandlers();
app.AddEmbeddingHandlers();
app.AddRetrievalV1Handlers();
app.AddRetrievalV2Handlers();
app.AddToolHandlers();
app.AddDataSourceHandlers();

app.AddAuthFilter();
app.MapOpenApiDocuments();
app.UseSwaggerUI(options => options.Configure(app.DescribeApiVersions()));
app.Run();