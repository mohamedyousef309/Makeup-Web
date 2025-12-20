using Application_Layer.CQRS.Basket.Commands.CreateOrUpdateBasket;
using Application_Layer.CQRS.Basket.Commands.DeletBasket;
using Application_Layer.CQRS.Basket.Quries.GetUserBsaket;
using Domain_Layer.ViewModels.Basket;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Makeup_Web.Controllers
{
    public class basketController : Controller
    {
        private readonly IMediator mediator;

        public basketController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToBasket() 
        {
            return View( new AddToBasketViewModle());
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(AddToBasketViewModle Modle)
        {
            if (!ModelState.IsValid)
            {
                return View(Modle);
            }
            var result = await mediator.Send(new CreateOrUpdateBasketOrchestrator(Modle.UserId,Modle.ProductId,Modle.Quantity));

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(Modle);
            }

            TempData["SuccessMessage"] = "Product added to basket successfully!";
            return RedirectToAction("Index"); 
        }

        public async Task<IActionResult> GetUserBasketByUserid(int userid) 
        {
            var basketResult =  await mediator.Send(new GetUserBsaketQuery(userid));
            if (basketResult.IsSuccess)
            {
                return Ok(basketResult.Data);
            }
            else
            {
                return NotFound(new { message = basketResult.Message });

            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomerBasket(int userid) 
        {
            var result = await mediator.Send(new DeletBasketCommand(userid));
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            else
            {
                return NotFound(new { message = result.Message });

            }
        }
    }
}
