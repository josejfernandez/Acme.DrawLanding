using Acme.DrawLanding.Library.Common.Pagination;
using Acme.DrawLanding.Library.Data;
using Acme.DrawLanding.Library.Domain.SerialNumbers;
using Microsoft.EntityFrameworkCore;

namespace Acme.DrawLanding.Library.Domain.Submissions;

public sealed class SubmissionRepository : ISubmissionRepository
{
    private readonly AppDbContext _context;

    public SubmissionRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<ResultPaginatedByOffset<Submission>> GetPagedSubmissions(PaginationByOffset pagination)
    {
        var query = _context.Submissions
            .Include(x => x.SerialNumber)
            .OrderBy(x => x.Id);

        int hits = await query.CountAsync();

        var items = await query
            .Skip(pagination.Offset)
            .Take(pagination.PageSize)
            .Select(x => new Submission()
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                SerialNumber = x.SerialNumber.Content,
            })
            .ToListAsync();

        return new ResultPaginatedByOffset<Submission>(items.AsReadOnly(), pagination, hits);
    }

    public async Task<SubmissionInsertionResult> InsertIfNotUsedMoreThan(Submission submission, int allowedNumberOfUses)
    {
        ValidateNumberOfUses(allowedNumberOfUses);

        using var transaction = _context.Database.BeginTransaction();

        var serialNumber = await _context.SerialNumbers
            .Where(x => x.Content == submission.SerialNumber)
            .FirstOrDefaultAsync();

        if (serialNumber == null)
        {
            return SubmissionInsertionResult.DoesNotExist;
        }

        if (serialNumber.Uses >= allowedNumberOfUses)
        {
            return SubmissionInsertionResult.AlreadyUsed;
        }

        serialNumber.Uses++;

        var record = CreateSubmissionRecord(submission, serialNumber);

        await _context.Submissions.AddAsync(record);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return SubmissionInsertionResult.Ok(allowedNumberOfUses -  serialNumber.Uses);
    }

    private void ValidateNumberOfUses(int allowedNumberOfUses)
    {
        if (allowedNumberOfUses <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(allowedNumberOfUses), "Allowed number of uses must be greater than 0.");
        }
    }

    private SubmissionRecord CreateSubmissionRecord(Submission submission, SerialNumberRecord serialNumber)
    {
        return new SubmissionRecord()
        {
            FirstName = submission.FirstName,
            LastName = submission.LastName,
            Email = submission.Email,
            SerialNumberId = serialNumber.Id,
        };
    }
}
