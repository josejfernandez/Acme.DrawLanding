using Acme.DrawLanding.Library.Common.Pagination;
using Acme.DrawLanding.Library.Domain.Submissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Acme.DrawLanding.Website.Areas.Admin.Controllers;

[Area("Admin")]
public sealed class SubmissionsController : Controller
{
    private readonly ISubmissionRepository _submissionRepository;

    public SubmissionsController(ISubmissionRepository submissionRepository)
    {
        _submissionRepository = submissionRepository ?? throw new ArgumentNullException(nameof(submissionRepository));
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _submissionRepository.GetPagedSubmissions(new PaginationByOffset(pageNumber, pageSize));

        return View(result);
    }
}
