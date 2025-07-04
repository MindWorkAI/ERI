using DemoServer.DataModel;

namespace DemoServer.Handler;

public static class Security
{
    private static readonly SecurityRequirements SECURITY_REQUIREMENTS = new(ProviderType.SELF_HOSTED);

    public static void AddSecurityHandlers(this WebApplication app)
    {
        app.MapGet("/security/requirements", GetSecurityRequirements)
            .WithDescription("Get the security requirements for this data source.")
            .WithName("GetSecurityRequirements")
            .WithTags("Security");
    }

    private static SecurityRequirements GetSecurityRequirements() => SECURITY_REQUIREMENTS;
}