namespace WatchDog.src.Models;

/// <summary>
/// The request model class.
/// </summary>
public class RequestModel
{
	/// <summary>
	/// Gets or sets the headers.
	/// </summary>
	/// <value>The headers.</value>
	public string? Headers { get; set; }

	/// <summary>
	/// Gets or sets the host.
	/// </summary>
	/// <value>The host.</value>
	public string? Host { get; set; }

	/// <summary>
	/// Gets or sets the IP address.
	/// </summary>
	/// <value>The IP address.</value>
	public string? IpAddress { get; set; }

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
	/// Gets or sets the start time.
	/// </summary>
	/// <value>The start time.</value>
	public DateTime StartTime { get; set; }
}
