namespace WatchDog.src.Managers;

using global::WatchDog.src.Helpers;
using global::WatchDog.src.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

internal static class DynamicDBManager
{
	private static string _connectionString = WatchDogExternalDbConfig.ConnectionString;

	/// <summary>
	/// Clears the logs.
	/// </summary>
	/// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
	public static async Task<bool> ClearLogs() =>
		IsExternalDb()
			? await ExternalDbHelper.ClearLogs()
			: LiteDBHelper.ClearAllLogs();

	/// <summary>
	/// Gets all logs.
	/// </summary>
	/// <returns>IEnumerable&lt;WatchLoggerModel&gt;.</returns>
	public static async Task<IEnumerable<WatchLoggerModel>> GetAllLogs() =>
		IsExternalDb()
			? await ExternalDbHelper.GetAllLogs()
			: LiteDBHelper.GetAllLogs();

	/// <summary>
	/// Gets all watch exception logs.
	/// </summary>
	/// <returns>IEnumerable&lt;WatchExceptionLog&gt;.</returns>
	public static async Task<IEnumerable<WatchExceptionLog>> GetAllWatchExceptionLogs() =>
		IsExternalDb()
			? await ExternalDbHelper.GetAllWatchExceptionLogs()
			: LiteDBHelper.GetAllWatchExceptionLogs();

	/// <summary>
	/// Gets all watch logs.
	/// </summary>
	/// <returns>IEnumerable&lt;WatchLog&gt;.</returns>
	public static async Task<IEnumerable<WatchLog>> GetAllWatchLogs() =>
		IsExternalDb()
			? await ExternalDbHelper.GetAllWatchLogs()
			: LiteDBHelper.GetAllWatchLogs();

	/// <summary>
	/// Inserts the log.
	/// </summary>
	/// <param name="log">The log.</param>
	public static async Task InsertLog(WatchLoggerModel log)
	{
		if (IsExternalDb())
		{
			await ExternalDbHelper.InsertLog(log);
		}
		else
		{
			_ = LiteDBHelper.InsertLog(log);
		}
	}

	/// <summary>
	/// Inserts the watch exception log.
	/// </summary>
	/// <param name="log">The log.</param>
	public static async Task InsertWatchExceptionLog(WatchExceptionLog log)
	{
		if (IsExternalDb())
		{
			await ExternalDbHelper.InsertWatchExceptionLog(log);
		}
		else
		{
			_ = LiteDBHelper.InsertWatchExceptionLog(log);
		}
	}

	/// <summary>
	/// Inserts the watch log.
	/// </summary>
	/// <param name="log">The log.</param>
	public static async Task InsertWatchLog(WatchLog log)
	{
		if (IsExternalDb())
		{
			await ExternalDbHelper.InsertWatchLog(log);
		}
		else
		{
			_ = LiteDBHelper.InsertWatchLog(log);
		}
	}

	private static bool IsExternalDb() => !string.IsNullOrEmpty(_connectionString);
}
