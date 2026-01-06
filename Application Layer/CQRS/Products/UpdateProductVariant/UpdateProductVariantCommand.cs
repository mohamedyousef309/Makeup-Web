using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.UpdateVariants
{
    public record UpdateProductVariantCommand(
        IEnumerable<UpdateProductVariantDto> UpdateProductVariantDtos)
        : ICommand<RequestRespones<bool>>;

    public class UpdateProductVariantCommandHandler
        : IRequestHandler<UpdateProductVariantCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<ProductVariant> _variantRepo;

        public UpdateProductVariantCommandHandler(
            IGenaricRepository<ProductVariant> variantRepo)
        {
            _variantRepo = variantRepo;
        }

        public async Task<RequestRespones<bool>> Handle(
            UpdateProductVariantCommand request,
            CancellationToken cancellationToken)
        {
            var variantIds = request.UpdateProductVariantDtos
                .Select(v => v.Id)
                .ToList();

            var existingVariants = _variantRepo.GetAll()
                .Where(v => variantIds.Contains(v.Id))
                .ToList();

            if (!existingVariants.Any())
                return RequestRespones<bool>
                    .Fail("No variants found to update", 404);

            foreach (var variant in existingVariants)
            {
                var dto = request.UpdateProductVariantDtos
                    .First(v => v.Id == variant.Id);

                variant.VariantName = dto.VariantName;
                variant.VariantValue = dto.VariantValue;
                variant.Stock = dto.Stock;

                _variantRepo.SaveInclude(variant);
            }

            await _variantRepo.SaveChanges();

            return new RequestRespones<bool>(true);
        }
    }
}
