namespace DemoServer.DataModel.v20;

/// <summary>
/// The delta text of a choice.
/// </summary>
/// <param name="Content">The content of the delta text.</param>
public record ChatCompletionDelta(string Content)
{
    public ChatCompletionDelta() : this(string.Empty)
    {
    }
}