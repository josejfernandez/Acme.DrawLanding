using Acme.DrawLanding.Library.Common.Pagination;

namespace Acme.DrawLanding.Library.Domain.Submissions;

public interface ISubmissionRepository
{
    public Task<ResultPaginatedByOffset<Submission>> GetPagedSubmissions(PaginationByOffset pagination);

    public Task<SubmissionInsertionResult> InsertIfNotUsedMoreThan(Submission submission, int allowedNumberOfUses);
}
