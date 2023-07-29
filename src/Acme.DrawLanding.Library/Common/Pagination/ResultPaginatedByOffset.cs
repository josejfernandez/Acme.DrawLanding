namespace Acme.DrawLanding.Library.Common.Pagination;

public sealed class ResultPaginatedByOffset<TResult>
{
    /// <summary>
    /// Gets the total number of items in the database, ignoring pagination.
    /// </summary>
    public int TotalItems { get; }

    /// <summary>
    /// Gets the total number of pages, based on the total number of items and the given page size.
    /// </summary>
    public int TotalPages => (int)Math.Abs(Math.Ceiling((double)TotalItems / Pagination.PageSize));

    public IReadOnlyCollection<TResult> Items { get; }

    public PaginationByOffset Pagination { get; }

    public ResultPaginatedByOffset(IReadOnlyCollection<TResult> items, PaginationByOffset pagination, int totalHits)
    {
        if (totalHits < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalHits));
        }

        Items = items ?? throw new ArgumentNullException(nameof(items));
        Pagination = pagination;
        TotalItems = totalHits;
    }
}
