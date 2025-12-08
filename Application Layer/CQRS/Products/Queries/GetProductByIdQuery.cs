using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Interfaces.ServiceInterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                // جلب المنتج مع Variants
                var product = await _productRepo.GetByCriteriaAsync(x => x.Id == request.Id);

                if (product == null)
                    return RequestRespones<ProductDto>.Fail($"Product with Id {request.Id} not found.", 404);

                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    CategoryId = product.CategoryId,
                    IsActive = product.IsActive,
                    Variants = product.Variants.Select(v => new ProductVariantDto
                    {
                        Id = v.Id,
                        VariantName = v.VariantName,
                        VariantValue = v.VariantValue,
                        Stock = v.Stock
                    }).ToList()
                };

                return RequestRespones<ProductDto>.Success(productDto, 200, "Product retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                return RequestRespones<ProductDto>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
