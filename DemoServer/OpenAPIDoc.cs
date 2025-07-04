using System.Reflection;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace DemoServer;

public static class OpenAPIDoc
{
    public static void Configure(SwaggerGenOptions options)
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        options.SchemaFilter<EnumSchemaFilter>();
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "ERI - (E)xternal (R)etrieval (I)nterface",
            Version = "v1",
            Description = """
                          This API serves as a contract between LLM tools like AI Studio and any external data sources for RAG
                          (retrieval-augmented generation). The tool, e.g., AI Studio acts as the client (the augmentation and
                          generation parts) and the data sources act as the server (the retrieval part). The data
                          sources implement some form of data retrieval and return a suitable context to the LLM tool.
                          The LLM tool, in turn, handles the integration of appropriate LLMs (augmentation & generation).
                          Data sources can be document or graph databases, or even a file system, for example. They
                          will likely implement an appropriate retrieval process by using some kind of embedding.
                          However, this API does not inherently require any embedding, as data processing is
                          implemented decentralized by the data sources.

                          The client expects that all fields in the JSON responses from an ERI server are named according
                          to camel case or Pascal case conventions. The client's JSON objects for requests use camel case
                          for the field names.
                          """
        });

        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "token",
            Description = "Enter the ERI token yielded by the authentication process at /auth.",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "token",
            Reference = new OpenApiReference
            {
                Id = "ERI_Token",
                Type = ReferenceType.SecurityScheme
            }
        };

        options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securityScheme, [] }
        });
    }
}