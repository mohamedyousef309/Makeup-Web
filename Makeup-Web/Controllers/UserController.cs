using Application_Layer.CQRS.User.Quries.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Makeup_Web.Controllers
{
    public class UserController : Controller
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
    }
}
