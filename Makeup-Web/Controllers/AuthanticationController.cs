using Application_Layer.CQRS.Authantication.Commads.Login;
using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.Respones;
using Domain_Layer.ViewModels.AuthanticationViewModles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Makeup_Web.Controllers
{
    public class AuthanticationController : Controller
    {
        private readonly IMediator mediator;

        public AuthanticationController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<ActionResult<EndpointRespones<AuthModleDto>>> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginRespone = await mediator.Send(new LoginOrchestrator(model.Email, model.Password));
                if (!loginRespone.IsSuccess)
                {
                    model.ErrorMessage = loginRespone.Message;
                    return View(model);
                }

                return RedirectToAction("Index", "Home");


            }

            return View(model);

        }
    }
}

