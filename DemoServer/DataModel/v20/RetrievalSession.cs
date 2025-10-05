namespace DemoServer.DataModel.v20;

/// <summary>
/// This represents a retrieval session for a data source.
/// </summary>
/// <param name="Id">The unique identifier of the retrieval session.</param>
/// <param name="CreatedAt">The date and time when the retrieval session was created.</param>
/// <param name="DataSourceId">The identifier of the data source for which the retrieval session was created.</param>
/// <param name="State">The current state of the retrieval session.</param>
/// <param name="Progress">The progress of the retrieval session.</param>
/// <param name="Semaphore">A semaphore to control access to the retrieval session.</param>
/// <param name="RetrievalTask">The task that performs the retrieval process.</param>
internal record RetrievalSession(
    Guid Id,
    DateTime CreatedAt,
    int DataSourceId,
    RetrievalState State,
    RetrievalProgress Progress,
    SemaphoreSlim? Semaphore,
    Task? RetrievalTask)
{
    public RetrievalSession(): this(
        Guid.Empty,
        DateTime.MinValue,
        0,
        RetrievalState.NOT_STARTED,
        new(),
        null,
        null)
    {
    }
}