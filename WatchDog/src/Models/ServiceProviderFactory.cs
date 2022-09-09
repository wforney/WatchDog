namespace WatchDog.src.Models;

using global::WatchDog.src.Interfaces;

/// <summary>
/// The service provider factory class.
/// </summary>
public static class ServiceProviderFactory
{
    /// <summary>
    /// Gets or sets the broadcast helper.
    /// </summary>
    /// <value>The broadcast helper.</value>
    public static IBroadcastHelper? BroadcastHelper { get; set; }
}
