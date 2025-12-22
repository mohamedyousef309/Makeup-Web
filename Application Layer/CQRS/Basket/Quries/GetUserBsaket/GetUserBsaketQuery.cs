using Domain_Layer.DTOs.Basket;
using Domain_Layer.Entites.Basket;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Basket.Quries.GetUserBsaket
{
    public record GetUserBsaketQuery(int userid): IRequest<RequestRespones<CustomerBasketDto>>;

    public class GetUserBsaketQueryHandler : IRequestHandler<GetUserBsaketQuery, RequestRespones<CustomerBasketDto>>
    {
        private readonly IBasketRepository basketRepository;

        public GetUserBsaketQueryHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }
        public async Task<RequestRespones<CustomerBasketDto>> Handle(GetUserBsaketQuery request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetCustomerBasketByUserId(request.userid.ToString());
            
            if (basket == null)
            {
                basket = new UserCart
                {
                    Id = request.userid.ToString(),  
                    Items = new List<CartItem>()
                };

                await basketRepository.UpdateOrCreateCustomerBasket(basket);
            }

            var basketDto = new CustomerBasketDto
            {
                Id = basket.Id,
                items = basket.Items.Select(i => new BasketItemsDto
                {
                    productid=i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            };


            return RequestRespones<CustomerBasketDto>.Success(basketDto, 200, "Basket retrieved successfully");


        }
    }


}
