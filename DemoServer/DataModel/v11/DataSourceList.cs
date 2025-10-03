namespace DemoServer.DataModel.v11;

/// <summary>
/// Briefly describes a data source.
/// </summary>
/// <param name="Id">The id of this data source. Use this index to refer to the data source in other API calls.</param>
/// <param name="Name">The name of the data source, e.g., "Internal Organization Documents."</param>
public readonly record struct DataSourceList(int Id, string Name);