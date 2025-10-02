using DemoServer.DataModel;

namespace DemoServer.Handler;

public static class DataSource
{
    private const string TAG = "Data Source";
    private static readonly DataSourceInfo DATA_SOURCE_INFO = new("DEMO: Wikipedia Links", "Contains some links to Wikipedia articles.");

    public static void AddDataSourceHandlers(this IEndpointRouteBuilder app)
    {
        var router = app.MapGroup("/dataSource")
            .WithTags(TAG)
            .WithApiVersionSet(Versions.SET_ALL_VERSIONS);

        router.MapGet("/", GetDataSourceInfo)
            .WithDescription("Get information about the data source.")
            .WithName("GetDataSourceInfo");
    }

    private static DataSourceInfo GetDataSourceInfo() => DATA_SOURCE_INFO;
}