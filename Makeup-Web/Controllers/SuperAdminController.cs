using Application_Layer.CQRS.Permission.Command.GiveUserPermission;
using Application_Layer.CQRS.Permission.Command.RemoveUserPerrmission;
using Application_Layer.CQRS.Permission.Queries.GetAllPermissions;
using Application_Layer.CQRS.User.Commands.MakeAdmine;
using Application_Layer.CQRS.User.Commands.RemoveUserRoleAdmin;
using Domain_Layer.ViewModels.Permissions;
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

        [HttpGet]
        public async Task<IActionResult> GetAllPermission()
        {
            var GetPermissionsResult = await mediator.Send(new GetAllPermissionsQuery());

            if (!GetPermissionsResult.IsSuccess)
            {
                TempData["ErrorMessage"] = GetPermissionsResult.Message;
                return Json(new { success = false, message = GetPermissionsResult.Message });
            }
            return Json(new { success = true, data = GetPermissionsResult.Data });
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

        [HttpPost]
        public async Task<IActionResult> AddPermssionsTouser([FromBody]AddPermissionToUserViewModel Modle)
        {
            var RemoveUserPermissionResult = await mediator.Send(new GiveUserPermissionOrechestrator(Modle.UserId,Modle.PermissionIds));
            if (!RemoveUserPermissionResult.IsSuccess)
            {
                return Json(new { success = false, message = RemoveUserPermissionResult });
            }
            return Json(new { success = true, message = RemoveUserPermissionResult.Message });
        }

        [HttpPost]

        public async Task<IActionResult>RemoveUserPermission(RemoveUserPermissionViewModle Modle) 
        {
            if (!ModelState.IsValid)
            {
               var errors = ModelState.Where(x => x.Value.Errors.Any()).SelectMany(x=>x.Value.Errors).
                    Select(x=>x.ErrorMessage).ToList();

                return Json(new
                {
                    success = false,
                    errors
                });

            }

            var RemoveUserPermissionResult = await mediator.Send(new RemoveUserPermissionCommand(Modle.UserId, Modle.PermissionId));

            if (!RemoveUserPermissionResult.IsSuccess)
            {
                return Json(new { success = false, message = RemoveUserPermissionResult });
            }

            return Json(new { success = true, message = RemoveUserPermissionResult.Message });
        }
    }
}
