using Microsoft.AspNetCore.Mvc;

namespace Makeup_Web.Controllers
{
    public class SuperAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
