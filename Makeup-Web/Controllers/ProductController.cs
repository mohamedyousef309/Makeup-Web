using Microsoft.AspNetCore.Mvc;

namespace Makeup_Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
