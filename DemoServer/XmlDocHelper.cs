using System.Reflection;
using System.Xml.Linq;

namespace DemoServer;

/// <summary>
/// Extensions methods for XML doc files based on Justin Lessard's repository https://github.com/Justin-Lessard/swagger-gen-enums.
/// </summary>
public static class XmlDocHelper
{
    /// <summary>
    /// Retrieves the XML documentation element corresponding to a specified member in the XML members root.
    /// </summary>
    /// <param name="membersRoot">The root XML element containing member documentation.</param>
    /// <param name="member">The member information for which to retrieve the documentation.</param>
    /// <returns>The XML element representing the documentation for the specified member, or null if no matching element is found.</returns>
    public static XElement? GetDocMember(XElement membersRoot, MemberInfo member)
    {
        var memberId = GetMemberId(member);
        return membersRoot
            .Elements("member")
            .FirstOrDefault(e => e.Attribute("name")?.Value == memberId);
    }

    /// <summary>
    /// Retrieves the XML documentation element corresponding to a specified type in the XML members root.
    /// </summary>
    /// <param name="membersRoot">The root XML element containing member documentation.</param>
    /// <param name="member">The type information for which to retrieve the documentation.</param>
    /// <returns>The XML element representing the documentation for the specified type or null if no matching element is found.</returns>
    public static XElement? GetTypeMember(XElement membersRoot, TypeInfo member)
    {
        var memberId = GetMemberId(member);
        return membersRoot
            .Elements("member")
            .FirstOrDefault(e => e.Attribute("name")?.Value == memberId);
    }

    /// <summary>
    /// Retrieves the full name of a member, including its declaring type or namespace, if applicable.
    /// </summary>
    /// <param name="member">The member for which to compute the full name.</param>
    /// <returns>A string representing the full name of the member, including its scope.</returns>
    private static string GetMemberFullName(MemberInfo member)
    {
        var memberScope = string.Empty;
        if (member.DeclaringType != null)
            memberScope = GetMemberFullName(member.DeclaringType);
        else if (member is Type type)
            memberScope = type.Namespace;

        return $"{memberScope}.{member.Name}";
    }

    /// <summary>
    /// Generates the unique identifier for a specified member, which includes a prefix indicating the member type and the full name of the member.
    /// </summary>
    /// <param name="member">The member for which to generate the unique identifier.</param>
    /// <returns>A string that uniquely identifies the member, prefixed with its type designation.</returns>
    private static string GetMemberId(MemberInfo member)
    {
        var memberKindPrefix = GetMemberPrefix(member);
        var memberName = GetMemberFullName(member);

        return $"{memberKindPrefix}:{memberName}";
    }

    /// <summary>
    /// Determines the prefix character used in the XML documentation identifier for a given member type.
    /// </summary>
    /// <param name="member">The member information for which to determine the prefix character.</param>
    /// <returns>The character representing the type of the member in the XML documentation identifier.</returns>
    private static char GetMemberPrefix(MemberInfo member)
    {
        var typeName = member.GetType().Name;
        switch (typeName)
        {
            case "MdFieldInfo":
                return 'F';

            default:
                return typeName.Replace("Runtime", "")[0];
        }
    }
}