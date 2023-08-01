using Acme.DrawLanding.Library.Common.Pagination;
using Acme.DrawLanding.Library.Domain.SerialNumbers;
using Acme.DrawLanding.Website.Domain.SerialNumbers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Acme.DrawLanding.Website.Areas.Admin.Controllers;

[Area("Admin")]
public sealed class SerialNumbersController : Controller
{
    private readonly ISerialNumberRepository _serialNumberRepository;

    public SerialNumbersController(ISerialNumberRepository serialNumberRepository)
    {
        _serialNumberRepository = serialNumberRepository ?? throw new ArgumentNullException(nameof(serialNumberRepository));
    }

    [HttpGet("/admin/serialnumbers")]
    [Authorize]
    public async Task<IActionResult> Index([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _serialNumberRepository.GetPagedSerialNumbers(new PaginationByOffset(pageNumber, pageSize));

        return View(result);
    }

    [HttpPost("/admin/serialnumbers/create")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync([FromForm] CreateSerialNumbersRequest request)
    {
        await _serialNumberRepository.CreateNew(request.HowMany);

        return Redirect("/admin/serialnumbers");
    }
}
