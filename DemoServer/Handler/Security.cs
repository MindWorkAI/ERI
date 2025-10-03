using v10 = DemoServer.DataModel.v10;
using v11 = DemoServer.DataModel.v11;

namespace DemoServer.Handler;

public static class Security
{
    private const string TAG = "Security";
    private static readonly v10.SecurityRequirements SECURITY_REQUIREMENTS_V10 = new(ProviderType.ANY);
    private static readonly v11.SecurityRequirements SECURITY_REQUIREMENTS_V11 = new([], [Provider.ALL]);

    public static void AddSecurityHandlers(this IEndpointRouteBuilder app)
    {
        var router = app.MapGroup("/security")
            .WithTags(TAG)
            .WithApiVersionSet(Versions.SET_ALL_VERSIONS);
        
        router.MapGet("/requirements", GetSecurityRequirementsV10)
            .WithDescription("Get the security requirements for this data source.")
            .WithName("GetSecurityRequirementsV1.0")
            .MapToApiVersion(Versions.V1_0);
        
        router.MapGet("/requirements", GetSecurityRequirementsV11)
            .WithDescription("""
                             Represents the security requirements for this ERI server.<br/><br/>
                              
                             The list of forbidden providers takes precedence over the list of allowed providers.
                             If a provider is in both lists, it is considered forbidden. Example: when you disallow all Asian
                             providers and allow all providers, the result is that all non-Asian providers are allowed.<br/><br/>
                             
                             When both lists are empty, it means that all providers are allowed.
                             """)
            .WithName("GetSecurityRequirementsV1.1+")
            .MapToApiVersion(Versions.V1_1)
            .MapToApiVersion(Versions.V2_0);
    }

    /// <summary>
    /// Get the security requirements for this data source.
    /// </summary>
    /// <returns>The security requirements for this data source.</returns>
    private static v10.SecurityRequirements GetSecurityRequirementsV10() => SECURITY_REQUIREMENTS_V10;
    
    /// <summary>
    /// Get the security requirements for this ERI server.
    /// </summary>
    /// <returns>The security requirements for this ERI server.</returns>
    private static v11.SecurityRequirements GetSecurityRequirementsV11() => SECURITY_REQUIREMENTS_V11;
}