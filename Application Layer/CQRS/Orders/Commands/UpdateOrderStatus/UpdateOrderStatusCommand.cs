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

namespace Application_Layer.CQRS.Orders.Commands.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(int Orderid, OrderStatus OrderStatus):ICommand<RequestRespones<bool>>;

    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Order> genaricRepository;

        public UpdateOrderStatusCommandHandler(IGenaricRepository<Order> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var Order = await genaricRepository.GetByCriteriaQueryable(x => x.Id == request.Orderid).FirstOrDefaultAsync(cancellationToken);

                if (Order == null)
                {
                    return RequestRespones<bool>.Fail("Order Not Found", 404);
                }

                Order.status = request.OrderStatus;

                genaricRepository.SaveInclude(Order, nameof(Order.status));

                await genaricRepository.SaveChanges();

                return RequestRespones<bool>.Success(true, 200, "Order status updated successfully");
            }
            catch (Exception ex)
            {

                return RequestRespones<bool>.Fail($"Unexpected Error{ex}", 404);
            }


        }
    }
}
