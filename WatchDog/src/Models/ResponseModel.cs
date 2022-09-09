namespace WatchDog.src.Models;

/// <summary>
/// The response model class.
/// </summary>
public class ResponseModel
{
	/// <summary>
	/// Gets or sets the finish time.
	/// </summary>
	/// <value>The finish time.</value>
	public DateTime FinishTime { get; set; }

	/// <summary>
	/// Gets or sets the headers.
	/// </summary>
	/// <value>The headers.</value>
	public string? Headers { get; set; }

	/// <summary>
	/// Gets or sets the response body.
	/// </summary>
	/// <value>The response body.</value>
	public string? ResponseBody { get; set; }

	/// <summary>
	/// Gets or sets the response status.
	/// </summary>
	/// <value>The response status.</value>
	public int ResponseStatus { get; set; }
}
