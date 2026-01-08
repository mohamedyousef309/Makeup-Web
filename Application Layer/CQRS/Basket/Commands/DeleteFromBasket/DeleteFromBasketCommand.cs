using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application_Layer.CQRS.Basket.Commands.DeleteFromBasket
{
    public record DeleteFromBasketCommand(int userid,int Productid):ICommand<RequestRespones<bool>>;

    public class DeleteFromBasketCommandHandler : IRequestHandler<DeleteFromBasketCommand, RequestRespones<bool>>
    {
        private readonly IBasketRepository basketRepository;

        public DeleteFromBasketCommandHandler(IBasketRepository basketRepository) 
        {
            this.basketRepository = basketRepository;
        }
        public async Task<RequestRespones<bool>> Handle(DeleteFromBasketCommand request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetCustomerBasket(request.userid.ToString());
            if (basket==null)
            {
                return RequestRespones<bool>.Fail("there is no Basket with this id", 404);
            }

            var itemToRemove = basket.Items.FirstOrDefault(i => i.ProductId == request.Productid);
            if (itemToRemove == null)
                return RequestRespones<bool>.Fail("Product not found in the basket", 404);

            basket.Items.Remove(itemToRemove);

            bool isUpdated;

            if (!basket.Items.Any())
            {
                isUpdated = await basketRepository.DeleteCustomerBasket(request.userid.ToString());
            }
            else 
            {
                var result = await basketRepository.UpdateOrCreateCustomerBasket(basket);
                isUpdated = result != null;
            }

            if (!isUpdated)
            {
                return RequestRespones<bool>.Fail("Error while updating the basket", 500);
            }

            return RequestRespones<bool>.Success(true, 200, "Product removed successfully");
        }


    }
    
}
