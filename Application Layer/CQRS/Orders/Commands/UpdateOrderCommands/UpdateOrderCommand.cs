using Application_Layer.CQRS.Orders.Commands.UpdateOrder;
using Domain_Layer.Entites.Order;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application_Layer.CQRS.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(int OrderId, OrderStatus Status, string Address, string PhoneNumber) : ICommand<RequestRespones<bool>>;

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Order> _orderRepo;

        public UpdateOrderCommandHandler(IGenaricRepository<Order> orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<RequestRespones<bool>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
           
            var order = await _orderRepo.GetByCriteriaQueryable(o => o.Id == request.OrderId)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
            {
                return RequestRespones<bool>.Fail("Order not found.", 404);
            }

            
            if (order.status == OrderStatus.Cancelled || order.status == OrderStatus.Delivered)
            {
                return RequestRespones<bool>.Fail($"Order already {order.status} and cannot be modified.", 400);
            }

            
            order.status = request.Status;
            order.Address = request.Address;
            order.PhoneNumber = request.PhoneNumber;

            
            
            _orderRepo.SaveInclude(order,
                nameof(order.status),
                nameof(order.Address),
                nameof(order.PhoneNumber));

           
            await _orderRepo.SaveChanges();

            return RequestRespones<bool>.Success(true, 200, "Order details updated successfully.");
        }
    }
}