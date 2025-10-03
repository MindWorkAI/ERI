namespace DemoServer.DataModel.v11;

/// <summary>
/// Represents the security requirements for this ERI server.<br/><br/>
/// 
/// The list of forbidden providers takes precedence over the list of allowed providers.
/// If a provider is in both lists, it is considered forbidden. Example: when you disallow all Asian
/// providers and allow all providers, the result is that all non-Asian providers are allowed.<br/><br/>
///
/// When both lists are empty, it means that all providers are allowed.
/// </summary>
/// <param name="ForbiddenProviders">The list of forbidden providers.</param>
/// <param name="AllowedProviders">The list of allowed providers.</param>
public readonly record struct SecurityRequirements(Provider[] ForbiddenProviders, Provider[] AllowedProviders);