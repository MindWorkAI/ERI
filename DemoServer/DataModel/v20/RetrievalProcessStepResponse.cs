namespace DemoServer.DataModel.v20;

/// <summary>
/// A response for a step in the retrieval process.
/// </summary>
/// <param name="StepType">Indicates what type of step this is. It might happen that the retrieval process
/// needs to call the user's LLM to get more information.</param>
/// <param name="LLMTaskId">The ID of the LLM task that was created for this step. When the retrieval process
/// does not need to call the user's LLM, this will be Guid.Empty.</param>
/// <param name="LLMTask">When the retrieval process needs to call the user's LLM, this contains
/// the chat thread to send to the user's LLM. When the retrieval process does not need to call
/// the user's LLM, this will be null.</param>
public record RetrievalProcessStepResponse(
    RetrievalProcessStepType StepType,
    Guid LLMTaskId,
    v10.ChatThread? LLMTask)
{
    public RetrievalProcessStepResponse() :this(
        RetrievalProcessStepType.NO_ACTION_NEEDED,
        Guid.Empty,
        null)
    {
    }
}