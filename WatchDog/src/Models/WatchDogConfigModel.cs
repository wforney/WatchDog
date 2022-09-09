namespace WatchDog.src.Models;

using global::WatchDog.src.Enums;

/// <summary>
/// The watch dog configuration model class.
/// </summary>
public static class WatchDogConfigModel
{
	/// <summary>
	/// Gets or sets the blacklist.
	/// </summary>
	/// <value>The blacklist.</value>
	public static string[]? Blacklist { get; set; }

	/// <summary>
	/// Gets or sets the password.
	/// </summary>
	/// <value>The password.</value>
	public static string? Password { get; set; }

	/// <summary>
	/// Gets or sets the name of the user.
	/// </summary>
	/// <value>The name of the user.</value>
	public static string? UserName { get; set; }
}

/// <summary>
/// The watch dog external database configuration class.
/// </summary>
public static class WatchDogExternalDbConfig
{
	/// <summary>
	/// Gets or sets the connection string.
	/// </summary>
	/// <value>The connection string.</value>
	public static string? ConnectionString { get; set; } = string.Empty;
}

/// <summary>
/// The watch dog SQL driver option class.
/// </summary>
public static class WatchDogSqlDriverOption
{
	/// <summary>
	/// Gets or sets the SQL driver option.
	/// </summary>
	/// <value>The SQL driver option.</value>
	public static WatchDogSqlDriverEnum SqlDriverOption { get; set; }
}

/// <summary>
/// The watch dog settings class.
/// </summary>
public class WatchDogSettings
{
	/// <summary>
	/// Gets or sets the clear time schedule.
	/// </summary>
	/// <value>The clear time schedule.</value>
	public WatchDogAutoClearScheduleEnum ClearTimeSchedule { get; set; } = WatchDogAutoClearScheduleEnum.Weekly;

	/// <summary>
	/// Gets or sets a value indicating whether this instance is automatic clear.
	/// </summary>
	/// <value><c>true</c> if this instance is automatic clear; otherwise, <c>false</c>.</value>
	public bool IsAutoClear { get; set; }

	/// <summary>
	/// Gets or sets the set external database connection string.
	/// </summary>
	/// <value>The set external database connection string.</value>
	public string SetExternalDbConnString { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the SQL driver option.
	/// </summary>
	/// <value>The SQL driver option.</value>
	public WatchDogSqlDriverEnum SqlDriverOption { get; set; }
}

/// <summary>
/// The automatic clearing model class.
/// </summary>
public static class AutoClearModel
{
	/// <summary>
	/// Gets or sets the automatic clearing time schedule.
	/// </summary>
	/// <value>The automatic clearing time schedule.</value>
	public static WatchDogAutoClearScheduleEnum ClearTimeSchedule { get; set; } = WatchDogAutoClearScheduleEnum.Weekly;

	/// <summary>
	/// Gets or sets a value indicating whether this instance has automatic clearing enabled.
	/// </summary>
	/// <value><c>true</c> if this instance has automatic clearing enabled; otherwise, <c>false</c>.</value>
	public static bool IsAutoClear { get; set; }
}
