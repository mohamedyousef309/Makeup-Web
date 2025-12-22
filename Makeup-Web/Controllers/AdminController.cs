using Microsoft.AspNetCore.Mvc;

namespace Makeup_Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
