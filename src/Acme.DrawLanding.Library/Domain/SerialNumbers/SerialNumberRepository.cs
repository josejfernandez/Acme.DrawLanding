using Acme.DrawLanding.Library.Common.Pagination;
using Acme.DrawLanding.Library.Data;
using Microsoft.EntityFrameworkCore;

namespace Acme.DrawLanding.Library.Domain.SerialNumbers;

public sealed class SerialNumberRepository : ISerialNumberRepository
{
    private readonly AppDbContext _context;

    public SerialNumberRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<ResultPaginatedByOffset<SerialNumber>> GetPagedSerialNumbers(PaginationByOffset pagination)
    {
        var query = _context.SerialNumbers
            .OrderBy(x => x.Id);

        int hits = await query.CountAsync();

        var items = await query
            .Skip(pagination.Offset)
            .Take(pagination.PageSize)
            .Select(x => new SerialNumber()
            {
                Number = x.Content,
                Uses = x.Uses,
            })
            .ToListAsync();

        return new ResultPaginatedByOffset<SerialNumber>(items.AsReadOnly(), pagination, hits);
    }

    public async Task CreateNew(int howMany)
    {
        var serialNumbers = new List<SerialNumberRecord>();

        for (var i = 0; i < howMany; i++)
        {
            serialNumbers.Add(new SerialNumberRecord()
            {
                Content = Guid.NewGuid(),
            });
        }

        await _context.AddRangeAsync(serialNumbers);

        await _context.SaveChangesAsync();
    }
}
