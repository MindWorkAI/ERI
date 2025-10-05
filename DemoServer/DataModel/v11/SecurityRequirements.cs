namespace DemoServer.DataModel.v11;

/// <summary>
/// Represents the security requirements for this ERI server.<br/><br/>
/// 
/// The list of forbidden providers takes precedence over the list of allowed providers.
/// If a provider is in both lists, it is considered forbidden. Example: when you disallow all Asian
/// providers and allow all providers, the result is that all non-Asian providers are allowed.<br/><br/>
///
/// When both lists are empty, it means that all providers are allowed.<br/><br/>
///
/// You can use the allowed IP ranges to restrict access to LLM providers from specific IP ranges.
/// If the list is empty, there are no restrictions on IP ranges. You can use CIDR notation to specify
/// IP ranges (e.g., 192.168.1.0/30), single IP addresses (e.g., 192.168.0.1), or specify a range using
/// a start and end IP address (e.g., 10.0.0.1-10.0.0.255).<br/><br/>
///
/// This is useful if you want to allow access to LLM providers only from specific networks,
/// e.g., your corporate network or a VPN. In practice, this means that only self-hosted
/// or on-premise LLM providers can be used, as public LLM providers do not publish
/// their IP addresses. You should use this feature in combination with allowed providers
/// set to self-hosted providers only.
/// </summary>
/// <param name="ForbiddenProviders">The list of forbidden providers.</param>
/// <param name="AllowedProviders">The list of allowed providers.</param>
/// <param name="AllowedIPRanges">The list of allowed IP ranges for LLM providers.</param>
public readonly record struct SecurityRequirements(
    Provider[] ForbiddenProviders,
    Provider[] AllowedProviders,
    string[] AllowedIPRanges);