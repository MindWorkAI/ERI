namespace DemoServer.DataModel.v20;

/// <summary>
/// Information about a retrieval process, which this data source implements.
/// </summary>
/// <param name="Id">A unique identifier for the retrieval process. This can be a GUID, a unique name,
/// or an increasing integer.</param>
/// <param name="Name">The name of the retrieval process, e.g.,
/// "Keyword-Based Wikipedia Article Retrieval".</param>
/// <param name="Description">A short description of the retrieval process. What kind of retrieval
/// process is it?</param>
/// <param name="Link">A link to the retrieval process's documentation, paper, Wikipedia article, or
/// the source code. Might be null.</param>
/// <param name="MustUseERIServerLLMProvider">Indicates whether this retrieval process depends on its
/// own LLM provider, which is hosted on the ERI server side. When true, the ERI client must not
/// provide an LLM provider for this retrieval process, as it will be ignored. Instead, the ERI client
/// will use the chat completion endpoint of the ERI server for the generation. Another case when this
/// is true is for confidential data sources, where the retrieval process must
/// ensure that no data is sent to any third-party LLM provider.</param>
/// <param name="ParametersDescription">A dictionary that describes the parameters of the retrieval
/// process. The key is the parameter name,
/// and the value is a description of the parameter. Although each parameter will be sent as a string,
/// the description should indicate the
/// expected type and range, e.g., 0.0 to 1.0 for a float parameter.</param>
/// <param name="Embeddings">A list of embeddings used in this retrieval process. It might be empty
/// in case no embedding is used.</param>
public readonly record struct RetrievalInfo(
    string Id,
    string Name,
    string Description,
    string? Link,
    bool MustUseERIServerLLMProvider,
    Dictionary<string, string>? ParametersDescription,
    List<v10.EmbeddingInfo> Embeddings);