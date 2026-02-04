using Application_Layer.CQRS.Authantication.Commads.ForgetPasswrd;
using Application_Layer.CQRS.Authantication.Commads.Login;
using Application_Layer.CQRS.Authantication.Commads.Register;
using Application_Layer.CQRS.Authantication.Commads.ValidateUserVerificationCode;
using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.Respones;
using Domain_Layer.ViewModels.AuthanticationViewModles.ForgetPassword;
using Domain_Layer.ViewModels.AuthanticationViewModles.Login;
using Domain_Layer.ViewModels.AuthanticationViewModles.Register;
using Domain_Layer.ViewModels.AuthanticationViewModles.ResetPassword;
using Domain_Layer.ViewModels.AuthanticationViewModles.VerifyCode;
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
        [ValidateAntiForgeryToken]

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

                return RedirectToAction("Index","Products");


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

        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");

            return RedirectToAction("Index", "Products");
        }

        public IActionResult ForgetPassword()
        {
            return View(new ForgetPasswordViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel Modle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Modle);
                }

                var ForgetPasswordresult = await mediator.Send(new ForgetpasswordOrchestrator(Modle.UserEmail));

                if (!ForgetPasswordresult.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, ForgetPasswordresult.Message);
                    return View(Modle);
                }

                return RedirectToAction("VerifyCode", new { email = Modle.UserEmail });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");

                return View(Modle);
            }


        }

        [HttpGet]
        public IActionResult VerifyCode(string email)
        {
            return View(new VerifyCodeViewModel { Email = email });
        }
        [HttpPost]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel Modle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Modle);

                }
                var VerifyCodeResult = await mediator.Send(new ValidateVerificationCodeOrchestrator(Modle.Code, Modle.Email));
                if (!VerifyCodeResult.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, VerifyCodeResult.Message);
                    return View(Modle);
                }

                return RedirectToAction("ResetPassword", new { email = Modle.Email });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                return View(Modle);

            }

        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            return View(new ResetPasswordViewModel { UserEmail = email });

        }

        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel Modle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Modle);
                }
                var ResetPasswordResult = await mediator.Send(new Application_Layer.CQRS.Authantication.Commads.ResetPassword.ResetPasswordOrchestrator(Modle.UserEmail, Modle.NewPassword));
                if (!ResetPasswordResult.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, ResetPasswordResult.Message);
                    return View(Modle);
                }
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                return View(Modle);
            }

        }
    }
}

