using Asp.Versioning;

namespace DemoServer;

public static class APIVersionOptions
{
    extension(IServiceCollection services)
    {
        public void AddAPIVersioning()
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = Versions.V1_0;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader(Versions.VERSION_HEADER_FIELD_NAME);
            }).AddApiExplorer(options =>
            {
                // The API explorer takes the default version from the API versioning options above:
                options.GroupNameFormat = "'v'VVVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}