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
            // 🔹 IQueryable
            IQueryable<ProductVariant> query = _variantRepo.GetAll().AsQueryable();

            // 🔹 Filter حسب VariantId
            query = query.Where(v => v.Id == request.VariantId);

            // 🔹 Filter حسب ProductId لو متوفر
            if (request.ProductId.HasValue)
            {
                query = query.Where(v => v.ProductId == request.ProductId.Value);
            }

            // 🔹 Projection لـ DTO
            var variant = await query
                .Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    VariantName = v.VariantName,
                    VariantValue = v.VariantValue,
                    Stock = v.Stock
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (variant == null)
                return RequestRespones<ProductVariantDto>.Fail("Product variant not found.", 404);

            return RequestRespones<ProductVariantDto>.Success(variant, 200, "Product variant retrieved successfully.");
        }
    }
}
