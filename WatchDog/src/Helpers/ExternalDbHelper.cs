namespace WatchDog.src.Helpers;

using Dapper;
using global::WatchDog.src.Data;
using global::WatchDog.src.Models;
using global::WatchDog.src.Utilities;
using System.Data;

internal static class ExternalDbHelper
{
	/// <summary>
	/// Clears the logs.
	/// </summary>
	/// <returns><c>true</c> if any logs were cleared, <c>false</c> otherwise.</returns>
	public static async Task<bool> ClearLogs()
	{
		using var connection = ExternalDbContext.CreateConnection();
		var watchlogs = await connection.ExecuteAsync($"truncate table {Constants.WatchLogTableName}");
		var exLogs = await connection.ExecuteAsync($"truncate table {Constants.WatchLogExceptionTableName}");
		var logs = await connection.ExecuteAsync($"truncate table {Constants.LogsTableName}");
		return watchlogs > 1 && exLogs > 1 && logs > 1;
	}

	/// <summary>
	/// Gets all logs.
	/// </summary>
	/// <returns>IEnumerable&lt;WatchLoggerModel&gt;.</returns>
	public static async Task<IEnumerable<WatchLoggerModel>> GetAllLogs()
	{
		using var connection = ExternalDbContext.CreateConnection();
		connection.Open();
		var logs = await connection.QueryAsync<WatchLoggerModel>($"SELECT * FROM {Constants.LogsTableName}");
		connection.Close();
		return logs.AsList();
	}

	/// <summary>
	/// Gets all watch exception logs.
	/// </summary>
	/// <returns>IEnumerable&lt;WatchExceptionLog&gt;.</returns>
	public static async Task<IEnumerable<WatchExceptionLog>> GetAllWatchExceptionLogs()
	{
		using var connection = ExternalDbContext.CreateConnection();
		var logs = await connection.QueryAsync<WatchExceptionLog>($"SELECT * FROM {Constants.WatchLogExceptionTableName}");
		return logs.AsList();
	}

	/// <summary>
	/// Gets all watch logs.
	/// </summary>
	/// <returns>IEnumerable&lt;WatchLog&gt;.</returns>
	public static async Task<IEnumerable<WatchLog>> GetAllWatchLogs()
	{
		using var connection = ExternalDbContext.CreateConnection();
		connection.Open();
		var logs = await connection.QueryAsync<WatchLog>($"SELECT * FROM {Constants.WatchLogTableName}");
		connection.Close();
		return logs.AsList();
	}

	/// <summary>
	/// Inserts the log.
	/// </summary>
	/// <param name="log">The log.</param>
	public static async Task InsertLog(WatchLoggerModel log)
	{
		string query = new StringBuilder()
			.Append("INSERT INTO ")
			.Append(Constants.LogsTableName)
			.Append(" (message,timestamp,callingFrom,callingMethod,lineNumber) ")
			.Append("VALUES (@Message,@Timestamp,@CallingFrom,@CallingMethod,@LineNumber);")
			.ToString();

		var parameters = new DynamicParameters();
		parameters.Add("Message", log.Message, DbType.String);
		parameters.Add("CallingFrom", log.CallingFrom, DbType.String);
		parameters.Add("CallingMethod", log.CallingMethod, DbType.String);
		parameters.Add("LineNumber", log.LineNumber, DbType.Int32);

		if (GeneralHelper.IsPostgres())
		{
			parameters.Add("Timestamp", log.Timestamp.ToUniversalTime(), DbType.DateTime);
		}
		else
		{
			parameters.Add("Timestamp", log.Timestamp, DbType.DateTime);
		}

		using var connection = ExternalDbContext.CreateConnection();
		_ = await connection.ExecuteAsync(query, parameters);
	}

	/// <summary>
	/// Inserts the watch exception log.
	/// </summary>
	/// <param name="log">The log.</param>
	public static async Task InsertWatchExceptionLog(WatchExceptionLog log)
	{
		string query = new StringBuilder()
			.Append("INSERT INTO ")
			.Append(Constants.WatchLogExceptionTableName)
			.Append(" (message,stackTrace,typeOf,source,path,method,queryString,requestBody,encounteredAt) ")
			.Append("VALUES (@Message,@StackTrace,@TypeOf,@Source,@Path,@Method,@QueryString,@RequestBody,@EncounteredAt);")
			.ToString();

		var parameters = new DynamicParameters();
		parameters.Add("Message", log.Message, DbType.String);
		parameters.Add("StackTrace", log.StackTrace, DbType.String);
		parameters.Add("TypeOf", log.TypeOf, DbType.String);
		parameters.Add("Source", log.Source, DbType.String);
		parameters.Add("Path", log.Path, DbType.String);
		parameters.Add("Method", log.Method, DbType.String);
		parameters.Add("QueryString", log.QueryString, DbType.String);
		parameters.Add("RequestBody", log.RequestBody, DbType.String);

		if (GeneralHelper.IsPostgres())
		{
			parameters.Add("EncounteredAt", log.EncounteredAt.ToUniversalTime(), DbType.DateTime);
		}
		else
		{
			parameters.Add("EncounteredAt", log.EncounteredAt, DbType.DateTime);
		}

		using var connection = ExternalDbContext.CreateConnection();
		_ = await connection.ExecuteAsync(query, parameters);
	}

	/// <summary>
	/// Inserts the watch log.
	/// </summary>
	/// <param name="log">The log.</param>
	public static async Task InsertWatchLog(WatchLog log)
	{
		string query = new StringBuilder()
			.Append("INSERT INTO ")
			.Append(Constants.WatchLogTableName)
			.Append(" (responseBody,responseStatus,requestBody,queryString,path,requestHeaders,responseHeaders,method,host,ipAddress,timeSpent,startTime,endTime) ")
			.Append("VALUES (@ResponseBody,@ResponseStatus,@RequestBody,@QueryString,@Path,@RequestHeaders,@ResponseHeaders,@Method,@Host,@IpAddress,@TimeSpent,@StartTime,@EndTime);")
			.ToString();

		var parameters = new DynamicParameters();
		parameters.Add("ResponseBody", log.ResponseBody, DbType.String);
		parameters.Add("ResponseStatus", log.ResponseStatus, DbType.Int32);
		parameters.Add("RequestBody", log.RequestBody, DbType.String);
		parameters.Add("QueryString", log.QueryString, DbType.String);
		parameters.Add("Path", log.Path, DbType.String);
		parameters.Add("RequestHeaders", log.RequestHeaders, DbType.String);
		parameters.Add("ResponseHeaders", log.ResponseHeaders, DbType.String);
		parameters.Add("Method", log.Method, DbType.String);
		parameters.Add("Host", log.Host, DbType.String);
		parameters.Add("IpAddress", log.IpAddress, DbType.String);
		parameters.Add("TimeSpent", log.TimeSpent, DbType.String);

		if (GeneralHelper.IsPostgres())
		{
			parameters.Add("StartTime", log.StartTime.ToUniversalTime(), DbType.DateTime);
			parameters.Add("EndTime", log.EndTime.ToUniversalTime(), DbType.DateTime);
		}
		else
		{
			parameters.Add("StartTime", log.StartTime);
			parameters.Add("EndTime", log.EndTime);
		}

		using var connection = ExternalDbContext.CreateConnection();
		connection.Open();
		_ = await connection.ExecuteAsync(query, parameters);
		connection.Close();
	}
}
