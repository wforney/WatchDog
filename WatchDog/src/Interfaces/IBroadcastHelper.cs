namespace WatchDog.src.Interfaces;

using global::WatchDog.src.Models;

/// <summary>
/// The broadcast helper interface.
/// </summary>
public interface IBroadcastHelper
{
	/// <summary>
	/// Broadcasts the exception log.
	/// </summary>
	/// <param name="log">The log.</param>
	/// <returns>A <see cref="Task"/>.</returns>
	Task BroadcastExLog(WatchExceptionLog log);

	/// <summary>
	/// Broadcasts the log.
	/// </summary>
	/// <param name="log">The log.</param>
	/// <returns>A <see cref="Task"/>.</returns>
	Task BroadcastLog(WatchLoggerModel log);

	/// <summary>
	/// Broadcasts the watch log.
	/// </summary>
	/// <param name="log">The log.</param>
	/// <returns>A <see cref="Task"/>.</returns>
	Task BroadcastWatchLog(WatchLog log);
}
