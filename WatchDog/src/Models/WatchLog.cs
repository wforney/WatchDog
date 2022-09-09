namespace WatchDog.src.Models;

/// <summary>
/// The watch log class.
/// </summary>
public class WatchLog
{
	/// <summary>
	/// Gets or sets the end time.
	/// </summary>
	/// <value>The end time.</value>
	public DateTime EndTime { get; set; }

	/// <summary>
	/// Gets or sets the host.
	/// </summary>
	/// <value>The host.</value>
	public string? Host { get; set; }

	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>The identifier.</value>
	public int Id { get; set; }

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
	/// Gets or sets the request headers.
	/// </summary>
	/// <value>The request headers.</value>
	public string? RequestHeaders { get; set; }

	/// <summary>
	/// Gets or sets the response body.
	/// </summary>
	/// <value>The response body.</value>
	public string? ResponseBody { get; set; }

	/// <summary>
	/// Gets or sets the response headers.
	/// </summary>
	/// <value>The response headers.</value>
	public string? ResponseHeaders { get; set; }

	/// <summary>
	/// Gets or sets the response status.
	/// </summary>
	/// <value>The response status.</value>
	public int ResponseStatus { get; set; }

	/// <summary>
	/// Gets or sets the start time.
	/// </summary>
	/// <value>The start time.</value>
	public DateTime StartTime { get; set; }

	/// <summary>
	/// Gets or sets the time spent.
	/// </summary>
	/// <value>The time spent.</value>
	public string? TimeSpent { get; set; }
}
