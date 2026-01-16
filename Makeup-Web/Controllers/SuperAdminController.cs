using Application_Layer.CQRS.User.Commands.MakeAdmine;
using Application_Layer.CQRS.User.Commands.RemoveUserRoleAdmin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Makeup_Web.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly IMediator mediator;

        public SuperAdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> GiveUserRoleAdmin([FromForm]int userId)
        {
            var result = await mediator.Send(new MakeUserAdmineOrchestrator(userId));
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return Json(new { success = false, message = result.Message });

            }
            else
            {
                TempData["SuccessMessage"] = "User promoted to Admin successfully.";
                return Json(new { success = true, message = "User has been blocked successfully!" });

            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveUserRoleAdmin([FromForm]int userId)
        {
            var result = await mediator.Send(new RemoveUserRoleAdminCommand(userId));
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return Json(new { success = false, message = result.Message });

            }
            else
            {
                TempData["SuccessMessage"] = "User promoted to Admin successfully.";
                return Json(new { success = true, message = "User has been blocked successfully!" });

            }
        }
    }
}
