using Application_Layer.CQRS.Orders.Commands.CreatOrder;
using Domain_Layer.ViewModels.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Makeup_Web.Controllers
{
    public class OrderController : BaseController
    {
        public IMediator _Mediator { get; }

        public OrderController(IMediator mediator)
        {
            _Mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreatOrder() 
        {
            return View(new CreatOrderViewModle());
        }

        [HttpPost]
        public async Task<IActionResult> CreatOrder(CreatOrderViewModle modle)  // / Order/CreatOrder
        {
            if(!TryGetUserId(out int userId)) 
            {
                return RedirectToAction("Login", "Authantication");
            }

            if (!ModelState.IsValid) 
            {
                return View(modle);
                
            }
            var CreatOrderResult = await _Mediator.Send(new CreatOrderOrchestrator(modle.BuyerEmail,userId,modle.PhoneNumber,modle.Address));
            TempData["SuccessMessage"] = "Order created successfully!";
            return RedirectToAction(nameof(GetUserOrders));
        }


        public async Task<IActionResult> GetUserOrders()
        {
            return View();
        }
    }
}
