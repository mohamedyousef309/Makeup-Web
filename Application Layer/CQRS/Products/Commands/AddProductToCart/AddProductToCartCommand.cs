using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.AddProductToCart
{
    public record AddProductToCartCommand(int userid, int productid, string ProductName, decimal ProductPrice, int Quantity) : ICommand<RequestRespones<bool>>;

    public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand, RequestRespones<bool>>
    {
        private readonly IBasketRepository basketRepository;

        public AddProductToCartCommandHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }
        public async Task<RequestRespones<bool>> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetCustomerBasket(request.userid.ToString());
            if (basket == null)
            {
                basket = new Domain_Layer.Entites.Basket.UserCart
                {
                    Id = request.userid.ToString(),
                    Items = new List<Domain_Layer.Entites.Basket.CartItem>()
                };
            }

            var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == request.productid);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                existingItem.Price = request.ProductPrice;
            }
            else
            {
                basket.Items.Add(new Domain_Layer.Entites.Basket.CartItem
                {
                    ProductId = request.productid,
                    Price = request.ProductPrice,
                    ProductName = request.ProductName,
                    Quantity = request.Quantity,
                    UserCartId = request.userid.ToString(),

                });
            }
            var updatedCart = await basketRepository.UpdateOrCreateCustomerBasket(basket);

            if (updatedCart == null)
            {
                return RequestRespones<bool>.Fail("error while adding to Basket", 400);
            }

            return RequestRespones<bool>.Success(true);
        }
    }
}
