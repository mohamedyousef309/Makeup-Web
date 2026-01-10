using Domain_Layer.DTOs.OrderDTOs;
using Domain_Layer.Entites;
using Domain_Layer.Entites.Order;
using Domain_Layer.Events;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Orders.Commands.CreatOrder
{
    public record CreatOrderCommand(int userid,string BuyerEmail, string PhoneNumber, string Address , IEnumerable<OrderItems> Items, decimal subTotal) : ICommand<RequestRespones<bool>>;

    public class CreatOrderCommandHandler : IRequestHandler<CreatOrderCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Order> OrderRepo;
        private readonly IGenaricRepository<Product> productRepo;
        private readonly IMediator mediator;

        public CreatOrderCommandHandler(IGenaricRepository<Order> OrderRepo,IGenaricRepository<Product> ProductRepo,IMediator mediator)
        {
            this.OrderRepo = OrderRepo;
            productRepo = ProductRepo;
            this.mediator = mediator;
        }
        public async Task<RequestRespones<bool>> Handle(CreatOrderCommand request, CancellationToken cancellationToken)
        {
            var outOfStockEvents = await ProcessStockReductionAsync(request.Items, cancellationToken);

            var order = new Order
            {
                    UserId = request.userid,
                    BuyerEmail = request.BuyerEmail,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Items = request.Items,
                    status = OrderStatus.pending,
                    orderDate = DateTimeOffset.UtcNow,
                    subTotal = request.subTotal,

            };

            

            await OrderRepo.addAsync(order);

                await OrderRepo.SaveChanges();

            foreach (var Event in outOfStockEvents)
            {
                await mediator.Publish(Event);
            }
            return RequestRespones<bool>.Success(true,Message: "Order Created Succesfuly");
            
          

        }

        public async Task<IEnumerable<OutOfStockEvent>> ProcessStockReductionAsync(IEnumerable<OrderItems> items, CancellationToken CT ) 
        {
            var events = new List<OutOfStockEvent>();
            var productsIds = items.Select(x => x.ProductId).ToList();

            var products = await productRepo.GetByCriteriaQueryable(p => productsIds.Contains(p.Id)).AsTracking()
                .ToListAsync(CT);

            foreach (var item in items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);

                if (product == null)
                {
                    throw new Exception($"Product with ID {item.Id} not found in database.");
                }

                if (product.Stock < item.Quantity)
                {
                    throw new Exception($"Product {product.Name} Out Of Stock: {product.Stock}");
                }

                bool isFinished = product.ReduceStock(item.Quantity);

                if (isFinished)
                {
                    events.Add(new OutOfStockEvent(product.Id, product.Name));
                }
            }

            return events;

        }
    

  
    }


  }
