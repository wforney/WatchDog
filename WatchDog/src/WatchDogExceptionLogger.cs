namespace WatchDog.src;

using global::WatchDog.src.Interfaces;
using global::WatchDog.src.Managers;
using global::WatchDog.src.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

internal class WatchDogExceptionLogger
{
	private readonly IBroadcastHelper _broadcastHelper;
	private readonly ILogger _logger;
	private readonly RequestDelegate _next;
	private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

	/// <summary>
	/// Initializes a new instance of the <see cref="WatchDogExceptionLogger"/> class.
	/// </summary>
	/// <param name="next">The next.</param>
	/// <param name="loggerFactory">The logger factory.</param>
	/// <param name="broadcastHelper">The broadcast helper.</param>
	public WatchDogExceptionLogger(RequestDelegate next, ILoggerFactory loggerFactory, IBroadcastHelper broadcastHelper)
	{
		_next = next;
		_logger = loggerFactory.CreateLogger<WatchDogExceptionLogger>();
		_recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
		_broadcastHelper = broadcastHelper;
	}

	/// <summary>
	/// Invoke as an asynchronous operation.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			var requestLog = WatchDog.RequestLog;
			await LogException(ex, requestLog);
			throw;
		}
	}

	/// <summary>
	/// Logs the exception.
	/// </summary>
	/// <param name="ex">The exception.</param>
	/// <param name="requestModel">The request model.</param>
	public async Task LogException(Exception ex, RequestModel? requestModel)
	{
		Debug.WriteLine($"The following exception is logged: {ex.Message}");
		WatchExceptionLog watchExceptionLog = new()
		{
			EncounteredAt = DateTime.UtcNow,
			Message = ex.Message ?? string.Empty,
			StackTrace = ex.StackTrace ?? string.Empty,
			Source = ex.Source ?? string.Empty,
			TypeOf = ex.GetType().ToString(),
			Path = requestModel?.Path ?? string.Empty,
			Method = requestModel?.Method ?? string.Empty,
			QueryString = requestModel?.QueryString ?? string.Empty,
			RequestBody = requestModel?.RequestBody ?? string.Empty
		};

		// Insert
		await DynamicDBManager.InsertWatchExceptionLog(watchExceptionLog);
		await _broadcastHelper.BroadcastExLog(watchExceptionLog);
	}
}
