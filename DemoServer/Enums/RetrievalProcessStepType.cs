namespace DemoServer.Enums;

/// <summary>
/// The type of action the client should take for a retrieval process step.
/// </summary>
public enum RetrievalProcessStepType
{
    /// <summary>
    /// No action is needed from the client.
    /// </summary>
    NO_ACTION_NEEDED,
    
    /// <summary>
    /// The client should send a query to the LLM to get more information.
    /// </summary>
    ASK_LLM,
    
    /// <summary>
    /// The retrieval process is complete.
    /// </summary>
    DONE,
}