using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Basket.Commands.CreateOrUpdateBasket
{
    public record CreateOrUpdateBasketoCommands( int userid,int ProductId,int ProductVariantid, string VariantImageUrl, IEnumerable<string> productVariantValues, string ProductName, decimal Price, int Quantity): ICommand<RequestRespones<bool>>;

    public class CreateOrUpdateBasketoCommandsHandler : IRequestHandler<CreateOrUpdateBasketoCommands, RequestRespones<bool>>
    {
        private readonly IBasketRepository basketRepository;

        public CreateOrUpdateBasketoCommandsHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }
        public async Task<RequestRespones<bool>> Handle(CreateOrUpdateBasketoCommands request, CancellationToken cancellationToken)
        {
            var basket= await basketRepository.GetCustomerBasket(request.userid.ToString());

            if (basket == null)
            {
                basket = new Domain_Layer.Entites.Basket.UserCart
                {
                    Id = request.userid.ToString(),
                    Items = new List<Domain_Layer.Entites.Basket.CartItem>()
                };
            }
            var existingItem = basket.Items.FirstOrDefault(i => i.ProductVariantid == request.ProductVariantid);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                existingItem.Price = request.Price;
            }
            else
            {
                basket.Items.Add(new Domain_Layer.Entites.Basket.CartItem
                {
                    ProductId = request.ProductId,
                    ProductVariantid = request.ProductVariantid,
                    VariantImageUrl = request.VariantImageUrl,
                    ProductName = request.ProductName,
                    Price = request.Price,
                    Quantity = request.Quantity,
                    ProductVariantValues = request.productVariantValues,
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
