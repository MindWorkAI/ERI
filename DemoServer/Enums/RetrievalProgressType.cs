namespace DemoServer.Enums;

/// <summary>
/// The type of progress indication for a retrieval operation.
/// </summary>
public enum RetrievalProgressType
{
    /// <summary>
    /// The progress is indicated as a percentage (0-100).
    /// </summary>
    PERCENTAGE,
    
    /// <summary>
    /// The progress is indicated as the number of items processed out of a total number of items.
    /// </summary>
    NUMBER_OF_ITEMS,
    
    /// <summary>
    /// The progress is indicated as a continuous stream of updates. We might not know the
    /// total amount of work to be done.
    /// </summary>
    CONTINUOUS,
}