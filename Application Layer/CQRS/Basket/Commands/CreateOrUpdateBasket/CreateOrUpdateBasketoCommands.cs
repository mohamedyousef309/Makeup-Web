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
    public record CreateOrUpdateBasketoCommands( int userid,int ProductId, string ProductName, decimal Price, int Quantity):IRequest<RequestRespones<bool>>;

    public class CreateOrUpdateBasketoCommandsHandler : IRequestHandler<CreateOrUpdateBasketoCommands, RequestRespones<bool>>
    {
        private readonly IBasketRepository basketRepository;

        public CreateOrUpdateBasketoCommandsHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }
        public async Task<RequestRespones<bool>> Handle(CreateOrUpdateBasketoCommands request, CancellationToken cancellationToken)
        {
            var basket= await basketRepository.GetCustomerBasketByUserId(request.userid.ToString());

            if (basket == null)
            {
                basket = new Domain_Layer.Entites.Basket.UserCart
                {
                    Id = request.userid.ToString(),
                    UserId= request.userid,
                    Items = new List<Domain_Layer.Entites.Basket.CartItem>()
                };
            }
                var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

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
                        Price = request.Price,
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
