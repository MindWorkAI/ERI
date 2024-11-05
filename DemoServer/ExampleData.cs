namespace DemoServer;

/// <summary>
/// This is a simple demonstration for a data source. In fact, you would want to
/// connect a graph database or a document database or a server's file system instead:
/// </summary>
public static class ExampleData
{
    public static readonly WikipediaArticle[] EXAMPLE_DATA = {
        new("Strategic foresight", "https://en.wikipedia.org/wiki/Strategic_foresight"),
        new("Scenario planning", "https://en.wikipedia.org/wiki/Scenario_planning"),
        new("Futures studies", "https://en.wikipedia.org/wiki/Futures_studies"),
        new("Futures techniques", "https://en.wikipedia.org/wiki/Futures_techniques"),
        new("Delphi method", "https://en.wikipedia.org/wiki/Delphi_method"),       
    };
}