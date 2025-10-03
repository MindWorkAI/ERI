using v10 = DemoServer.DataModel.v10;

namespace DemoServer.Handler;

public static class Embedding
{
    private const string TAG = "Embedding";
    private static readonly v10.EmbeddingInfo[] EMBEDDING_INFO =
    [
        //
        // Just to demonstrate the usage of the EmbeddingInfo record.
        // This demo server uses doesn't use any embedding, though.
        //
        // In case a data source uses no embedding, it can return an
        // empty list.
        //
        new()
        {
            EmbeddingType = "Transformer Embedding",
            EmbeddingName = "OpenAI: text-embedding-3-large",
            Description = "Uses the text-embedding-3-large model from OpenAI",
            UsedWhen = "Anytime",
            Link = "https://platform.openai.com/docs/guides/embeddings/embedding-models",
        },
    ];
    
    public static void AddEmbeddingHandlers(this IEndpointRouteBuilder app)
    {
        var router = app.MapGroup("/embedding")
            .WithTags(TAG)
            .WithApiVersionSet(Versions.SET_ALL_VERSIONS);

        router.MapGet("/info", GetEmbeddingInfoV10)
            .WithDescription("Get information about the used embedding(s).")
            .WithName("GetEmbeddingInfoV1.0")
            .MapToApiVersion(Versions.V1_0);
        
        router.MapGet("/info", GetEmbeddingInfoV11)
            .WithDescription("Get information about the embedding(s) used by a specific data source.")
            .WithName("GetEmbeddingInfoV1.1+")
            .MapToApiVersion(Versions.V1_1)
            .MapToApiVersion(Versions.V2_0);
    }

    /// <summary>
    /// Get information about the embedding(s) used by this data source.
    /// </summary>
    /// <returns>Information about the embedding(s) used by this data source.</returns>
    private static v10.EmbeddingInfo[] GetEmbeddingInfoV10() => EMBEDDING_INFO;
    
    /// <summary>
    /// Get information about the embedding(s) used by the specified data source.
    /// </summary>
    /// <param name="dataSourceId">The data source ID for which to get the embedding information.</param>
    /// <returns>Information about the embedding(s) used by the specified data source.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the data source ID is unknown.</exception>
    private static v10.EmbeddingInfo[] GetEmbeddingInfoV11(int dataSourceId)
    {
        if(dataSourceId != 0)
            throw new ArgumentOutOfRangeException(nameof(dataSourceId), "Invalid data source ID.");
        
        return EMBEDDING_INFO;
    }
}