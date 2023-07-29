using Microsoft.AspNetCore.Mvc;

namespace Acme.DrawLanding.Website.Controllers;

public sealed class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
