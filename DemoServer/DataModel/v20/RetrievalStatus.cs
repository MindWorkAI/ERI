namespace DemoServer.DataModel.v20;

/// <summary>
/// The status of a retrieval operation.
/// </summary>
/// <param name="State">This is the current state of the retrieval operation.</param>
/// <param name="Progress">This contains progress information about the retrieval operation.</param>
public record RetrievalStatus(RetrievalState State, RetrievalProgress Progress)
{
    public RetrievalStatus() : this(RetrievalState.NOT_STARTED, new())
    {
    }
}