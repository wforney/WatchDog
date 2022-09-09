namespace WatchDog.src.Models;

/// <summary>
/// The watch dog options model class.
/// </summary>
public class WatchDogOptionsModel
{
	/// <summary>
	/// Gets or sets the blacklist.
	/// </summary>
	/// <value>The blacklist.</value>
	public string? Blacklist { get; set; }

	/// <summary>
	/// Gets or sets the watch page password.
	/// </summary>
	/// <value>The watch page password.</value>
	public string? WatchPagePassword { get; set; }

	/// <summary>
	/// Gets or sets the watch page username.
	/// </summary>
	/// <value>The watch page username.</value>
	public string? WatchPageUsername { get; set; }
}
