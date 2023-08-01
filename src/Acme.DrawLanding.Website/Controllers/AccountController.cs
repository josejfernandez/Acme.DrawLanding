using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Acme.DrawLanding.Library.Domain.Users;
using Acme.DrawLanding.Website.Domain.Submissions;
using Acme.DrawLanding.Website.Domain.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Acme.DrawLanding.Website.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] UserLoginRequest request)
    {
        ViewData["ReturnUrl"] = request.ReturnUrl;

        var validLogin = await _userService.ValidateCredentialsAsync(request.Username, request.Password);

        if (!validLogin)
        {
            ModelState.AddModelError(nameof(UserLoginRequest.Username), string.Empty);
            ModelState.AddModelError(nameof(UserLoginRequest.Password), Messages.InvalidCredentials);

            return View();
        }

        await CreateAndStoreClaims(request.Username);

        if (Url.IsLocalUrl(request.ReturnUrl))
        {
            return Redirect(request.ReturnUrl);
        }
        else
        {
            return Redirect("/");
        }
    }

    [HttpGet]
    public IActionResult AccessDenied([FromQuery] string? returnUrl = null)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    private async Task CreateAndStoreClaims(string username)
    {
        var claims = new List<Claim>()
        {
            new Claim("user", username),
            new Claim("role", "Member")
        };

        await HttpContext.SignInAsync(
            new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role")));
    }
}
