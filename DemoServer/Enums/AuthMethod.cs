namespace DemoServer.Enums;

/// <summary>
/// An authentication method.
/// </summary>
public enum AuthMethod
{
    /// <summary>
    /// No authentication method.
    /// </summary>
    NONE,
    
    /// <summary>
    /// Kerberos authentication.
    /// </summary>
    KERBEROS,
    
    /// <summary>
    /// Username and password authentication.
    /// </summary>
    USERNAME_PASSWORD,
    
    /// <summary>
    /// Token-based authentication.
    /// </summary>
    TOKEN,
}