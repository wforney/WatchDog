namespace WatchDog.src.Models;

/// <summary>
/// The watch logger model class.
/// </summary>
public class WatchLoggerModel
{
	/// <summary>
	/// Gets or sets the calling from.
	/// </summary>
	/// <value>The calling from.</value>
	public string? CallingFrom { get; set; }

	/// <summary>
	/// Gets or sets the calling method.
	/// </summary>
	/// <value>The calling method.</value>
	public string? CallingMethod { get; set; }

	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>The identifier.</value>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the line number.
	/// </summary>
	/// <value>The line number.</value>
	public int LineNumber { get; set; }

	/// <summary>
	/// Gets or sets the message.
	/// </summary>
	/// <value>The message.</value>
	public string? Message { get; set; }

	/// <summary>
	/// Gets or sets the timestamp.
	/// </summary>
	/// <value>The timestamp.</value>
	public DateTime Timestamp { get; set; }
}
