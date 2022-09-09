namespace WatchDog.src.Helpers;

using global::WatchDog.src.Models;
using LiteDB;

internal static class LiteDBHelper
{
	/// <summary>
	/// The database
	/// </summary>
	public static LiteDatabase db = new("watchlogs.db");

	private static ILiteCollection<WatchLoggerModel> _logs = db.GetCollection<WatchLoggerModel>("Logs");
	private static ILiteCollection<WatchExceptionLog> _watchExLogs = db.GetCollection<WatchExceptionLog>("WatchExceptionLogs");
	private static ILiteCollection<WatchLog> _watchLogs = db.GetCollection<WatchLog>("WatchLogs");

	/// <summary>
	/// Clears all logs.
	/// </summary>
	/// <returns><c>true</c> if cleared any logs, <c>false</c> otherwise.</returns>
	public static bool ClearAllLogs()
	{
		var watchLogs = ClearWatchLog();
		var exLogs = ClearWatchExceptionLog();
		var logs = ClearLogs();

		return watchLogs > 1 && exLogs > 1 && logs > 1;
	}

	/// <summary>
	/// Clears the logs.
	/// </summary>
	/// <returns>System.Int32.</returns>
	public static int ClearLogs() => _logs.DeleteAll();

	/// <summary>
	/// Clears the watch exception log.
	/// </summary>
	/// <returns>System.Int32.</returns>
	public static int ClearWatchExceptionLog() => _watchExLogs.DeleteAll();

	/// <summary>
	/// Clears the watch log.
	/// </summary>
	/// <returns>System.Int32.</returns>
	public static int ClearWatchLog() => _watchLogs.DeleteAll();

	/// <summary>
	/// Deletes the watch exception log.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
	public static bool DeleteWatchExceptionLog(int id) => _watchExLogs.Delete(id);

	/// <summary>
	/// Deletes the watch log.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
	public static bool DeleteWatchLog(int id) => _watchLogs.Delete(id);

	/// <summary>
	/// Gets all logs.
	/// </summary>
	/// <returns>IEnumerable&lt;WatchLoggerModel&gt;.</returns>
	public static IEnumerable<WatchLoggerModel> GetAllLogs() => _logs.FindAll();

	/// <summary>
	/// Gets all watch exception logs.
	/// </summary>
	/// <returns>IEnumerable&lt;WatchExceptionLog&gt;.</returns>
	public static IEnumerable<WatchExceptionLog> GetAllWatchExceptionLogs() => _watchExLogs.FindAll();

	/// <summary>
	/// Gets all watch logs.
	/// </summary>
	/// <returns>IEnumerable&lt;WatchLog&gt;.</returns>
	public static IEnumerable<WatchLog> GetAllWatchLogs() => _watchLogs.FindAll();

	/// <summary>
	/// Gets the watch exception log by identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns>WatchExceptionLog.</returns>
	public static WatchExceptionLog GetWatchExceptionLogById(int id) => _watchExLogs.FindById(id);

	/// <summary>
	/// Gets the watch log by identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns>WatchLog.</returns>
	public static WatchLog GetWatchLogById(int id) => _watchLogs.FindById(id);

	/// <summary>
	/// Inserts the log.
	/// </summary>
	/// <param name="log">The log.</param>
	/// <returns>System.Int32.</returns>
	public static int InsertLog(WatchLoggerModel log) => _logs.Insert(log);

	/// <summary>
	/// Inserts the watch exception log.
	/// </summary>
	/// <param name="log">The log.</param>
	/// <returns>System.Int32.</returns>
	public static int InsertWatchExceptionLog(WatchExceptionLog log) => _watchExLogs.Insert(log);

	/// <summary>
	/// Inserts the watch log.
	/// </summary>
	/// <param name="log">The log.</param>
	/// <returns>System.Int32.</returns>
	public static int InsertWatchLog(WatchLog log) => _watchLogs.Insert(log);

	/// <summary>
	/// Updates the watch exception log.
	/// </summary>
	/// <param name="log">The log.</param>
	/// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
	public static bool UpdateWatchExceptionLog(WatchExceptionLog log) => _watchExLogs.Update(log);

	/// <summary>
	/// Updates the watch log.
	/// </summary>
	/// <param name="log">The log.</param>
	/// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
	public static bool UpdateWatchLog(WatchLog log) => _watchLogs.Update(log);
}
