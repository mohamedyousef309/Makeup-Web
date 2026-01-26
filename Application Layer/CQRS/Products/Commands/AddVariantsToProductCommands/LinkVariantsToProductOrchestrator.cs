using Application_Layer.CQRS.Products.Commands.AddVariantsToProduct;
using Application_Layer.CQRS.Products.Queries.GetProductVariantsByIds;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;

namespace Application_Layer.Orchestrators
{
    public record LinkVariantsToProductOrchestrator(IEnumerable<int> variantIds, int productId ) : ICommand<RequestRespones<bool>>;

    public class LinkVariantsToProductOrchestratorHandler: IRequestHandler<LinkVariantsToProductOrchestrator, RequestRespones<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IGenaricRepository<Product> _productRepo;

        public LinkVariantsToProductOrchestratorHandler(IMediator mediator, IGenaricRepository<Product> productRepo)
        {
            _mediator = mediator;
            _productRepo = productRepo;
        }

        public async Task<RequestRespones<bool>> Handle(LinkVariantsToProductOrchestrator request, CancellationToken cancellationToken)
        {
            var productExists = await _productRepo.ExistsAsync(request.productId);

            if (!productExists)
            {
                return RequestRespones<bool>.Fail($"Product with ID {request.productId} does not exist.", 404);
            }


            if (request.variantIds == null || !request.variantIds.Any())
            {
                return RequestRespones<bool>.Fail("The list of variant IDs cannot be empty.", 400);
            }

            var GetvariantIdsResult = await _mediator.Send(new GetProductVariantsByIdsQuery(request.variantIds));

            if (!GetvariantIdsResult.IsSuccess|| GetvariantIdsResult.Data==null)
            {
                return RequestRespones<bool>.Fail(GetvariantIdsResult.Message, 400);
            }


            return await _mediator.Send(new AddVariantsToProductCommand(request.productId, request.variantIds));
        }

      
    }
}