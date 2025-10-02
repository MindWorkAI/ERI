using DemoServer.DataModel;

namespace DemoServer.Handler;

public static class Retrieval
{
    private const string TAG = "Retrieval";
    
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

    public static void AddRetrievalHandlers(this IEndpointRouteBuilder app)
    {
        var router = app.MapGroup("/retrieval")
            .WithTags(TAG)
            .WithApiVersionSet(Versions.SET_ALL_VERSIONS);

        router.MapGet("/info", GetRetrievalInfo)
            .WithDescription("Get information about the retrieval processes implemented by this data source.")
            .WithName("GetRetrievalInfo");

        router.MapPost("/", Retrieve)
            .WithDescription("Retrieve information from the data source.")
            .WithName("Retrieve");
    }

    private static RetrievalInfo[] GetRetrievalInfo() => RETRIEVAL_INFO;

    private static List<Context> Retrieve(RetrievalRequest request)
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
                Path = article.Url,
                Type = ContentType.TEXT,
                MatchedContent = article.Url,
                SurroundingContent = [],
                Links = [],
            });
        }
    
        return results;
    }
}