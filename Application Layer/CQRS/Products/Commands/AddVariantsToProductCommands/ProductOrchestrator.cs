using Application_Layer.CQRS.Products.Commands.AddVariantsToProduct;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;

namespace Application_Layer.Orchestrators
{
    public class ProductOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IGenaricRepository<Product> _productRepo;

        public ProductOrchestrator(IMediator mediator, IGenaricRepository<Product> productRepo)
        {
            _mediator = mediator;
            _productRepo = productRepo;
        }

        public async Task<RequestRespones<bool>> LinkVariantsToProductAsync(int productId, List<int> variantIds)
        {
          
            var productExists = await _productRepo.ExistsAsync(productId);

            if (!productExists)
            {
                return RequestRespones<bool>.Fail($"Product with ID {productId} does not exist.", 404);
            }

           
            if (variantIds == null || !variantIds.Any())
            {
                return RequestRespones<bool>.Fail("The list of variant IDs cannot be empty.", 400);
            }

           
            return await _mediator.Send(new AddVariantsToProductCommand(productId, variantIds));
        }
    }
}