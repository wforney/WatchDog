namespace WatchDog.src.Services;

using global::WatchDog.src.Helpers;
using global::WatchDog.src.Interfaces;
using global::WatchDog.src.Models;

internal class LoggerService : ILoggerService
{
	/// <summary>
	/// Clears the watch logs.
	/// </summary>
	public void ClearWatchLogs()
	{
		if (AutoClearModel.IsAutoClear)
		{
			_ = LiteDBHelper.ClearWatchLog();
			_ = LiteDBHelper.ClearWatchExceptionLog();
		}
	}
}
