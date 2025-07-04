using DemoServer.DataModel;

namespace DemoServer.Handler;

public static class AuthHandler
{
    private static readonly HashSet<string> VALID_TOKENS = new();
    
    private static readonly AuthScheme[] ALLOWED_AUTH_SCHEMES =
    [
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
    ];

    public static void AddAuthHandlers(this WebApplication app)
    {
        app.Use(EnsureAuth);
        app.MapGet("/auth/methods", GetAuthMethods)
            .WithDescription("Get the available authentication methods.")
            .WithName("GetAuthMethods")
            .WithTags("Authentication");

        app.MapPost("/auth", PerformAuth)
            .WithDescription("Authenticate with the data source to get a token for further requests.")
            .WithName("Authenticate")
            .WithTags("Authentication");
    }

    private static IEnumerable<AuthScheme> GetAuthMethods() => ALLOWED_AUTH_SCHEMES;

    private static async Task EnsureAuth(HttpContext context, RequestDelegate next)
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
    
        if (!VALID_TOKENS.Contains(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid token provided." });
            return;
        }
    
        // Call the next delegate/middleware in the pipeline.
        await next(context);
    }

    private static AuthResponse PerformAuth(HttpContext context, AuthMethod authMethod)
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
                VALID_TOKENS.Add(token);
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
                VALID_TOKENS.Add(token);
                return new AuthResponse(true, token, null);
        }
    
        return new AuthResponse(false, null, "Unknown authentication method.");
    }
}