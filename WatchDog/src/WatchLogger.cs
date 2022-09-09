namespace WatchDog;

using WatchDog.src.Managers;
using WatchDog.src.Models;

/// <summary>
/// The watch logger class.
/// </summary>
public class WatchLogger
{
    /// <summary>
    /// Logs the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="callerName">Name of the caller.</param>
    /// <param name="filePath">The file path.</param>
    /// <param name="lineNumber">The line number.</param>
    public static async void Log(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
    {
        var log = new WatchLoggerModel
        {
            Message = message,
            Timestamp = DateTime.Now,
            CallingFrom = Path.GetFileName(filePath),
            CallingMethod = callerName,
            LineNumber = lineNumber,
        };

        // Insert
        await DynamicDBManager.InsertLog(log);
        await ServiceProviderFactory.BroadcastHelper.BroadcastLog(log);
    }
}
