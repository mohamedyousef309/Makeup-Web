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
    public record UpdateProductVariantCommand(int Id, string VariantName, string VariantValue, decimal Price) :ICommand<RequestRespones<bool>>;

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
            var Variant = await _variantRepo.GetByCriteriaQueryable(x=>x.Id==request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (Variant == null) 
            {
                return RequestRespones<bool>.Fail("There is no Variant with this id", 404);
            }

            Variant.VariantValue=request.VariantValue;
            Variant.Price=request.Price;
            Variant.VariantName=request.VariantName;

            _variantRepo.SaveInclude(Variant,nameof(Variant.VariantName),nameof(Variant.VariantValue),nameof(Variant.Price));

            await _variantRepo.SaveChanges();

            return RequestRespones<bool>.Success(true);


        }
    }
}
