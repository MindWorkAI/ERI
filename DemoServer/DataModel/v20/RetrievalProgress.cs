namespace DemoServer.DataModel.v20;

/// <summary>
/// Represents the progress of a retrieval operation.
/// </summary>
/// <param name="Type">The type of progress being reported.</param>
/// <param name="Current">The current progress value. When the type is CONTINUOUS, this is ignored.</param>
/// <param name="Total">The total value of the progress. When the type is CONTINUOUS, this is ignored.
/// When the type is PERCENTAGE, this is always 100.</param>
public record RetrievalProgress(RetrievalProgressType Type, int Current, int Total)
{
    public RetrievalProgress() : this(RetrievalProgressType.PERCENTAGE, 0, 100)
    {
    }
}