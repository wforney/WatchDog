namespace WatchDog.src;

using global::WatchDog.src.Interfaces;
using global::WatchDog.src.Managers;
using global::WatchDog.src.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

internal class WatchDogExceptionLogger
{
	private readonly RequestDelegate _next;
	private readonly ILogger _logger;
	private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
	private readonly IBroadcastHelper _broadcastHelper;
	public WatchDogExceptionLogger(RequestDelegate next, ILoggerFactory loggerFactory, IBroadcastHelper broadcastHelper)
	{
		_next = next;
		_logger = loggerFactory.CreateLogger<WatchDogExceptionLogger>();
		_recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
		_broadcastHelper = broadcastHelper;
	}

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
	public async Task LogException(Exception ex, RequestModel requestModel)
	{
		Debug.WriteLine($"The following exception is logged: {ex.Message}");
		WatchExceptionLog watchExceptionLog = new()
		{
			EncounteredAt = DateTime.Now,
			Message = ex.Message,
			StackTrace = ex.StackTrace,
			Source = ex.Source,
			TypeOf = ex.GetType().ToString(),
			Path = requestModel?.Path,
			Method = requestModel?.Method,
			QueryString = requestModel?.QueryString,
			RequestBody = requestModel?.RequestBody
		};

		// Insert
		await DynamicDBManager.InsertWatchExceptionLog(watchExceptionLog);
		await _broadcastHelper.BroadcastExLog(watchExceptionLog);
	}
}
