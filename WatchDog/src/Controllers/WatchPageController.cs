namespace WatchDog.src.Controllers;

using global::WatchDog.src.Helpers;
using global::WatchDog.src.Managers;
using global::WatchDog.src.Models;
using global::WatchDog.src.Utilities;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Class WatchPageController.
/// Implements the <see cref="Controller" />
/// </summary>
/// <seealso cref="Controller" />
public class WatchPageController : Controller
{
	/// <summary>
	/// Authentications the specified <see paramref="username"/>.
	/// </summary>
	/// <param name="username">The username.</param>
	/// <param name="password">The password.</param>
	/// <returns>The <see cref="JsonResult"/>.</returns>
	[HttpPost]
	public JsonResult Auth(string username, string password) => Json(username.ToLower() == WatchDogConfigModel.UserName.ToLower() && password == WatchDogConfigModel.Password);

	/// <summary>
	/// Clears the logs.
	/// </summary>
	/// <returns>The <see cref="JsonResult"/>.</returns>
	public async Task<JsonResult> ClearLogs()
	{
		var cleared = await DynamicDBManager.ClearLogs();
		return Json(cleared);
	}

	/// <summary>
	/// Returns the page of exceptions for the specified search string.
	/// </summary>
	/// <param name="searchString">The search string.</param>
	/// <param name="pageNumber">The page number.</param>
	/// <returns>The <see cref="JsonResult"/>.</returns>
	public async Task<JsonResult> Exceptions(string searchString = "", int pageNumber = 1)
	{
		var logs = await DynamicDBManager.GetAllWatchExceptionLogs();
		if (logs is not null && !string.IsNullOrEmpty(searchString))
		{
			searchString = searchString.ToLower();
			logs = logs.Where(l => l.Message.ToLower().Contains(searchString) || l.StackTrace.ToLower().Contains(searchString) || l.Source.ToString().Contains(searchString));
		}

		logs = logs?.OrderByDescending(x => x.EncounteredAt) ?? Enumerable.Empty<WatchExceptionLog>();
		var result = PaginatedList<WatchExceptionLog>.CreateAsync(logs, pageNumber, Constants.PageSize);
		return Json(new { result.PageIndex, result.TotalPages, HasNext = result.HasNextPage, HasPrevious = result.HasPreviousPage, logs = result });
	}

	/// <summary>
	/// Returns the page of logs for the specified search string.
	/// </summary>
	/// <param name="searchString">The search string.</param>
	/// <param name="verbString">The verb string.</param>
	/// <param name="statusCode">The status code.</param>
	/// <param name="pageNumber">The page number.</param>
	/// <returns>The <see cref="JsonResult"/>.</returns>
	public async Task<JsonResult> Index(string searchString = "", string verbString = "", string statusCode = "", int pageNumber = 1)
	{
		var logs = await DynamicDBManager.GetAllWatchLogs();
		if (logs is not null)
		{
			if (!string.IsNullOrEmpty(searchString))
			{
				searchString = searchString.ToLower();
				logs = logs.Where(l => l.Path.ToLower().Contains(searchString) || l.Method.ToLower().Contains(searchString) || l.ResponseStatus.ToString().Contains(searchString) || (!string.IsNullOrEmpty(l.QueryString) && l.QueryString.ToLower().Contains(searchString)));
			}

			if (!string.IsNullOrEmpty(verbString))
			{
				logs = logs.Where(l => l.Method.ToLower() == verbString.ToLower());
			}

			if (!string.IsNullOrEmpty(statusCode))
			{
				logs = logs.Where(l => l.ResponseStatus.ToString() == statusCode);
			}
		}

		logs = logs?.OrderByDescending(x => x.StartTime) ?? Enumerable.Empty<WatchLog>();
		var result = PaginatedList<WatchLog>.CreateAsync(logs, pageNumber, Constants.PageSize);
		return Json(new { result.PageIndex, result.TotalPages, HasNext = result.HasNextPage, HasPrevious = result.HasPreviousPage, logs = result });
	}

	/// <summary>
	/// Returns the page of logs for the specified search string.
	/// </summary>
	/// <param name="searchString">The search string.</param>
	/// <param name="pageNumber">The page number.</param>
	/// <returns>The <see cref="JsonResult"/>.</returns>
	public async Task<JsonResult> Logs(string searchString = "", int pageNumber = 1)
	{
		var logs = await DynamicDBManager.GetAllLogs();
		if (logs is not null && !string.IsNullOrEmpty(searchString))
		{
			searchString = searchString.ToLower();
			logs = logs.Where(l => l.Message.ToLower().Contains(searchString) || l.CallingMethod.ToLower().Contains(searchString) || l.CallingFrom.ToString().Contains(searchString));
		}

		logs = logs?.OrderByDescending(x => x.Timestamp) ?? Enumerable.Empty<WatchLoggerModel>();
		var result = PaginatedList<WatchLoggerModel>.CreateAsync(logs, pageNumber, Constants.PageSize);
		return Json(new { result.PageIndex, result.TotalPages, HasNext = result.HasNextPage, HasPrevious = result.HasPreviousPage, logs = result });
	}
}
