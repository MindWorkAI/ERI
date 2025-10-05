namespace DemoServer.Enums;

/// <summary>
/// Represents the state of a retrieval operation.
/// </summary>
public enum RetrievalState
{
    /// <summary>
    /// The retrieval process has not started yet.
    /// </summary>
    NOT_STARTED,
    
    /// <summary>
    /// The retrieval process is ongoing.
    /// </summary>
    ONGOING,
    
    /// <summary>
    /// The retrieval process has completed successfully.
    /// </summary>
    DONE,
}