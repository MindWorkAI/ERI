using DemoServer.DataModel;

namespace DemoServer.Handler;

public static class Embedding
{
    private static readonly EmbeddingInfo[] EMBEDDING_INFO =
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

    public static void AddEmbeddingHandlers(this WebApplication app)
    {
        app.MapGet("/embedding/info", GetEmbeddingInfo)
            .WithDescription("Get information about the used embedding(s).")
            .WithName("GetEmbeddingInfo")
            .WithTags("Embedding");
    }

    private static IEnumerable<EmbeddingInfo> GetEmbeddingInfo() => EMBEDDING_INFO;
}