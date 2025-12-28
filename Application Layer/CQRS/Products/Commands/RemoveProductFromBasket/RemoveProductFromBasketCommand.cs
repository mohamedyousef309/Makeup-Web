using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.RemoveProductFromBasket
{
    public record RemoveProductFromBasketCommand(int Userid,int productId):ICommand<RequestRespones<bool>>;

    public class RemoveProductFromBasketCommandHandler : IRequestHandler<RemoveProductFromBasketCommand, RequestRespones<bool>>
    {
        private readonly IBasketRepository basketRepository;

        public RemoveProductFromBasketCommandHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }
        public async Task<RequestRespones<bool>> Handle(RemoveProductFromBasketCommand request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetCustomerBasket(request.Userid.ToString());
            if (basket == null) 
            {
                return RequestRespones<bool>.Fail("there is no basket with this id",404);
            }
            var itemToRemove = basket.Items.FirstOrDefault(x => x.ProductId == request.productId);
            if (itemToRemove != null)
            {
                var updatedItems = basket.Items.ToList();
                updatedItems.Remove(itemToRemove);
                basket.Items = updatedItems;

                var result = await basketRepository.UpdateOrCreateCustomerBasket(basket);

                if (result == null)
                    return RequestRespones<bool>.Fail("Failed to update basket after removal", 500);

                return RequestRespones<bool>.Success(true, 200, "Product removed from basket successfully");
            }

            return RequestRespones<bool>.Fail("Product not found in basket", 404);
        }
    }



}
