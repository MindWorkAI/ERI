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

        router.MapGet("/info", GetEmbeddingInfo)
            .WithDescription("Get information about the used embedding(s).")
            .WithName("GetEmbeddingInfo");
    }

    private static v10.EmbeddingInfo[] GetEmbeddingInfo() => EMBEDDING_INFO;
}