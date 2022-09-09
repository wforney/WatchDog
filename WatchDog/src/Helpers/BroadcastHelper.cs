namespace WatchDog.src.Helpers;

using global::WatchDog.src.Hubs;
using global::WatchDog.src.Interfaces;
using global::WatchDog.src.Models;
using Microsoft.AspNetCore.SignalR;

internal class BroadcastHelper : IBroadcastHelper
{
	private readonly IHubContext<LoggerHub> _hubContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="BroadcastHelper"/> class.
	/// </summary>
	/// <param name="hubContext">The hub context.</param>
	public BroadcastHelper(IHubContext<LoggerHub> hubContext)
	{
		_hubContext = hubContext;
	}

	/// <summary>
	/// Broadcasts the ex log.
	/// </summary>
	/// <param name="log">The log.</param>
	public async Task BroadcastExLog(WatchExceptionLog log)
	{
		var result = new { log, type = "exLog" };
		await _hubContext.Clients.All.SendAsync("getLogs", result);
	}

	/// <summary>
	/// Broadcasts the log.
	/// </summary>
	/// <param name="log">The log.</param>
	public async Task BroadcastLog(WatchLoggerModel log)
	{
		var result = new { log, type = "log" };
		await _hubContext.Clients.All.SendAsync("getLogs", result);
	}

	/// <summary>
	/// Broadcasts the watch log.
	/// </summary>
	/// <param name="log">The log.</param>
	public async Task BroadcastWatchLog(WatchLog log)
	{
		var result = new { log, type = "rqLog" };
		await _hubContext.Clients.All.SendAsync("getLogs", result);
	}
}
