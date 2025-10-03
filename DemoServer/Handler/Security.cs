using v10 = DemoServer.DataModel.v10;

namespace DemoServer.Handler;

public static class Security
{
    private const string TAG = "Security";
    private static readonly v10.SecurityRequirements SECURITY_REQUIREMENTS = new(ProviderType.ANY);

    public static void AddSecurityHandlers(this IEndpointRouteBuilder app)
    {
        var router = app.MapGroup("/security")
            .WithTags(TAG)
            .WithApiVersionSet(Versions.SET_ALL_VERSIONS);
        
        router.MapGet("/requirements", GetSecurityRequirements)
            .WithDescription("Get the security requirements for this data source.")
            .WithName("GetSecurityRequirements");
    }

    private static v10.SecurityRequirements GetSecurityRequirements() => SECURITY_REQUIREMENTS;
}