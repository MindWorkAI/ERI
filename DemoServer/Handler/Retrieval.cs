using DemoServer.DataModel;

namespace DemoServer.Handler;

public static class Retrieval
{
    private static readonly RetrievalInfo[] RETRIEVAL_INFO = 
    [
        new ()
        {
            Id = "DEMO-1",
            Name = "DEMO: Wikipedia Links",
            Description = "Contains some links to Wikipedia articles.",
            Link = null,
            Embeddings = [],
        },
    ];

    public static void AddRetrievalHandlers(this WebApplication app)
    {
        app.MapGet("/retrieval/info", GetRetrievalInfo)
            .WithDescription("Get information about the retrieval processes implemented by this data source.")
            .WithName("GetRetrievalInfo")
            .WithTags("Retrieval");

        app.MapPost("/retrieval", Retrieve)
            .WithDescription("Retrieve information from the data source.")
            .WithName("Retrieve")
            .WithTags("Retrieval");
    }

    private static IEnumerable<RetrievalInfo> GetRetrievalInfo() => RETRIEVAL_INFO;

    private static IEnumerable<Context> Retrieve(RetrievalRequest request)
    {
        //
        // We use a simple demo retrieval process here, without any embedding.
        // We're looking for matching keywords in the user prompt and return the
        // corresponding Wikipedia articles.
        //
    
        var results = new List<Context>();
        foreach (var article in ExampleData.EXAMPLE_DATA)
        {
            // Find matches:
            if(!request.LatestUserPrompt.Contains(article.Title, StringComparison.InvariantCultureIgnoreCase))
                continue;
        
            if(request.MaxMatches > 0 && results.Count >= request.MaxMatches)
                break;
        
            results.Add(new Context
            {
                Name = article.Title,
                Category = "Wikipedia Article",
                Path = null,
                Type = ContentType.TEXT,
                MatchedContent = article.Url,
                SurroundingContent = [],
                Links = [ article.Url ],
            });
        }
    
        return results;
    }
}