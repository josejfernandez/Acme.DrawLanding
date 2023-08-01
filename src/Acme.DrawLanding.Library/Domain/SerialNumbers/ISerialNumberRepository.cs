using Acme.DrawLanding.Library.Common.Pagination;

namespace Acme.DrawLanding.Library.Domain.SerialNumbers;

public interface ISerialNumberRepository
{
    public Task<ResultPaginatedByOffset<SerialNumber>> GetPagedSerialNumbers(PaginationByOffset pagination);

    public Task CreateNew(int howMany);
}
