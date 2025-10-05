namespace DemoServer.Enums;

/// <summary>
/// Represents the state of a retrieval result.
/// </summary>
public enum RetrievalResultState
{
    /// <summary>
    /// The state of the retrieval result is unknown.
    /// </summary>
    UNKNOWN,
    
    /// <summary>
    /// The retrieval process is still ongoing. Thus, results are not yet available.
    /// </summary>
    PROCESS_STILL_ONGOING,
    
    /// <summary>
    /// The retrieval process has completed, and results are ready to be accessed.
    /// </summary>
    RESULTS_READY,
    
    /// <summary>
    /// There are no results found or left to retrieve.
    /// </summary>
    NO_RESULTS_FOUND,
}