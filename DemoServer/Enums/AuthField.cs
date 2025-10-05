namespace DemoServer.Enums;

/// <summary>
/// An authentication field.
/// </summary>
public enum AuthField
{
    /// <summary>
    /// No authentication field.
    /// </summary>
    NONE,
    
    /// <summary>
    /// The username field.
    /// </summary>
    USERNAME,
    
    /// <summary>
    /// The password field.
    /// </summary>
    PASSWORD,
    
    /// <summary>
    /// The authentication token field.
    /// </summary>
    TOKEN,
    
    /// <summary>
    /// The Kerberos ticket field.
    /// </summary>
    KERBEROS_TICKET,
}