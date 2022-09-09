namespace WatchDog.src.Helpers;

internal class PaginatedList<T> : List<T>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PaginatedList{T}"/> class.
	/// </summary>
	/// <param name="items">The items.</param>
	/// <param name="count">The count.</param>
	/// <param name="pageIndex">Index of the page.</param>
	/// <param name="pageSize">Size of the page.</param>
	public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
	{
		PageIndex = pageIndex;
		TotalPages = (int)Math.Ceiling(count / (double)pageSize);

		this.AddRange(items);
	}

	/// <summary>
	/// Gets a value indicating whether this instance has next page.
	/// </summary>
	/// <value><c>true</c> if this instance has next page; otherwise, <c>false</c>.</value>
	public bool HasNextPage => PageIndex < TotalPages;

	/// <summary>
	/// Gets a value indicating whether this instance has previous page.
	/// </summary>
	/// <value><c>true</c> if this instance has previous page; otherwise, <c>false</c>.</value>
	public bool HasPreviousPage => PageIndex > 1;

	/// <summary>
	/// Gets the index of the page.
	/// </summary>
	/// <value>The index of the page.</value>
	public int PageIndex { get; private set; }

	/// <summary>
	/// Gets the total pages.
	/// </summary>
	/// <value>The total pages.</value>
	public int TotalPages { get; private set; }

	/// <summary>
	/// Creates a paginated list from the specified source.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="pageIndex">Index of the page.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <returns>PaginatedList&lt;T&gt;.</returns>
	public static PaginatedList<T> CreateAsync(IEnumerable<T> source, int pageIndex, int pageSize)
	{
		var count = source.Count();
		var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
		return new PaginatedList<T>(items, count, pageIndex, pageSize);
	}
}
