using DemoServer.Handler;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(Json.Configure);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(OpenAPIDoc.Configure);

var app = builder.Build();
app.MapSwagger();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "v1"));
app.AddAuthHandlers();
app.AddSecurityHandlers();
app.AddEmbeddingHandlers();
app.AddRetrievalHandlers();
app.AddDataSourceHandlers();
app.Run();