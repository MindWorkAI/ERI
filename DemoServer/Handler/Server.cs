using v11 = DemoServer.DataModel.v11;

namespace DemoServer.Handler;

public static class Server
{
    private const string TAG = "ERI Server";

    private static readonly v11.ServerInfo SERVER_INFO = new(
        ServerName: "Demo ERI Server",
        Description: "This is a demo implementation of an ERI server.",
        ContactName: "John Doe",
        ContactEmail: "john.doe@example.com",
        TermsOfServiceUrl: null,
        PrivacyPolicyUrl: null,
        UHDUrl: null);

    public static void AddServerHandlers(this IEndpointRouteBuilder app)
    {
        var router = app.MapGroup("/server")
            .WithTags(TAG)
            .WithApiVersionSet(Versions.SET_V1_1_AND_ABOVE);

        router.MapGet("/", GetServerInfoV11)
            .WithDescription("""
                             Get information about the ERI server.<br/><br/>
                             
                             This call requires no authentication.
                             """)
            .WithName("GetServerInfoV1.1+");
    }
    
    /// <summary>
    /// Get information about the ERI server.
    /// </summary>
    /// <returns>Information about the ERI server.</returns>
    private static v11.ServerInfo GetServerInfoV11() => SERVER_INFO;
}