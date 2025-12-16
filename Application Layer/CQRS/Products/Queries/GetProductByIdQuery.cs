using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Interfaces.ServiceInterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
                var product = await _productRepo.GetByCriteriaQueryable(x => x.Id == request.Id).
                    Select(x=> new ProductDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        Stock = x.Stock,
                        CategoryId = x.CategoryId,
                        IsActive = x.IsActive,
                        Variants = x.Variants.Select(v => new ProductVariantDto
                        {
                            Id = v.Id,
                            VariantName = v.VariantName,
                            VariantValue = v.VariantValue,
                            Stock = v.Stock
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

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
