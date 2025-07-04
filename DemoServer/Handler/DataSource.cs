using DemoServer.DataModel;

namespace DemoServer.Handler;

public static class DataSource
{
    private static readonly DataSourceInfo DATA_SOURCE_INFO = new("DEMO: Wikipedia Links", "Contains some links to Wikipedia articles.");

    public static void AddDataSourceHandlers(this WebApplication app)
    {
        app.MapGet("/dataSource", GetDataSourceInfo)
            .WithDescription("Get information about the data source.")
            .WithName("GetDataSourceInfo")
            .WithTags("Data Source");
    }

    private static DataSourceInfo GetDataSourceInfo() => DATA_SOURCE_INFO;
}