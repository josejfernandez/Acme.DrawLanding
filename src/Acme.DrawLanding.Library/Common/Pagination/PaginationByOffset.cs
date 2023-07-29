namespace Acme.DrawLanding.Library.Common.Pagination;

public readonly struct PaginationByOffset
{
    public int PageNumber { get; }

    public int PageSize { get; }

    public int Offset => (PageNumber - 1) * PageSize;

    public PaginationByOffset(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), $"Page numbers range from 1 to {int.MaxValue}.");
        }

        if (pageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), $"Page size ranges from 1 to {int.MaxValue}.");
        }

        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
