namespace WatchDog.src;

using global::WatchDog.src.Helpers;
using global::WatchDog.src.Interfaces;
using global::WatchDog.src.Managers;
using global::WatchDog.src.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IO;

internal class WatchDog
{
	/// <summary>
	/// The request log
	/// </summary>
	public static RequestModel? RequestLog;

	private readonly IBroadcastHelper _broadcastHelper;
	private readonly RequestDelegate _next;
	private readonly WatchDogOptionsModel _options;
	private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

	/// <summary>
	/// Initializes a new instance of the <see cref="WatchDog"/> class.
	/// </summary>
	/// <param name="options">The options.</param>
	/// <param name="next">The next request delegate.</param>
	/// <param name="broadcastHelper">The broadcast helper.</param>
	public WatchDog(WatchDogOptionsModel options, RequestDelegate next, IBroadcastHelper broadcastHelper)
	{
		_next = next;
		_options = options;
		_recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
		_broadcastHelper = broadcastHelper;

		WatchDogConfigModel.UserName = _options.WatchPageUsername;
		WatchDogConfigModel.Password = _options.WatchPagePassword;
		WatchDogConfigModel.Blacklist = string.IsNullOrEmpty(_options.Blacklist) ? Array.Empty<string>() : _options.Blacklist.Replace(" ", string.Empty).Split(',');
	}

	/// <summary>
	/// Invoke as an asynchronous operation.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	public async Task InvokeAsync(HttpContext context)
	{
		var watchLog = new WatchLog();

		if (!context.Request.Path.ToString().Contains("WTCHDwatchpage") &&
			!context.Request.Path.ToString().Contains("watchdog") &&
			!context.Request.Path.ToString().Contains("WTCHDGstatics") &&
			!context.Request.Path.ToString().Contains("favicon") &&
			!context.Request.Path.ToString().Contains("wtchdlogger") &&
			!WatchDogConfigModel.Blacklist.Contains(context.Request.Path.ToString().Remove(0, 1), StringComparer.OrdinalIgnoreCase))
		{
			// Request handling comes here
			var requestLog = await LogRequest(context);
			var responseLog = await LogResponse(context);

			var timeSpent = responseLog.FinishTime.Subtract(requestLog.StartTime);
			// Build General WatchLog, Join from requestLog and responseLog

			watchLog = new WatchLog
			{
				IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
				ResponseStatus = responseLog.ResponseStatus,
				QueryString = requestLog.QueryString ?? string.Empty,
				Method = requestLog.Method ?? string.Empty,
				Path = requestLog.Path ?? string.Empty,
				Host = requestLog.Host ?? string.Empty,
				RequestBody = requestLog.RequestBody ?? string.Empty,
				ResponseBody = responseLog.ResponseBody,
				TimeSpent = $"{timeSpent.Hours:D1} hrs {timeSpent.Minutes:D1} mins {timeSpent.Seconds:D1} secs {timeSpent.Milliseconds:D1} ms",
				RequestHeaders = requestLog.Headers ?? string.Empty,
				ResponseHeaders = responseLog.Headers,
				StartTime = requestLog.StartTime,
				EndTime = responseLog.FinishTime
			};

			await DynamicDBManager.InsertWatchLog(watchLog);
			await _broadcastHelper.BroadcastWatchLog(watchLog);
		}
		else
		{
			await _next.Invoke(context);
		}
	}

	private async Task<RequestModel> LogRequest(HttpContext context)
	{
		var startTime = DateTime.Now;
		List<string> requestHeaders = new();

		RequestModel requestBodyDto = new()
		{
			RequestBody = string.Empty,
			Host = context.Request.Host.ToString(),
			Path = context.Request.Path.ToString(),
			Method = context.Request.Method.ToString(),
			QueryString = context.Request.QueryString.ToString(),
			StartTime = startTime,
			Headers = context.Request.Headers.Select(x => x.ToString()).Aggregate((a, b) => $"{a}: {b}"),
		};

		if (context.Request.ContentLength > 1)
		{
			context.Request.EnableBuffering();
			await using var requestStream = _recyclableMemoryStreamManager.GetStream();
			await context.Request.Body.CopyToAsync(requestStream);
			requestBodyDto.RequestBody = GeneralHelper.ReadStreamInChunks(requestStream);
			context.Request.Body.Position = 0;
		}

		RequestLog = requestBodyDto;
		return requestBodyDto;
	}

	private async Task<ResponseModel> LogResponse(HttpContext context)
	{
		var responseBody = string.Empty;
		using var originalBodyStream = context.Response.Body;
		try
		{
			using var originalResponseBody = _recyclableMemoryStreamManager.GetStream();
			context.Response.Body = originalResponseBody;
			await _next(context);
			_ = context.Response.Body.Seek(0, SeekOrigin.Begin);
			responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
			_ = context.Response.Body.Seek(0, SeekOrigin.Begin);
			ResponseModel responseBodyDto = new()
			{
				ResponseBody = responseBody,
				ResponseStatus = context.Response.StatusCode,
				FinishTime = DateTime.Now,
				Headers = context.Response.StatusCode is not 200 and not 201
					? context.Response.Headers.Select(x => x.ToString()).Aggregate((a, b) => $"{a}: {b}")
					: string.Empty,
			};
			await originalResponseBody.CopyToAsync(originalBodyStream);
			return responseBodyDto;
		}
		finally
		{
			context.Response.Body = originalBodyStream;
		}
	}
}
