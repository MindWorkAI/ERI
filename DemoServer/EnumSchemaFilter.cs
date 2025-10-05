using System.Reflection;
using System.Text;
using System.Xml.Linq;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace DemoServer;

public sealed class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            model.Enum.Clear();
            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(n =>
                {
                    model.Enum.Add(new OpenApiString(n)); 
                    model.Type = "string";
                    model.Format = null;
                    model.Description = BuildDescription(context.Type.GetTypeInfo());
                });
        }
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
        var file = ConfigureSwaggerGen.XML_DOCUMENTATION_FILE;
        var docXml = XDocument.Load(file);
        var xmlRoot = docXml.Root;

        if (xmlRoot is null)
            throw new ArgumentNullException(nameof(xmlRoot), "The XML documentation file is invalid.");

        return xmlRoot.Element("members");
    }
}