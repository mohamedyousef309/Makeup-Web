using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Queries
{
    // Query لجلب Variant واحد
    public record GetProductVariantByIdQuery(
        int VariantId,
        int? ProductId = null // اختياري للتأكد إنه مرتبط بمنتج محدد
    ) : IRequest<RequestRespones<ProductVariantDto>>;

    public class GetProductVariantByIdHandler : IRequestHandler<GetProductVariantByIdQuery, RequestRespones<ProductVariantDto>>
    {
        private readonly IGenaricRepository<ProductVariant> _variantRepo;

        public GetProductVariantByIdHandler(IGenaricRepository<ProductVariant> variantRepo)
        {
            _variantRepo = variantRepo;
        }

        public async Task<RequestRespones<ProductVariantDto>> Handle(
            GetProductVariantByIdQuery request,
            CancellationToken cancellationToken)
        {
            var query = _variantRepo.GetAll().AsQueryable();

            query = query.Where(v => v.Id == request.VariantId);

            if (request.ProductId.HasValue)
            {
                query = query.Where(v => v.ProductId == request.ProductId.Value);
            }

            var variant = await query
                .Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    ProductId=v.ProductId,  
                    //VariantName = v.VariantName,
                    //VariantValue = v.VariantValue,
                    Price = v.Price,
                    Stock = v.Stock,
                    
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (variant == null)
                return RequestRespones<ProductVariantDto>.Fail("Product variant not found.", 404);

            return RequestRespones<ProductVariantDto>.Success(variant, 200, "Product variant retrieved successfully.");
        }
    }
}
