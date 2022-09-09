namespace WatchDog.src.Models;

/// <summary>
/// The watch exception log class.
/// </summary>
public class WatchExceptionLog
{
	/// <summary>
	/// Gets or sets the encountered at.
	/// </summary>
	/// <value>The encountered at.</value>
	public DateTime EncounteredAt { get; set; }

	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>The identifier.</value>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the message.
	/// </summary>
	/// <value>The message.</value>
	public string? Message { get; set; }

	/// <summary>
	/// Gets or sets the method.
	/// </summary>
	/// <value>The method.</value>
	public string? Method { get; set; }

	/// <summary>
	/// Gets or sets the path.
	/// </summary>
	/// <value>The path.</value>
	public string? Path { get; set; }

	/// <summary>
	/// Gets or sets the query string.
	/// </summary>
	/// <value>The query string.</value>
	public string? QueryString { get; set; }

	/// <summary>
	/// Gets or sets the request body.
	/// </summary>
	/// <value>The request body.</value>
	public string? RequestBody { get; set; }

	/// <summary>
	/// Gets or sets the source.
	/// </summary>
	/// <value>The source.</value>
	public string? Source { get; set; }

	/// <summary>
	/// Gets or sets the stack trace.
	/// </summary>
	/// <value>The stack trace.</value>
	public string? StackTrace { get; set; }

	/// <summary>
	/// Gets or sets the type of.
	/// </summary>
	/// <value>The type of.</value>
	public string? TypeOf { get; set; }
}
