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
        private readonly IGenaricRepository<ProductVariant> ProductVariant;
        private readonly IMediator mediator;

        public CreatOrderCommandHandler(IGenaricRepository<Order> OrderRepo,IGenaricRepository<ProductVariant> ProductRepo,IMediator mediator)
        {
            this.OrderRepo = OrderRepo;
            ProductVariant = ProductRepo;
            this.mediator = mediator;
        }
        public async Task<RequestRespones<bool>> Handle(CreatOrderCommand request, CancellationToken cancellationToken)
        {
            var outOfStockEvents = await ProcessVariantStockReductionAsync(request.Items, cancellationToken);

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

        public async Task<IEnumerable<OutOfStockEvent>> ProcessVariantStockReductionAsync(IEnumerable<OrderItems> items, CancellationToken CT ) 
        {
            var events = new List<OutOfStockEvent>();
            var ProductVariantIds = items.Select(x => x.ProductVariantId).ToList();

            var ProductVariants = await ProductVariant.GetByCriteriaQueryable(p => ProductVariantIds.Contains(p.Id)).Include(x=>x.Product).AsTracking()
                .ToListAsync(CT);

            foreach (var item in items)
            {
                var ProductVariant = ProductVariants.FirstOrDefault(p => p.Id == item.ProductVariantId);

                if (ProductVariant == null)
                {
                    throw new Exception($"Product with ID {item.Id} not found in database.");
                }

                if (ProductVariant.Stock < item.Quantity)
                {
                    throw new Exception($"Product {ProductVariant.Product.Name}-{ProductVariant.VariantName} Out Of Stock: {ProductVariant.Stock}");
                }

                bool isFinished = ProductVariant.ReduceStock(item.Quantity);

                if (isFinished)
                {
                    events.Add(new OutOfStockEvent(ProductVariant.Id, ProductVariant.Product.Name,ProductVariant.VariantName));
                }
            }

            return events;

        }
    

  
    }


  }
