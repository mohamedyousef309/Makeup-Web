using Application_Layer.CQRS.Orders.Commands.UpdateOrder;
using Domain_Layer.Entites; 
using Domain_Layer.Entites.Order;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using Domain_Layer.DTOs.OrderDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore; 

namespace Application_Layer.CQRS.Orders.Commands.UpdateOrderDetails
{
    public record UpdateOrderDetailsCommand(int OrderId, List<OrderItemUpdateDto> NewItems) : ICommand<RequestRespones<bool>>;
    public class UpdateOrderDetailsCommandHandler : IRequestHandler<UpdateOrderDetailsCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Order> _orderRepo;
        private readonly IGenaricRepository<ProductVariant> _variantRepo;
        private readonly IGenaricRepository<OrderItems> _orderItemsRepo;

        public UpdateOrderDetailsCommandHandler(
            IGenaricRepository<Order> orderRepo,
            IGenaricRepository<ProductVariant> variantRepo,
            IGenaricRepository<OrderItems> orderItemsRepo)
        {
            _orderRepo = orderRepo;
            _variantRepo = variantRepo;
            _orderItemsRepo = orderItemsRepo;
        }

        public async Task<RequestRespones<bool>> Handle(UpdateOrderDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
               
                var order = await _orderRepo.GetByCriteriaQueryable(o => o.Id == request.OrderId)
                                            .Include(o => o.Items) 
                                            .FirstOrDefaultAsync(cancellationToken);

                if (order == null) return RequestRespones<bool>.Fail("Order not found", 404);

                
                foreach (var oldItem in order.Items)
                {
                    var variant = await _variantRepo.GetByCriteriaQueryable(v => v.Id == oldItem.ProductVariantId)
                                                   .FirstOrDefaultAsync(cancellationToken);
                    if (variant != null)
                    {
                        variant.Stock += oldItem.Quantity;
                        _variantRepo.SaveInclude(variant, nameof(variant.Stock));
                    }
                }

                
                _orderItemsRepo.DeleteRange(order.Items);

                decimal newSubTotal = 0;
                var newItems = new List<OrderItems>();

                foreach (var item in request.NewItems)
                {
                    var variant = await _variantRepo.GetByCriteriaQueryable(v => v.Id == item.ProductVariantId)
                                                   .Include(v => v.Product)
                                                   .FirstOrDefaultAsync(cancellationToken);

                    if (variant == null) return RequestRespones<bool>.Fail($"Variant {item.ProductVariantId} not found", 400);

                    variant.Stock -= item.Quantity;
                    _variantRepo.SaveInclude(variant, nameof(variant.Stock));

                    newItems.Add(new OrderItems
                    {
                        orderid = order.Id,
                        ProductVariantId = variant.Id,
                        ProductId = variant.ProductId,
                        ProductName = variant.Product.Name ?? "Unknown", 
                        Quantity = item.Quantity,
                        Price = variant.Price
                    });

                    newSubTotal += (variant.Price * item.Quantity);
                }

                order.Items = newItems;
                order.subTotal = newSubTotal;
                _orderRepo.SaveInclude(order, nameof(order.subTotal));

                await _orderRepo.SaveChanges();
                return RequestRespones<bool>.Success(true, 200, "Updated successfully");
            }
            catch (Exception) 
            {
                return RequestRespones<bool>.Fail("An error occurred during update", 500);
            }
        }
    }
}