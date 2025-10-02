using Asp.Versioning;
using Asp.Versioning.Builder;

namespace DemoServer;

public static class Versions
{
    public const string VERSION_HEADER_FIELD_NAME = "eri-version";

    public static readonly ApiVersion V1_0 = new(1, 0);
    public static readonly ApiVersion V2_0 = new(2, 0);
    
    public static readonly ApiVersionSet SET_ALL_VERSIONS = new ApiVersionSetBuilder(null)
        .HasApiVersion(V1_0)
        .HasApiVersion(V2_0)
        .Build();
    
    public static readonly ApiVersionSet SET_V1_ONLY = new ApiVersionSetBuilder(null)
        .HasApiVersion(V1_0)
        .Build();
    
    public static readonly ApiVersionSet SET_V2_ONLY = new ApiVersionSetBuilder(null)
        .HasApiVersion(V2_0)
        .Build();
}