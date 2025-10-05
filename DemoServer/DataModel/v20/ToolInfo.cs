namespace DemoServer.DataModel.v20;

/// <summary>
/// Information about a tool available in this ERI server.
/// </summary>
/// <param name="Id">The unique identifier of the tool.</param>
/// <param name="Name">The name of the tool.</param>
/// <param name="Description">A short description of the tool.</param>
/// <param name="JsonSchema">The JSON schema that describes the entire tool. May have a look
/// at https://platform.openai.com/docs/guides/function-calling or
/// https://docs.claude.com/en/docs/agents-and-tools/tool-use/overview#tool-use-examples.</param>
public record ToolInfo(Guid Id, string Name, string Description, string JsonSchema)
{
    public ToolInfo() : this(
        Guid.Empty,
        string.Empty,
        string.Empty,
        string.Empty)
    {
    }
}