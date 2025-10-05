namespace DemoServer.DataModel.v20;

/// <summary>
/// The response from a client-side LLM task.
/// </summary>
/// <param name="LLMTaskId">The ID of the LLM task.</param>
/// <param name="Response">The response from the LLM.</param>
public record LLMResponse(Guid LLMTaskId, string Response)
{
    public LLMResponse() : this(Guid.Empty, string.Empty)
    {
    }
}