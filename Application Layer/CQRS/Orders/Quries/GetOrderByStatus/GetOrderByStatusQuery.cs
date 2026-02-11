using Domain_Layer.DTOs.OrderDTOs;
using Domain_Layer.Entites.Order;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Orders.Quries.GetOrderByStatus
{
    public record GetOrderByStatusQuery(OrderStatus OrderStatus):IRequest<RequestRespones<IEnumerable<OrderToReturnDto>>>;

    public class GetOrderByStatusQueryHandler:IRequestHandler<GetOrderByStatusQuery,RequestRespones<IEnumerable<OrderToReturnDto>>>
    {
        private readonly IGenaricRepository<Order> genaricRepository;

        public GetOrderByStatusQueryHandler(IGenaricRepository<Order> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<IEnumerable<OrderToReturnDto>>> Handle(GetOrderByStatusQuery request, CancellationToken cancellationToken)
        {
            var order = await genaricRepository.GetByCriteriaQueryable(x => x.status == request.OrderStatus)
                .Select(x => new OrderToReturnDto 
                {
                    orderid=x.Id,
                    BuyerEmail=x.BuyerEmail,
                    subTotal=x.subTotal,
                    status=x.status.ToString(),
                    orderDate=x.orderDate,
                    PhoneNumber=x.PhoneNumber
                }).ToListAsync(cancellationToken);

            if (!order.Any())
            {
                return RequestRespones<IEnumerable<OrderToReturnDto>>.Fail("NO Orders Found ;(", 404);
            }

            return RequestRespones<IEnumerable<OrderToReturnDto>>.Success(order, 200, "Orders Retrieved Successfully.");
        }
    }

}
