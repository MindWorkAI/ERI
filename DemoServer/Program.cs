using DemoServer.Handler;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(Json.Configure);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAPIVersioning();
builder.Services.ConfigureOptions<ConfigureSwaggerGen>();

var app = builder.Build();
app.AddServerHandlers();
app.AddAuthHandlers();
app.AddSecurityHandlers();
app.AddEmbeddingHandlers();
app.AddRetrievalHandlers();
app.AddDataSourceHandlers();

app.AddAuthFilter();
app.MapSwagger("/specification/{documentName}.json");
app.UseSwaggerUI(options => options.Configure(app.DescribeApiVersions()));
app.Run();