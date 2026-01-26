using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using Domain_Layer.Interfaces.Abstraction;

namespace Application_Layer.CQRS.Products.Commands.AddVariantsToProduct
{
  
    public record AddVariantsToProductCommand(int ProductId, IEnumerable<int> VariantIds) : ICommand<RequestRespones<bool>>;

    public class AddVariantsToProductCommandHandler : IRequestHandler<AddVariantsToProductCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<ProductVariant> _variantRepo;

        public AddVariantsToProductCommandHandler(IGenaricRepository<ProductVariant> variantRepo)
        {
            _variantRepo = variantRepo;
        }

        public async Task<RequestRespones<bool>> Handle(AddVariantsToProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                var rowsAffected = await _variantRepo.GetByCriteriaQueryable(v => request.VariantIds.Contains(v.Id))
                    .ExecuteUpdateAsync(setters=> setters
                        .SetProperty(v => v.ProductId, request.ProductId));

                if (rowsAffected == 0)
                {
                    return RequestRespones<bool>.Fail("No valid variants found to link.", 404);
                }

                return RequestRespones<bool>.Success(true, 200, "Product variants updated successfully.");
            }
            catch (Exception)
            {
                
                return RequestRespones<bool>.Fail("An unexpected error occurred during the update process.", 500);
            }
        }
    }
}