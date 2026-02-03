using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.UpdateVariants
{
    public record UpdateProductVariantCommand(
    int Id,
    decimal Price,string VariantName,
    int Stock,IFormFile ImageUrl
    
) : ICommand<RequestRespones<bool>>;

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
            
            var variant = await _variantRepo.GetByCriteriaQueryable(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (variant == null)
                return RequestRespones<bool>.Fail("There is no Variant with this id", 404);

           
            variant.Price = request.Price;
            variant.VariantName = request.VariantName;
            variant.Stock = request.Stock;
            _variantRepo.SaveInclude(variant,nameof(variant.Price),nameof(variant.VariantName),nameof(variant.Stock));







                await _variantRepo.SaveChanges();

            return RequestRespones<bool>.Success(true, 200, "Variant and its attributes updated successfully");
        }
    }
}
