using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Http.Json;

namespace DemoServer;

public static class Json
{
    public static void Configure(JsonOptions options)
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    }
}