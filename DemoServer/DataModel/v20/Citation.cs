namespace DemoServer.DataModel.v20;

/// <summary>
/// A citation for a piece of information.
/// </summary>
/// <param name="Title">The title of the citation.</param>
/// <param name="Url">The URL of the citation.</param>
public readonly record struct Citation(string Title, string Url);