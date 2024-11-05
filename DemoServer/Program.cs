using System.Reflection;
using System.Text.Json.Serialization;

using DemoServer.DataModel;

using Microsoft.OpenApi.Models;

var validTokens = new HashSet<string>();

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    c.SchemaFilter<EnumSchemaFilter>();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EDI - (E)xternal (D)ata AP(I) for AI Studio",
        Version = "v1",
        Description = """
                      This API serves as a contract between AI Studio and any external data sources for RAG
                      (retrieval-augmented generation). AI Studio acts as the client (the augmentation and
                      generation parts) and the data sources act as the server (the retrieval part). The data
                      sources implement some form of data retrieval and return a suitable context to AI Studio.
                      AI Studio, in turn, handles the integration of appropriate LLMs (augmentation & generation).
                      Data sources can be document or graph databases, or even a file system, for example. They
                      will likely implement an appropriate retrieval process by using some kind of embedding.
                      However, this API does not inherently require any embedding, as data processing is
                      implemented decentralized by the data sources.
                      """
    });
    
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "token",
        Description = "Enter the EDI token yielded by the authentication process at /auth.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "token",
        Reference = new OpenApiReference
        {
            Id = "EDI_Token",
            Type = ReferenceType.SecurityScheme
        }
    };
    
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, [] }
    });
});

var app = builder.Build();
app.MapSwagger();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "v1");
});

app.Use(async (context, next) =>
{
    if(context.Request.Path.StartsWithSegments("/swagger"))
    {
        await next(context);
        return;
    }
    
    if(context.Request.Path.StartsWithSegments("/auth"))
    {
        await next(context);
        return;
    }
    
    var tokens = context.Request.Headers["token"];
    if (tokens.Count == 0)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new { error = "No token provided." });
        return;
    }
    
    var token = tokens[0];
    if (string.IsNullOrWhiteSpace(token))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new { error = "Empty token provided." });
        return;
    }
    
    if (!validTokens.Contains(token))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new { error = "Invalid token provided." });
        return;
    }
    
    // Call the next delegate/middleware in the pipeline.
    await next(context);
});

#region Implementing the EDI

#region Data Source

app.MapGet("/dataSource", () => new DataSourceInfo("DEMO: Wikipedia Links", "Contains some links to Wikipedia articles."))
    .WithDescription("Get information about the data source.")
    .WithName("GetDataSourceInfo")
    .WithTags("Data Source");

#endregion

#region Security

app.MapGet("/security/requirements", () => new SecurityRequirements(ProviderType.SELF_HOSTED))
    .WithDescription("Get the security requirements for this data source.")
    .WithName("GetSecurityRequirements")
    .WithTags("Security");

#endregion

#region Authentication

app.MapGet("/auth/methods", () => new List<AuthScheme>
{
    new()
    {
        AuthMethod = AuthMethod.NONE,
        AuthFieldMappings = new(),
    },

    new()
    {
        AuthMethod = AuthMethod.USERNAME_PASSWORD,
        AuthFieldMappings =
        [
            new(AuthField.USERNAME, "user"),
            new(AuthField.PASSWORD, "password"),
        ],
    },
})
    .WithDescription("Get the available authentication methods.")
    .WithName("GetAuthMethods")
    .WithTags("Authentication");

app.MapPost("/auth", (HttpContext context, AuthMethod authMethod) =>
{
    //
    // Authenticate with the data source to get a token for further requests.
    // 
    // Please note that the authentication is a two-step process. (1) The
    // client authenticates with the server by using the chosen authentication
    // method. All methods returned by /auth/methods are valid. (2) The server
    // then returns a token that the client can use for further requests.
    //
    
    switch (authMethod)
    {
        case AuthMethod.NONE:
            // We don't need to authenticate (part 1 of the process), so we return a token:
            var token = Guid.NewGuid().ToString();
            validTokens.Add(token);
            return new AuthResponse(true, token, null);
        
        case AuthMethod.USERNAME_PASSWORD:
            // Check if the username and password are present (part 1 of the process):
            if (!context.Request.Headers.TryGetValue("user", out var username) || !context.Request.Headers.TryGetValue("password", out var password))
                return new AuthResponse(false, null, "Username and password are required.");

            // Check a dummy user:
            if (username != "user1" || password != "test")
                return new AuthResponse(false, null, "Invalid username and/or password.");
                
            // Return a token (part 2 of the process):
            token = Guid.NewGuid().ToString();
            validTokens.Add(token);
            return new AuthResponse(true, token, null);
    }
    
    return new AuthResponse(false, null, "Unknown authentication method.");
})
    .WithDescription("Authenticate with the data source to get a token for further requests.")
    .WithName("Authenticate")
    .WithTags("Authentication");

#endregion

#region Embedding

app.MapGet("/embedding/info", () => new List<EmbeddingInfo>
{
    //
    // Just to demonstrate the usage of the EmbeddingInfo record.
    // This demo server uses doesn't use any embedding, though.
    //
    // In case a data source uses no embedding, it can return an
    // empty list.
    //
    new()
    {
        EmbeddingType = "Transformer Embedding",
        EmbeddingName = "OpenAI: text-embedding-3-large",
        Description = "Uses the text-embedding-3-large model from OpenAI",
        UsedWhen = "Anytime",
        Link = "https://platform.openai.com/docs/guides/embeddings/embedding-models",
    },
})
    .WithDescription("Get information about the used embedding(s).")
    .WithName("GetEmbeddingInfo")
    .WithTags("Embedding");

#endregion

#region Retrieval

app.MapGet("/retrieval/info", () => new List<RetrievalInfo>
{
    new ()
    {
        Id = "DEMO-1",
        Name = "DEMO: Wikipedia Links",
        Description = "Contains some links to Wikipedia articles.",
        Link = null,
        Embeddings = [],
    },
})
    .WithDescription("Get information about the retrieval processes implemented by this data source.")
    .WithName("GetRetrievalInfo")
    .WithTags("Retrieval");

app.MapPost("/retrieval", (RetrievalRequest request) =>
{
    //
    // We use a simple demo retrieval process here, without any embedding.
    // We're looking for matching keywords in the user prompt and return the
    // corresponding Wikipedia articles.
    //
    
    var results = new List<Context>();
    var lowerCasePrompt = request.LatestUserPrompt.ToLowerInvariant();
    foreach (var article in ExampleData.EXAMPLE_DATA)
    {
        // Find matches:
        if(!lowerCasePrompt.Contains(article.Title.ToLowerInvariant()))
            continue;
        
        if(request.MaxMatches > 0 && results.Count >= request.MaxMatches)
            break;
        
        results.Add(new Context
        {
            Name = article.Title,
            Category = "Wikipedia Article",
            Path = null,
            Type = ContentType.TEXT,
            MatchedContent = article.Url,
            SurroundingContent = [],
            Links = [ article.Url ],
        });
    }
    
    return results;
})
    .WithDescription("Retrieve information from the data source.")
    .WithName("Retrieve")
    .WithTags("Retrieval");

#endregion

#endregion

app.Run();