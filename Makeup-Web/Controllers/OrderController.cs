using Application_Layer.CQRS.Orders.Commands.CreatOrder;
using Application_Layer.CQRS.Orders.Quries.GetAllOrdersForUser;
using Domain_Layer.DTOs;
using Domain_Layer.DTOs.OrderDTOs;
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


        public async Task<IActionResult> GetUserOrders(int? pageIndex,int? pageSize,string? sortBy
            ,string? sortDir,
            string? search)
        {
            if(!TryGetUserId(out int userid)) 
            {
                return RedirectToAction("Login", "Authantication");

            }

            var query = new GetAllOrdersForUserQuery(
                    userid,
                    pageSize ?? 10,     
                    pageIndex ?? 1,     
                    sortBy ?? "id",
                    sortDir ?? "desc",
                    search
                );

            var result = await _Mediator.Send(query); 
            if (!result.IsSuccess|| result.Data==null)
            {
                ViewBag.ErrorMessage=result.Message;
                return View(new PaginatedListDto<OrderToReturnDto>()); 

            }

            return View(result.Data);
        }
    }
}
