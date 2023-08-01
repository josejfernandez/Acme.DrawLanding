using Microsoft.AspNetCore.Mvc;

namespace Acme.DrawLanding.Website.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
