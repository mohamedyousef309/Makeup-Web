using Domain_Layer.Entites.Order;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand(int Orderid) :ICommand<RequestRespones<bool>>;

    public class EditOrderCommandHandler:IRequestHandler<DeleteOrderCommand,RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Order.Order> genaricRepository;

        public EditOrderCommandHandler(IGenaricRepository<Domain_Layer.Entites.Order.Order> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await genaricRepository.GetByCriteriaQueryable(x => x.Id == request.Orderid).
                    Select(x => new Domain_Layer.Entites.Order.Order 
                    {
                        Id=x.Id,
                        status=x.status
                    }).FirstOrDefaultAsync();

                if (order == null)
                {
                    return RequestRespones<bool>.Fail("Order not found", 404);
                }

                if (order.status == OrderStatus.OutForDelivery)
                {
                    return RequestRespones<bool>.Fail("Cannot delete order that is already OutForDelivery.", 400);
                }

                genaricRepository.Delete(order);

                await genaricRepository.SaveChanges();

                return RequestRespones<bool>.Success(true, 200, "Order and its items deleted successfully");
            }
            catch (Exception ex)
            {
                return RequestRespones<bool>.Fail("An error occurred while editing the order: " + ex.Message, 500);
            }
        }
    }




}
