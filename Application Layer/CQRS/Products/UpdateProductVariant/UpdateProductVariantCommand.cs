using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.UpdateVariants
{
    public record UpdateProductVariantCommand(
    int Id,
    decimal Price,
    int Stock,
    List<int> SelectedAttributeValueIds 
) : ICommand<RequestRespones<bool>>;

    public class UpdateProductVariantCommandHandler
    : IRequestHandler<UpdateProductVariantCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<ProductVariant> _variantRepo;
        
        private readonly IGenaricRepository<VariantAttributeValue> _variantAttrRepo;

        public UpdateProductVariantCommandHandler(
            IGenaricRepository<ProductVariant> variantRepo,
            IGenaricRepository<VariantAttributeValue> variantAttrRepo)
        {
            _variantRepo = variantRepo;
            _variantAttrRepo = variantAttrRepo;
        }

        public async Task<RequestRespones<bool>> Handle(
            UpdateProductVariantCommand request,
            CancellationToken cancellationToken)
        {
            
            var variant = await _variantRepo.GetByCriteriaQueryable(x => x.Id == request.Id)
                .Include(v => v.ProductVariantAttributeValues)
                .FirstOrDefaultAsync(cancellationToken);

            if (variant == null)
                return RequestRespones<bool>.Fail("There is no Variant with this id", 404);

           
            variant.Price = request.Price;
           
            if (variant.ProductVariantAttributeValues.Any())
            {
                _variantAttrRepo.DeleteRange(variant.ProductVariantAttributeValues);
            }

            
            if (request.SelectedAttributeValueIds != null && request.SelectedAttributeValueIds.Any())
            {
                var newRelations = request.SelectedAttributeValueIds.Select(valId => new VariantAttributeValue
                {
                    ProductVariantId = variant.Id,
                    AttributeValueId = valId
                }).ToList();

                await _variantAttrRepo.AddRangeAsync(newRelations);
            }

           
            await _variantRepo.SaveChanges();

            return RequestRespones<bool>.Success(true, 200, "Variant and its attributes updated successfully");
        }
    }
}
