namespace DemoServer.DataModel.v11;

/// <summary>
/// Information about the ERI server.
/// </summary>
/// <param name="ServerName">The name of the server.</param>
/// <param name="Description">A short description of the server.</param>
/// <param name="ContactName">The name of the contact person for the server.</param>
/// <param name="ContactEmail">The email address of the contact person for the server.</param>
/// <param name="TermsOfServiceUrl">A URL to the terms of service for the server.</param>
/// <param name="PrivacyPolicyUrl">A URL to the privacy policy for the server.</param>
/// <param name="UHDUrl">A URL to the user help desk solution for the server.</param>
public record ServerInfo(
    string ServerName,
    string Description,
    string ContactName,
    string ContactEmail,
    string? TermsOfServiceUrl,
    string? PrivacyPolicyUrl,
    string? UHDUrl);