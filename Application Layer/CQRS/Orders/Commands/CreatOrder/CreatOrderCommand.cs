using Domain_Layer.DTOs.OrderDTOs;
using Domain_Layer.Entites.Order;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Orders.Commands.CreatOrder
{
    public record CreatOrderCommand(string BuyerEmail, string PhoneNumber, string Address , IEnumerable<OrderItems> Items, decimal subTotal) :IRequest<RequestRespones<bool>>;

    public class CreatOrderCommandHandler : IRequestHandler<CreatOrderCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Order> genaricRepository;

        public CreatOrderCommandHandler(IGenaricRepository<Order> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(CreatOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = new Order
                {
                    BuyerEmail = request.BuyerEmail,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Items = request.Items,
                    status = OrderStatus.pending,
                    orderDate = DateTimeOffset.UtcNow,
                    subTotal = request.subTotal,

                };

               await genaricRepository.addAsync(order);
                await genaricRepository.SaveChanges();

                return RequestRespones<bool>.Success(true,Message: "Order Created Succesfuly");
            }
            catch (Exception ex)
            {

                return RequestRespones<bool>.Fail("Failed to create order",400);
            }

        }
    }


}
