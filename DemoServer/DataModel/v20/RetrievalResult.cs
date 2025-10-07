namespace DemoServer.DataModel.v20;

/// <summary>
/// The result of a retrieval operation.
/// </summary>
/// <param name="State">The state of the retrieval result operation. You may have to call this
/// endpoint multiple times until the state is NO_RESULTS_FOUND.</param>
/// <param name="Contexts">The list of contexts retrieved. This list may be empty when this retrieval
/// process is a conclusive answer retrieval process (aka deep research).</param>
/// <param name="Citations">The list of citations. This list may be null or empty.</param>
/// <param name="ConclusiveAnswer">A conclusive answer if this retrieval process is a conclusive answer
/// retrieval process (aka deep research). This will be null or empty when the retrieval process performed
/// classic retrieval.</param>
/// <param name="MoreResultsAvailable">Indicates whether more results are available for retrieval.
/// This is useful for pagination scenarios.</param>
public record RetrievalResult(
    RetrievalResultState State,
    List<v11.Context> Contexts,
    string? ConclusiveAnswer,
    List<Citation>? Citations,
    bool MoreResultsAvailable)
{
    public RetrievalResult() : this(
        RetrievalResultState.UNKNOWN,
        [],
        null,
        null,
        false)
    {
    }
}