using Application_Layer.CQRS.Basket.Commands.CreateOrUpdateBasket;
using Application_Layer.CQRS.Basket.Commands.DeletBasket;
using Application_Layer.CQRS.Basket.Commands.DeleteFromBasket;
using Application_Layer.CQRS.Basket.Commands.UpdateBasketProductQuntaty;
using Application_Layer.CQRS.Basket.Quries.GetUserBsaket;
using Application_Layer.CQRS.Products.Commands.UpdateProduct;
using Domain_Layer.Entites.Basket;
using Domain_Layer.ViewModels.Basket;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Makeup_Web.Controllers
{
    public class basketController : BaseController
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

        //public IActionResult AddToBasket() 
        //{
        //    return View( new AddToBasketViewModle());
        //}

        [HttpPost]
        public async Task<IActionResult> AddToBasket([FromForm] AddToBasketViewModle Modle)
        {

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdStr == null)
            {
                return RedirectToAction("Login", "Authantication");
            }

            int userId = int.Parse(userIdStr);


            if (!ModelState.IsValid)
            {
                return View(Modle);
            }

            var userIdFromClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await mediator.Send(new CreateOrUpdateBasketOrchestrator(userId, Modle.ProductId,Modle.ProductName,Modle.ProductPrice,Modle.Quantity));

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(Modle);
            }

            TempData["SuccessMessage"] = "Product added to basket successfully!";
            return RedirectToAction("GetAllProducts", "Products"); 
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItemFromBasket(int productId) 
        {
            if(!TryGetUserId( out int userid)) 
            {
                return RedirectToAction("Login", "Authantication");
            }
            var DeleteFromBasketresult = await mediator.Send(new DeleteFromBasketCommand(userid, productId));

            if (!DeleteFromBasketresult.IsSuccess)
            {
                ModelState.AddModelError(string.Empty,DeleteFromBasketresult.Message);
                return View();
            }
            return View("GetUserBasketByUserid");

        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductQunaty([FromBody]UpdateProductQuntatyViewModle Modle) 
        {
            if (!TryGetUserId(out var userId))
            {
                return RedirectToAction("Login", "Authantication");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateProductQunatyResult = await mediator.Send(new UpdateBasketProductQuntatyCommand(userId, Modle.ProductId,  Modle.NewQuantity));
            if (!UpdateProductQunatyResult.IsSuccess)
            {
                return BadRequest(new { message = UpdateProductQunatyResult.Message });
            }

            return Ok(UpdateProductQunatyResult.Data);
        }

        public async Task<IActionResult> GetUserBasketByUserid() // /basket/GetUserBasketByUserid
        {
           

            if (!TryGetUserId(out var userId))
            {
                return RedirectToAction("Login", "Authantication");
            }

            


            var basketResult =  await mediator.Send(new GetUserBsaketQuery(userId));
            if (basketResult.IsSuccess)
            {
               return View(basketResult.Data);
            }
            else
            {
               var basket = new UserCart { Id = userId.ToString(), Items = new List<CartItem>() };
                return View(basketResult);

            }
        }

        [HttpPost]
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
