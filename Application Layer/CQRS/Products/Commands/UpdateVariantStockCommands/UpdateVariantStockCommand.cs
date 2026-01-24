using Application_Layer.CQRS.Products.Commands.UpdateVariants;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application_Layer.CQRS.Products.Commands.UpdateVariantStock
{
    
    public record UpdateVariantStockCommand(int Id, int NewStock) : ICommand<RequestRespones<bool>>;

    public class UpdateVariantStockCommandHandler
        : IRequestHandler<UpdateVariantStockCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<ProductVariant> _variantRepo;

        public UpdateVariantStockCommandHandler(IGenaricRepository<ProductVariant> variantRepo)
        {
            _variantRepo = variantRepo;
        }

        public async Task<RequestRespones<bool>> Handle(UpdateVariantStockCommand request, CancellationToken cancellationToken)
        {
           
            var variant = await _variantRepo.GetByCriteriaQueryable(x => x.Id == request.Id)
                                           .FirstOrDefaultAsync(cancellationToken);

            if (variant == null)
            {
                return RequestRespones<bool>.Fail("Variant not found", 404);
            }

            variant.Stock = request.NewStock;

           
            _variantRepo.SaveInclude(variant, nameof(variant.Stock));

            await _variantRepo.SaveChanges();

            return RequestRespones<bool>.Success(true, 200, "Stock updated successfully.");
        }
    }
}