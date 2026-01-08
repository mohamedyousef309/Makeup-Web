using Application_Layer.CQRS.Products.Queries;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Basket.Commands.CreateOrUpdateBasket
{
    public record CreateOrUpdateBasketOrchestrator(int userid, int Productid,string ProductName,decimal ProductPrice,int Quantity) : ICommand<RequestRespones<bool>>;

    public class CreateOrUpdateBasketOrchestratorHandler : IRequestHandler<CreateOrUpdateBasketOrchestrator, RequestRespones<bool>>
    {
        private readonly IMediator mediator;

        public CreateOrUpdateBasketOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<RequestRespones<bool>> Handle(CreateOrUpdateBasketOrchestrator request, CancellationToken cancellationToken)
        {
           

            var addProductTobasketResult= await mediator.Send(new CreateOrUpdateBasketoCommands(
                request.userid,
                request.Productid,
                request.ProductName,
                request.ProductPrice,
                Quantity: request.Quantity));

            if (!addProductTobasketResult.IsSuccess)
            {
                return RequestRespones<bool>.Fail("error while adding product to cart", 400);
            }

            return RequestRespones<bool>.Success(true, 200, "product added to basket successfully");

        }
    }
}
