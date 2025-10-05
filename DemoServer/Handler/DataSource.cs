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
            .WithName("GetDataSourceInfoV1.0")
            .MapToApiVersion(Versions.V1_0);
        
        router.MapGet("/", GetDataSourceInfoV11)
            .WithDescription("Get information about a data source.")
            .WithName("GetDataSourceInfoV1.1+")
            .MapToApiVersion(Versions.V1_1)
            .MapToApiVersion(Versions.V2_0);

        router.MapGet("/list", GetDataSourceList)
            .WithDescription("Get a list of available data sources.")
            .WithName("GetDataSourceListV1.1+")
            .MapToApiVersion(Versions.V1_1)
            .MapToApiVersion(Versions.V2_0);
    }

    /// <summary>
    /// Get the data source information for this ERI server.
    /// </summary>
    /// <returns>The data source information.</returns>
    private static v10.DataSourceInfo GetDataSourceInfoV10() => DATA_SOURCE_INFO;
    
    /// <summary>
    /// Get the data source information for the given data source ID.
    /// </summary>
    /// <param name="dataSourceId">The data source ID for which to get the information.</param>
    /// <returns>The data source information.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the data source ID is unknown.</exception>
    private static v10.DataSourceInfo GetDataSourceInfoV11(int dataSourceId)
    {
        if(dataSourceId != 0)
            throw new ArgumentOutOfRangeException(nameof(dataSourceId), "Invalid data source ID.");
        
        return DATA_SOURCE_INFO;
    }

    /// <summary>
    /// Get a list of available data sources for this ERI server.
    /// </summary>
    /// <returns>A list of available data sources.</returns>
    private static v11.DataSourceList[] GetDataSourceList() => [ new(0, "DEMO: Wikipedia Links") ];
}