using v10 = DemoServer.DataModel.v10;
using v11 = DemoServer.DataModel.v11;

namespace DemoServer.Handler;

public static class Retrieval
{
    private const string TAG = "Retrieval";
    
    private static readonly v10.RetrievalInfo[] RETRIEVAL_INFO = 
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

        router.MapGet("/info", GetRetrievalInfoV10)
            .WithDescription("Get information about the retrieval processes implemented by this data source.")
            .WithName("GetRetrievalInfoV10")
            .MapToApiVersion(Versions.V1_0);
        
        router.MapGet("/info", GetRetrievalInfoV11)
            .WithDescription("Get information about the retrieval processes implemented by the specified data source.")
            .WithName("GetRetrievalInfoV1.1+")
            .MapToApiVersion(Versions.V1_1)
            .MapToApiVersion(Versions.V2_0);

        router.MapPost("/", RetrieveV10)
            .WithDescription("Retrieve information from the data source.")
            .WithName("RetrieveV10")
            .MapToApiVersion(Versions.V1_0);

        router.MapPost("/", RetrieveV11)
            .WithDescription("Retrieve information from the specified data source.")
            .WithName("RetrieveV1.1")
            .MapToApiVersion(Versions.V1_1);
    }

    /// <summary>
    /// Get information about the retrieval processes implemented by this data source.
    /// </summary>
    /// <returns>Information about the retrieval processes implemented by this data source.</returns>
    private static v10.RetrievalInfo[] GetRetrievalInfoV10() => RETRIEVAL_INFO;
    
    /// <summary>
    /// Get information about the retrieval processes implemented by the specified data source.
    /// </summary>
    /// <param name="dataSourceId">The data source ID for which to get the retrieval information.</param>
    /// <returns>Information about the retrieval processes implemented by the specified data source.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the data source ID is unknown.</exception>
    private static v10.RetrievalInfo[] GetRetrievalInfoV11(int dataSourceId)
    {
        if(dataSourceId != 0)
            throw new ArgumentOutOfRangeException(nameof(dataSourceId), "Invalid data source ID.");
        
        return RETRIEVAL_INFO;
    }

    /// <summary>
    /// Retrieve information from the data source.
    /// </summary>
    /// <param name="request">The retrieval request.</param>
    /// <returns>The retrieval results.</returns>
    private static List<v10.Context> RetrieveV10(v10.RetrievalRequest request)
    {
        //
        // We use a simple demo retrieval process here, without any embedding.
        // We're looking for matching keywords in the user prompt and return the
        // corresponding Wikipedia articles.
        //
    
        var results = new List<v10.Context>();
        foreach (var article in ExampleData.EXAMPLE_DATA)
        {
            // Find matches:
            if(!request.LatestUserPrompt.Contains(article.Title, StringComparison.InvariantCultureIgnoreCase))
                continue;
        
            if(request.MaxMatches > 0 && results.Count >= request.MaxMatches)
                break;
        
            results.Add(new v10.Context
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
    
    /// <summary>
    /// Retrieve information from the specified data source.
    /// </summary>
    /// <param name="dataSourceId">The data source ID from which to retrieve the information.</param>
    /// <param name="request">The retrieval request.</param>
    /// <returns>The retrieval results.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the data source ID is unknown.</exception>
    private static List<v11.Context> RetrieveV11(int dataSourceId, v10.RetrievalRequest request)
    {
        if(dataSourceId != 0)
            throw new ArgumentOutOfRangeException(nameof(dataSourceId), "Invalid data source ID.");
        
        //
        // We use a simple demo retrieval process here, without any embedding.
        // We're looking for matching keywords in the user prompt and return the
        // corresponding Wikipedia articles.
        //
    
        var results = new List<v11.Context>();
        foreach (var article in ExampleData.EXAMPLE_DATA)
        {
            // Find matches:
            if(!request.LatestUserPrompt.Contains(article.Title, StringComparison.InvariantCultureIgnoreCase))
                continue;
        
            if(request.MaxMatches > 0 && results.Count >= request.MaxMatches)
                break;
        
            results.Add(new v11.Context
            {
                Name = article.Title,
                Category = "Wikipedia Article",
                Path = article.Url,
                ConfidenceScore = 1f,
                Type = ContentType.TEXT,
                MatchedContent = article.Url,
                SurroundingContent = [],
                Links = [],
            });
        }
    
        return results;
    }
}