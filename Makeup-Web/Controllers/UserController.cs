using Application_Layer.CQRS.User.Commands.EditUserProfile;
using Application_Layer.CQRS.User.Quries.GetAllUsers;
using Application_Layer.CQRS.User.Quries.GetUserEmailbyUserid;
using AspNetCoreGeneratedDocument;
using Domain_Layer.ViewModels.User;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Makeup_Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllUsers(int? pageIndex, int? pageSize, string? sortBy
            , string? sortDir,
            string? search) 
        {
            var GetAllUsersResult = await mediator.Send(new GetAllUserswithRolsQuery(pageSize ?? 10,
                    pageIndex ?? 1,
                    sortBy ?? "id",
                    sortDir ?? "desc",
                    search));
            if (!GetAllUsersResult.IsSuccess)
            {
                TempData["ErrorMessage"] = GetAllUsersResult.Message;
                return View(GetAllUsersResult.Data);

            }

            return View(GetAllUsersResult.Data);


        }

        public async Task<IActionResult> GetUserbyid() 
        {
            if (!TryGetUserId(out int Userid))
            {
                return RedirectToAction("Login", "Authantication");

            }
            var GetUserResult = await mediator.Send(new GetUserEmailbyUseridQuery(Userid));
            if (!GetUserResult.IsSuccess|| GetUserResult.Data==null)
            {
                ViewBag.rrorMessage = GetUserResult.Message;
                return View();
            }
            return View(GetUserResult.Data);

        }
        public async Task<IActionResult> EditUserProfile(int? userid) 
        {

           
            if (!TryGetUserId(out int loggedInUserId))
            {
                return RedirectToAction("Login", "Authantication");
            }

            bool isAdmin = User.IsInRole("SuperAdmin") || User.IsInRole("Admin");
            int targetUserId = (isAdmin && userid.HasValue && userid != 0) ? userid.Value : loggedInUserId;
            
            var GetUserResult = await mediator.Send(new GetUserEmailbyUseridQuery(targetUserId));

            if (!GetUserResult.IsSuccess)
            {
                TempData["ErrorMessage"] = GetUserResult.Message;
                return View();
            }

            var viewModel = new EditUserViewModle
            {
                Userid = GetUserResult.Data.Id,
                Username = GetUserResult.Data.Username,
                Email = GetUserResult.Data.Email,
                UserAddress = GetUserResult.Data.UserAddress,
                PhoneNumber = GetUserResult.Data.PhoneNumber
            };

            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserViewModle Modle) 
        {
            if (!ModelState.IsValid) 
            {
                
                return View(Modle);
            }

            if (!TryGetUserId(out int Userid))
            {
                return RedirectToAction("Login", "Authantication");

            }

            bool isAdmin = User.IsInRole("SuperAdmin") || User.IsInRole("Admin");

            int actualTargetId = (isAdmin && Modle.Userid.HasValue) ? Modle.Userid.Value : Userid;

            var EditUserResult = await mediator.Send(new EditUserProfileCommnad(actualTargetId, Modle.Username,Modle.Email,Modle.UserAddress,Modle.PhoneNumber));
            if (!EditUserResult.IsSuccess) 
            {
                ViewBag.ErrorMessage = EditUserResult.Message;
                return View(EditUserResult);
            }

            return RedirectToAction("Index", "Products");

        }
    }
}
