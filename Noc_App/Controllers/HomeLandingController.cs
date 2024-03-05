using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Noc_App.Controllers
{
    [AllowAnonymous]
    public class HomeLandingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
