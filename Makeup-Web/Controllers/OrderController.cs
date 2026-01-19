using Application_Layer.CQRS.Orders.Commands;
using Application_Layer.CQRS.Orders.Commands.CreatOrder;
using Application_Layer.CQRS.Orders.Commands.DeleteOrder;
using Application_Layer.CQRS.Orders.Quries.GetAllOrders;
using Application_Layer.CQRS.Orders.Quries.GetAllOrdersForUser;
using Application_Layer.CQRS.Orders.Quries.GetOrderbyid;
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
        private readonly ILogger<OrderController> logger;

        public IMediator _Mediator { get; }

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _Mediator = mediator;
            this.logger = logger;
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

        public async Task<IActionResult> GetOrderbyid(int orderid) 
        {

            var getOrderResult = await _Mediator.Send(new GetOrderbyidQuery(orderid));
            if (!getOrderResult.IsSuccess || getOrderResult.Data == null)
            {
                TempData["ErrorMessage"] = getOrderResult.Message;
                return View(getOrderResult.Data);
            }
            return View(getOrderResult.Data);
        }

        public async Task<IActionResult> GetAllOrders(int? pageIndex, int? pageSize, string? sortBy
            , string? sortDir,
            string? search)
        {
            try
            {
                var GetallOrdersResult = await _Mediator.Send(new GetAllOrdersQuery(
                pageSize ?? 10,
                pageIndex ?? 1,
                sortBy ?? "id",
                sortDir ?? "desc",
                search
                ));

                if (!GetallOrdersResult.IsSuccess || GetallOrdersResult.Data == null)
                {
                    ViewBag.ErrorMessage = GetallOrdersResult.Message;
                    return View(new PaginatedListDto<OrderToReturnDto>());
                }

                return View(GetallOrdersResult.Data);



            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Error occurred while fetching all orders.");

                ViewBag.ErrorMessage = "Something went wrong on our side. Please try again later.";

                return View(new PaginatedListDto<OrderToReturnDto>());
            }

        }

        [HttpPost]

        public async Task<IActionResult> DeletOrder(int orderid) 
        {
            var DeletOrderResult = await _Mediator.Send(new DeleteOrderCommand(orderid));

            if (!DeletOrderResult.IsSuccess) 
            {
                return BadRequest(new { success = false, message = DeletOrderResult.Message });
            }
            return Ok(new { success = true, message = "Order deleted" });
        }
    }
}
