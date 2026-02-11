using Application_Layer.CQRS.Basket.Quries.GetUserBsaket;
using Application_Layer.CQRS.Products.Queries;
using Application_Layer.CQRS.Products.Queries.GetProductsByIds;
using Application_Layer.CQRS.Products.Queries.GetProductVariantsByIds;
using Application_Layer.CQRS.User.Quries.GetUserEmailbyUserid;
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
    public record CreatOrderOrchestrator(string? BuyerEmail, int userid, string? PhoneNumber, string? Address) : ICommand<RequestRespones<OrderToReturnDto>>;

    public class CreatOrderOrchestratorHandler:IRequestHandler<CreatOrderOrchestrator,RequestRespones<OrderToReturnDto>>
    {
        private readonly IMediator mediator;

        public CreatOrderOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<RequestRespones<OrderToReturnDto>> Handle(CreatOrderOrchestrator request, CancellationToken cancellationToken)
        {
            var userResult = await mediator.Send(new GetUserEmailbyUseridQuery(request.userid));
            if (!userResult.IsSuccess|| userResult.Data==null)
            {
                return RequestRespones<OrderToReturnDto>.Fail(userResult.Message??"User not found.", 404);
            }

            var userBasket = await mediator.Send(new GetUserBsaketQuery(userResult.Data.Id));

            if (!userBasket.IsSuccess || userBasket.Data?.items == null || !userBasket.Data.items.Any())
                return RequestRespones<OrderToReturnDto>.Fail(userBasket.Message ?? "Basket is empty.", 400);


            var variantIds = userBasket.Data.items.Select(i => i.ProductVariantid).ToList();

            var variantsResult = await mediator.Send(new GetProductVariantsByIdsQuery(variantIds));

            if (!variantsResult.IsSuccess || variantsResult.Data == null)
                return RequestRespones<OrderToReturnDto>.Fail(variantsResult.Message ?? "Products not found.", 404);

            var variantsDict = variantsResult.Data.ToDictionary(p => p.id);

            var orderItems = userBasket.Data.items
                .Select(item => {
                    if (!variantsDict.TryGetValue(item.ProductVariantid, out var dbVariant)) return null;
                    return new OrderItems
                    {
                        ProductId = dbVariant.productid,
                        ProductVariantId = dbVariant.id,
                        ProductName = $"{dbVariant.ProductName} ({dbVariant.VariantValue})",
                        Price = dbVariant.price,
                        Quantity = item.Quantity,
                        PictureUrl= item.VariantImageUrl
                    };
                }).Where(x => x != null).ToList();




            if (!orderItems.Any())
                return RequestRespones<OrderToReturnDto>.Fail("No valid items to create order.", 400);

            decimal calculatedSubtotal = orderItems.Sum(i => i.Price * i.Quantity);

            var createOrderResult = await mediator.Send(new CreatOrderCommand(
                userResult.Data.Id,
                request.BuyerEmail ?? userResult.Data.Email,
                request.PhoneNumber ?? userResult.Data.PhoneNumber,
                request.Address ?? userResult.Data.UserAddress,
                orderItems,
                calculatedSubtotal));

            if (!createOrderResult.IsSuccess)
                return RequestRespones<OrderToReturnDto>.Fail(createOrderResult.Message, 500);


            var resultDto = new OrderToReturnDto
            {
                BuyerEmail = request.BuyerEmail ?? userResult.Data.Email,
                Address = request.Address ?? userResult.Data.UserAddress,
                subTotal = calculatedSubtotal,
                PhoneNumber = request.PhoneNumber ?? userResult.Data.PhoneNumber,
                orderDate = DateTime.Now,
                Items = orderItems.Select(i => new OrderItemsDTo
                {
                    ProductName = i.ProductName,
                    Price = i.Price,
                    PictureUrl = i.PictureUrl,
                    Quantity = i.Quantity
                }).ToList()
            };

            return RequestRespones<OrderToReturnDto>.Success(
                resultDto,
                200,
                "Order created successfully :) We will contact you via your phone number."
            );
        }

    }

      
 }



