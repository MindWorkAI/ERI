using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Xml.Linq;

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace DemoServer;

public sealed class EnumSchemaTransformer : IOpenApiSchemaTransformer
{
    private static readonly string XML_DOCUMENTATION_FILE = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;
        if (!type.IsEnum)
            return Task.CompletedTask;

        schema.Enum = [..Enum.GetNames(type).Select(JsonNode (name) => JsonValue.Create(name))];
        schema.Type = JsonSchemaType.String;
        schema.Format = null;
        schema.Description = BuildDescription(type.GetTypeInfo());
        return Task.CompletedTask;
    }

    private static string BuildDescription(TypeInfo typeInfo)
    {
        var docMembers = LoadXmlMembers();
        if (docMembers is null)
            return string.Empty;
        
        var sb = new StringBuilder();

        var docMember = XmlDocHelper.GetTypeMember(docMembers, typeInfo);
        if(docMember is null)
            return string.Empty;
        
        sb.AppendLine(docMember.Value.Trim());
        sb.AppendLine();

        BuildMembersDescription(typeInfo, sb, docMembers);
        return sb.ToString();
    }

    private static void BuildMembersDescription(TypeInfo typeInfo, StringBuilder sb, XElement docMembers)
    {
        var enumValues = Enum.GetValues(typeInfo);
        foreach (var enumValue in enumValues)
        {
            var name = enumValue.ToString();
            if (name == null)
                continue;
            
            var member = typeInfo.GetMember(name).Single();
            var docMember = XmlDocHelper.GetDocMember(docMembers, member);
            if (docMember == null)
                continue;
            
            var description = docMember.Value.Trim();
            sb.AppendFormat("* `{0}` - {1}", name, description);
            sb.AppendLine();
        }
    }

    private static XElement? LoadXmlMembers()
    {
        var file = XML_DOCUMENTATION_FILE;
        var docXml = XDocument.Load(file);
        var xmlRoot = docXml.Root;

        if (xmlRoot is null)
            throw new ArgumentNullException(nameof(xmlRoot), "The XML documentation file is invalid.");

        return xmlRoot.Element("members");
    }
}