using Domain_Layer.DTOs.ProductDtos;
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
   
    public record GetProductByIdQuery(int Id) : IRequest<RequestRespones<ProductDto>>;

    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, RequestRespones<ProductDto>>
    {
        private readonly IGenaricRepository<Product> _productRepo;

        public GetProductByIdHandler(IGenaricRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<RequestRespones<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
               
                var query = _productRepo.GetAll()
                    .Include(p => p.Variants)
                    .AsQueryable();

                
                var product = await query
                    .Where(p => p.Id == request.Id)
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Stock = p.Stock,
                        ProductStock = p.productStock,
                        CategoryId = p.CategoryId,
                        IsActive = p.IsActive,
                        Variants = p.Variants.Select(v => new ProductVariantDto
                        {
                            Id = v.Id,
                            VariantName = v.VariantName,
                            VariantValue = v.VariantValue,
                            Stock = v.Stock
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (product == null)
                    return RequestRespones<ProductDto>.Fail($"Product with Id {request.Id} not found.", 404);

                return RequestRespones<ProductDto>.Success(product, 200, "Product retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                return RequestRespones<ProductDto>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
