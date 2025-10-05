namespace DemoServer.DataModel.v20;

/// <summary>
/// Data model for a delta line in the chat completion response stream.
/// </summary>
/// <param name="Choices">The choices made by the AI.</param>
public record ChatCompletionDeltaStreamLine(IList<ChatCompletionChoice> Choices)
{
    public ChatCompletionDeltaStreamLine() : this([])
    {
    }
}