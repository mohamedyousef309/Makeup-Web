using Application_Layer.CQRS.Basket.Quries.GetUserBsaket;
using Application_Layer.CQRS.Products.Queries;
using Domain_Layer.DTOs.Basket;
using Domain_Layer.DTOs.OrderDTOs;
using Domain_Layer.Entites.Order;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Orders.Commands.CreatOrder
{
    public record CreatOrderOrchestrator(string BuyerEmail, int userid, string PhoneNumber, string Address, IEnumerable<OrderItems> Items) : ICommand<RequestRespones<OrderToReturnDto>>;

    public class CreatOrderOrchestratorHandler:IRequestHandler<CreatOrderOrchestrator,RequestRespones<OrderToReturnDto>>
    {
        private readonly IMediator mediator;

        public CreatOrderOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<RequestRespones<OrderToReturnDto>> Handle(CreatOrderOrchestrator request, CancellationToken cancellationToken)
        {
            var userBasket = await mediator.Send(new GetUserBsaketQuery(request.userid));
            if (!userBasket.IsSuccess || userBasket.Data == null || !userBasket.Data.items.Any())
            {
                return RequestRespones<OrderToReturnDto>.Fail(userBasket.Message, 400);
            }

            var productIds = userBasket.Data.items.Select(i => i.productid).ToList();
            var productsResult = await mediator.Send(new GetProductsByIdsQuery(productIds));

            if (!productsResult.IsSuccess || productsResult.Data == null)
            {
                return RequestRespones<OrderToReturnDto>.Fail(productsResult.Message, 404);
            }

            var orderItems = new List<OrderItems>();
            decimal calculatedSubtotal = 0;

            foreach (var basketItem in userBasket.Data.items)
            {
                var product = productsResult.Data.FirstOrDefault(p => p.Id == basketItem.productid);
                if (product != null)
                {
                    var item = new OrderItems
                    {
                        Id = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = basketItem.Quantity
                    };
                    orderItems.Add(item);
                    calculatedSubtotal += item.Price * item.Quantity;
                }
            }

            var createOrderResult = await mediator.Send(new CreatOrderCommand(
            request.BuyerEmail,
            request.PhoneNumber,
            request.Address,
            orderItems,
            calculatedSubtotal));
            
            if (!createOrderResult.IsSuccess)
            {
                return RequestRespones<OrderToReturnDto>.Fail(createOrderResult.Message, 500);
            }


            var resultDto = new OrderToReturnDto
            {
                BuyerEmail = request.BuyerEmail,
                Address = request.Address,
                subTotal = calculatedSubtotal,
                orderDate = DateTime.Now,
                Items = orderItems.Select(i => new OrderItemsDTo 
                {
                    ProductName = i.ProductName,
                    Price= i.Price,
                    PictureUrl = i.PictureUrl,
                    Quantity = i.Quantity,
                }).ToList()
            };

            return RequestRespones<OrderToReturnDto>.Success(resultDto, 200, "تم إنشاء الطلب بنجاح");
        }

    }

      
 }



