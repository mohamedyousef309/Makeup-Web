using Domain_Layer.DTOs.OrderDTOs;
using Domain_Layer.Entites.Order;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Orders.Quries.GetOrderbyid
{
    public record GetOrderbyidQuery(int Orderid):ICommand<RequestRespones<OrderToReturnDto>>;

    public class GetOrderbyidQueryHandler : IRequestHandler<GetOrderbyidQuery, RequestRespones<OrderToReturnDto>>
    {
        private readonly IGenaricRepository<Order> genaricRepository;

        public GetOrderbyidQueryHandler(IGenaricRepository<Order> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<OrderToReturnDto>> Handle(GetOrderbyidQuery request, CancellationToken cancellationToken)
        {
           var order = await genaricRepository.GetByCriteriaQueryable(x => x.Id == request.Orderid)
                .Select(x => new OrderToReturnDto
                {
                    orderid = x.Id,
                    status = x.status.ToString(),
                    subTotal = x.subTotal,
                    Address = x.Address,
                    BuyerEmail = x.BuyerEmail,
                    Deliverycost = x.Deliverycost,
                    Items = x.Items.Select(it => new OrderItemsDTo
                    {
                        PictureUrl = it.PictureUrl??"",
                        Price = it.Price,
                        ProductName = it.ProductName,
                        Quantity = it.Quantity,
                    }).ToList(),
                    orderDate = x.orderDate,
                    PhoneNumber = x.PhoneNumber,
                }).FirstOrDefaultAsync(cancellationToken);
            if (order == null)
            {
                return RequestRespones<OrderToReturnDto>.Fail("Order not found", 404);
            }
            return RequestRespones<OrderToReturnDto>.Success(order, 200);





        }
    }
}
