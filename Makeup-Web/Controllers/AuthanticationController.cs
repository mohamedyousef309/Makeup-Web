using Application_Layer.CQRS.Authantication.Commads.Login;
using Application_Layer.CQRS.Authantication.Commads.Register;
using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.Respones;
using Domain_Layer.ViewModels.AuthanticationViewModles.Login;
using Domain_Layer.ViewModels.AuthanticationViewModles.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

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

                Response.Cookies.Append("AccessToken", loginRespone.Data.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.Strict, 
                    Expires = loginRespone.Data.TokenExpiresOn
                });

                Response.Cookies.Append("RefreshToken", loginRespone.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.Strict, 
                    Expires = loginRespone.Data.RefreshTokenExpiration
                });

                return RedirectToAction("Index", "Home");


            }

            return View(model);

        }


        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var RegisterResult = await mediator.Send(new RegisterCommand(model.Email, model.Password, model.PhoneNumber, model.UserAddress, model.Image));

                if (!RegisterResult.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, RegisterResult.Message);
                    return View(model);
                }

                return RedirectToAction("Login");

            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                return View(model);
            }

           
        }
    }
}

