namespace DemoServer.DataModel.v20;

/// <summary>
/// A message in a chat conversation.
/// </summary>
/// <param name="Content">The text content of the message.</param>
/// <param name="Role">The role of the message, e.g., "user", "assistant", or "system".</param>
public record ChatMessage(string Content, string Role);