using Microsoft.AspNetCore.Mvc;

namespace DemoServer.Handler;

public static class Tools
{
    private const string TAG = "Tools";

    public static void AddToolHandlers(this IEndpointRouteBuilder app)
    {
        var router = app.MapGroup("/tools")
            .WithTags(TAG)
            .WithApiVersionSet(Versions.SET_V2_0_AND_ABOVE);

        router.MapGet("/list", GetToolListV20)
            .WithDescription("Get a list of available tools for this ERI server.")
            .WithName("GetToolListV2.0+");
        
        router.MapPost("/execute", ExecuteToolV20)
            .WithDescription("Execute the specified tool with the given input.")
            .WithName("ExecuteToolV2.0+");
    }
    
    /// <summary>
    /// Get a list of available tools for this ERI server.
    /// </summary>
    /// <returns>A list of available tools.</returns>
    private static v20.ToolInfo[] GetToolListV20() => [];
    
    /// <summary>
    /// Execute the specified tool with the given input.
    /// </summary>
    /// <param name="toolId">The tool ID to execute.</param>
    /// <param name="input">The input for the tool.</param>
    /// <returns>The output of the tool execution.</returns>
    private static string ExecuteToolV20(Guid toolId, [FromBody] string input)
    {
        return string.Empty;
    }
}