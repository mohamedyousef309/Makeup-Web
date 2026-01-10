using Domain_Layer.DTOs.Basket;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Entites.Basket;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Basket.Commands.UpdateBasketProductQuntaty
{
    public record UpdateBasketProductQuntatyCommand(int Userid,int productid,int newQuntaty):ICommand<RequestRespones<CustomerBasketDto>>;

    public class UpdateBasketProductQuntatyCommandHandler : IRequestHandler<UpdateBasketProductQuntatyCommand, RequestRespones<CustomerBasketDto>>
    {
        private readonly IBasketRepository basketRepository;

        public UpdateBasketProductQuntatyCommandHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }
        public async Task<RequestRespones<CustomerBasketDto>> Handle(UpdateBasketProductQuntatyCommand request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetCustomerBasket(request.Userid.ToString());

            if (basket == null)
            {
                return RequestRespones<CustomerBasketDto>.Fail("Basket not found for this user.", 404);
            }

            var item = basket.Items.FirstOrDefault(x => x.ProductId == request.productid);

            if (item == null)
            {
                return RequestRespones<CustomerBasketDto>.Fail("Product not found in your basket.", 404);
            }

            
            else
            {
                item.Quantity = request.newQuntaty;
            }

            var updatedBasket = await basketRepository.UpdateOrCreateCustomerBasket(basket);

            if (updatedBasket == null)
            {
                return RequestRespones<CustomerBasketDto>.Fail("Could not update the basket in database.", 500);
            }

            var responseDto = MapToDto(updatedBasket);

            return RequestRespones<CustomerBasketDto>.Success(responseDto);

        }

        private CustomerBasketDto MapToDto(UserCart basket)
        {
            return new CustomerBasketDto
            {
                Id = basket.Id,
                items = basket.Items.Select(i => new BasketItemsDto
                {
                    productid = i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    PictureUrl = i.PictureUrl 
                }).ToList()
            };
        }
    }
}
