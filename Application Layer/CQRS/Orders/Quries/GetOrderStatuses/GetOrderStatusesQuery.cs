using Domain_Layer.DTOs.OrderDTOs;
using Domain_Layer.Entites.Order;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Orders.Quries.GetOrderStatuses
{
    public record GetOrderStatusesQuery : IRequest<RequestRespones<IEnumerable<OrderStatusDto>>>;

    public class GetOrderStatusesQueryHandler : IRequestHandler<GetOrderStatusesQuery, RequestRespones<IEnumerable<OrderStatusDto>>>
    {
        public Task<RequestRespones<IEnumerable<OrderStatusDto>>> Handle(GetOrderStatusesQuery request, CancellationToken cancellationToken)
        {
            var statuses = Enum.GetValues(typeof(OrderStatus))
                           .Cast<OrderStatus>()
                           .Select(s => new OrderStatusDto
                           {
                               Value = (int)s,
                               Name = s.ToString(),
                               DisplayName = SplitCamelCase(s.ToString())
                           }).ToList();
            if (!statuses.Any())
            {
                return Task.FromResult(RequestRespones<IEnumerable<OrderStatusDto>>.Fail("Error While Geting status",404));

            }


            return Task.FromResult(RequestRespones<IEnumerable<OrderStatusDto>>.Success(statuses));
        }

        private string SplitCamelCase(string input) =>
            System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1").Trim();
    }
}
