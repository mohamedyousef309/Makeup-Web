using Microsoft.AspNetCore.Mvc;

namespace Makeup_Web.Controllers
{
    public class basketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateOrUpdateBasket() 
        {
            return View();
        }
    }
}
