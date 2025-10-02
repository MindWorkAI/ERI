using Asp.Versioning;

namespace DemoServer;

public static class APIVersionOptions
{
    public static void AddAPIVersioning(this IServiceCollection services)
    {
        var defaultVersion = Versions.V1_0;
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = defaultVersion;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new HeaderApiVersionReader(Versions.VERSION_HEADER_FIELD_NAME);
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VV";
            options.DefaultApiVersion = defaultVersion;
            options.SubstituteApiVersionInUrl = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        });
    }
}