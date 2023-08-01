using Acme.DrawLanding.Library.Domain.Users;
using Acme.DrawLanding.Website.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Acme.DrawLanding.Website.Areas.Admin.Controllers;

[Area("Admin")]
public sealed class UsersController : Controller
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpGet("/admin/users")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("/admin/users")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexAsync([FromForm] CreateUserRequest request)
    {
        await _userService.CreateUserAsync(request.Username, request.Password);

        return Redirect("/");
    }
}
