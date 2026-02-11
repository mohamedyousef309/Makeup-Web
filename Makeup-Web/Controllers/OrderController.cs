using Application_Layer.CQRS.Orders.Commands;
using Application_Layer.CQRS.Orders.Commands.CreatOrder;
using Application_Layer.CQRS.Orders.Commands.DeleteOrder;
using Application_Layer.CQRS.Orders.Commands.UpdateOrder;
using Application_Layer.CQRS.Orders.Commands.UpdateOrderDetails; // الـ Namespace الجديد
using Application_Layer.CQRS.Orders.Commands.UpdateOrderStatus;
using Application_Layer.CQRS.Orders.Quries.GetAllOrders;
using Application_Layer.CQRS.Orders.Quries.GetAllOrdersForUser;
using Application_Layer.CQRS.Orders.Quries.GetOrderbyid;
using Application_Layer.CQRS.Orders.Quries.GetOrderStatuses;
using Domain_Layer.DTOs;
using Domain_Layer.DTOs.OrderDTOs;
using Domain_Layer.Entites.Order;
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

        #region Create Order
        public IActionResult CreatOrder()
        {
            return View(new CreatOrderViewModle());
        }

        [HttpPost]
        public async Task<IActionResult> CreatOrder(CreatOrderViewModle modle)
        {
            if (!TryGetUserId(out int userId))
            {
                return RedirectToAction("Login", "Authantication");
            }

            if (!ModelState.IsValid)
            {
                return View(modle);
            }

            var CreatOrderResult = await _Mediator.Send(new CreatOrderOrchestrator(modle.BuyerEmail, userId, modle.PhoneNumber, modle.Address));
            if (!CreatOrderResult.IsSuccess)
            {
                TempData["ErrorMessage"] = CreatOrderResult.Message;
                return RedirectToAction("GetUserBasketByUserid", "basket");

            }
            TempData["SuccessMessage"] = "Order created successfully!";
            return RedirectToAction(nameof(GetUserOrders));
        }
        #endregion

        #region Update Order (Status & Info)
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(int orderId, OrderStatus status, string address, string phoneNumber)
        {
            var result = await _Mediator.Send(new UpdateOrderCommand(orderId, status, address, phoneNumber));

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(GetOrderbyid), new { orderid = orderId });
            }

            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(GetOrderbyid), new { orderid = orderId });

        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusViewModle Modle) 
        {
            if (!ModelState.IsValid)
            {
                var Errors = ModelState.Where(x=>x.Value.Errors.Any())
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return BadRequest(new
                {
                    success = false,
                    message = "Validation failed",
                    Errors
                });
            }

            var UpdateOrderStatusReslut= await _Mediator.Send(new UpdateOrderStatusCommand(Modle.Orderid,Modle.OrderStatus));

            if (!UpdateOrderStatusReslut.IsSuccess)
            {
                return Json(new { succes = false,Message="Error While Updating Status" });
            }

            return Json(new { succes = true, Message = "Updated Successfuly" });

        }
        #endregion

        #region Update Order Details (Items & Inventory)
        /// <summary>
        /// تحديث أصناف الطلب (إضافة/حذف/تعديل كميات)
        /// يتم استدعاؤها غالباً عبر AJAX JSON
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateOrderDetails([FromBody] UpdateOrderDetailsCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "invalid Data" });
            }

            var result = await _Mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(new { success = true, message = result.Message });
            }

            return BadRequest(new { success = false, message = result.Message });
        }
        #endregion

        #region Queries (Get Orders)
        public async Task<IActionResult> GetUserOrders(int? pageIndex, int? pageSize, string? sortBy, string? sortDir, string? search)
        {
            if (!TryGetUserId(out int userid))
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
            if (!result.IsSuccess || result.Data == null)
            {
                ViewBag.ErrorMessage = result.Message;
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

        public async Task<IActionResult> GetAllOrders(int? pageIndex, int? pageSize, string? sortBy, string? sortDir, string? search)
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
                ViewBag.ErrorMessage = "Error Try againg Later.";
                return View(new PaginatedListDto<OrderToReturnDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderStatus() 
        {
            var GetStatusResult = await _Mediator.Send(new GetOrderStatusesQuery());

            if (!GetStatusResult.IsSuccess)
            {
                return Json(new { succes = false, Message = GetStatusResult.Message });
            }

            return Json(new { succes = true, Message = GetStatusResult.Message });

        }
        #endregion

        #region Delete Order
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
        #endregion
    }
}