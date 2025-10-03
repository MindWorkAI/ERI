using v10 = DemoServer.DataModel.v10;

namespace DemoServer.Handler;

public static class DataSource
{
    private const string TAG = "Data Source";
    private static readonly v10.DataSourceInfo DATA_SOURCE_INFO = new("DEMO: Wikipedia Links", "Contains some links to Wikipedia articles.");

    public static void AddDataSourceHandlers(this IEndpointRouteBuilder app)
    {
        var router = app.MapGroup("/dataSource")
            .WithTags(TAG)
            .WithApiVersionSet(Versions.SET_ALL_VERSIONS);

        router.MapGet("/", GetDataSourceInfoV10)
            .WithDescription("Get information about the data source.")
            .WithName("GetDataSourceInfo");
    }

    /// <summary>
    /// Get the data source information for this ERI server.
    /// </summary>
    /// <returns>The data source information.</returns>
    private static v10.DataSourceInfo GetDataSourceInfoV10() => DATA_SOURCE_INFO;
}