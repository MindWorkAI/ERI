namespace DemoServer.Handler;

public static class AuthHandler
{
    private const string TAG = "Authentication";
    private static readonly HashSet<string> VALID_TOKENS = new();
    
    private static readonly v10.AuthScheme[] ALLOWED_AUTH_SCHEMES =
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

    public static void AddAuthHandlers(this IEndpointRouteBuilder app)
    {
        var router = app.MapGroup("/auth")
            .WithTags(TAG)
            .WithApiVersionSet(Versions.SET_ALL_VERSIONS);

        router.MapGet("/methods", GetAuthMethods)
            .WithDescription("""
                             Get the available authentication methods.<br/><br/>
                             
                             This call requires no authentication.
                             """)
            .WithName("GetAuthMethodsV1.0+");

        router.MapPost("/", PerformAuth)
            .WithDescription("""
                             Authenticate with the data source to get a token for further requests.<br/><br/>
                             
                             This call requires no authentication.
                             """)
            .WithName("AuthenticateV1.0+");
    }
    
    /// <summary>
    /// Get the available authentication methods.
    /// </summary>
    /// <returns>The available authentication methods.</returns>
    private static v10.AuthScheme[] GetAuthMethods() => ALLOWED_AUTH_SCHEMES;

    /// <summary>
    /// Authenticate with the ERI server to get a token for further requests.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="authMethod">The authentication method to use.</param>
    /// <returns>The authentication response, including the token if authentication was successful.</returns>
    private static v10.AuthResponse PerformAuth(HttpContext context, AuthMethod authMethod)
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
                return new v10.AuthResponse(true, token, null);
        
            case AuthMethod.USERNAME_PASSWORD:
                // Check if the username and password are present (part 1 of the process):
                if (!context.Request.Headers.TryGetValue("user", out var username) || !context.Request.Headers.TryGetValue("password", out var password))
                    return new v10.AuthResponse(false, null, "Username and password are required.");

                // Check a dummy user:
                if (username != "user1" || password != "test")
                    return new v10.AuthResponse(false, null, "Invalid username and/or password.");
                
                // Return a token (part 2 of the process):
                token = Guid.NewGuid().ToString();
                VALID_TOKENS.Add(token);
                return new v10.AuthResponse(true, token, null);
        }
    
        return new v10.AuthResponse(false, null, "Unknown authentication method.");
    }

    public static void AddAuthFilter(this WebApplication app)
    {
        app.Use(EnsureAuth);
    }

    private static async Task EnsureAuth(HttpContext context, RequestDelegate next)
    {
        if(context.Request.Path.StartsWithSegments("/specification"))
        {
            await next(context);
            return;
        }
        
        if(context.Request.Path.StartsWithSegments("/server"))
        {
            await next(context);
            return;
        }
        
        if(context.Request.Path.Value!.Contains("/auth"))
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
}