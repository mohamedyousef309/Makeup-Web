using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Basket.Commands.DeletBasket
{
    public record DeletBasketCommand(int userid) :IRequest<RequestRespones<bool>>;

    public class DeletBasketCommandHandler : IRequestHandler<DeletBasketCommand, RequestRespones<bool>>
    {
        private readonly IBasketRepository basketRepository;

        public DeletBasketCommandHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }
        public async Task<RequestRespones<bool>> Handle(DeletBasketCommand request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetCustomerBasket(request.userid.ToString());
            if (basket==null)
            {
                return RequestRespones<bool>.Fail("Basket not found", 404);

            }

            var isDeleted = await basketRepository.DeleteCustomerBasket(basket.Id);

            if (!isDeleted)
            {
                return RequestRespones<bool>.Fail("Basket not found or already deleted", 404);
            }

            return RequestRespones<bool>.Success(true, 200, "Basket deleted successfully");
        }
    }
}
        
    

