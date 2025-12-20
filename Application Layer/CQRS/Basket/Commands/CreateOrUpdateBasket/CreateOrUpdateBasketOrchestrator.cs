using Application_Layer.CQRS.Products.Queries;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Basket.Commands.CreateOrUpdateBasket
{
    public record CreateOrUpdateBasketOrchestrator(int userid, int Productid,int Quantity) :IRequest<RequestRespones<bool>>;

    public class CreateOrUpdateBasketOrchestratorHandler : IRequestHandler<CreateOrUpdateBasketOrchestrator, RequestRespones<bool>>
    {
        private readonly IMediator mediator;

        public CreateOrUpdateBasketOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<RequestRespones<bool>> Handle(CreateOrUpdateBasketOrchestrator request, CancellationToken cancellationToken)
        {
            var GetProductResult = await mediator.Send(new GetProductByIdQuery(request.Productid), cancellationToken);
            if (!GetProductResult.IsSuccess)
            {
                return RequestRespones<bool>.Fail("this product is not exsit",403);
            }

            var addProductTobasketResult= await mediator.Send(new CreateOrUpdateBasketoCommands(
                request.userid,
                GetProductResult.Data.Id,
                GetProductResult.Data.Name,
                GetProductResult.Data.Price,
                Quantity: request.Quantity));

            if (!addProductTobasketResult.IsSuccess)
            {
                return RequestRespones<bool>.Fail("error while adding product to cart", 400);
            }

            return RequestRespones<bool>.Success(true, 200, "product added to basket successfully");

        }
    }
}
