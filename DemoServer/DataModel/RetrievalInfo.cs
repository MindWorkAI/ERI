namespace DemoServer.DataModel;

/// <summary>
/// Information about a retrieval process, which this data source implements.
/// </summary>
/// <param name="Id">A unique identifier for the retrieval process. This can be a GUID, a unique name, or an increasing integer.</param>
/// <param name="Name">The name of the retrieval process, e.g., "Keyword-Based Wikipedia Article Retrieval".</param>
/// <param name="Description">A short description of the retrieval process. What kind of retrieval process is it?</param>
/// <param name="Link">A link to the retrieval process's documentation, paper, Wikipedia article, or the source code. Might be null.</param>
/// <param name="Embeddings">A list of embeddings used in this retrieval process. It might be empty in case no embedding is used.</param>
public readonly record struct RetrievalInfo(
    string Id,
    string Name,
    string Description,
    string? Link,
    List<EmbeddingInfo> Embeddings);