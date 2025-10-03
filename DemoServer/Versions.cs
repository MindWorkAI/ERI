using Asp.Versioning;
using Asp.Versioning.Builder;

namespace DemoServer;

public static class Versions
{
    public const string VERSION_HEADER_FIELD_NAME = "eri-version";

    public static readonly ApiVersion V1_0 = new(1, 0, "production");
    public static readonly ApiVersion V1_1 = new(1, 1, "design");
    public static readonly ApiVersion V2_0 = new(2, 0, "design");
    
    public static readonly ApiVersionSet SET_ALL_VERSIONS = new ApiVersionSetBuilder(null)
        .HasApiVersion(V1_0)
        .HasApiVersion(V1_1)
        .HasApiVersion(V2_0)
        .Build();
    
    public static readonly ApiVersionSet SET_ALL_V1_ONLY = new ApiVersionSetBuilder(null)
        .HasApiVersion(V1_0)
        .HasApiVersion(V1_1)
        .Build();
    
    public static readonly ApiVersionSet SET_V1_0_ONLY = new ApiVersionSetBuilder(null)
        .HasApiVersion(V1_0)
        .Build();
    
    public static readonly ApiVersionSet SET_V1_1_ONLY = new ApiVersionSetBuilder(null)
        .HasApiVersion(V1_1)
        .Build();
    
    public static readonly ApiVersionSet SET_V2_ONLY = new ApiVersionSetBuilder(null)
        .HasApiVersion(V2_0)
        .Build();
}