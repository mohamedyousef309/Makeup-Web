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
    public record GetAllProductsQuery : IRequest<RequestRespones<List<ProductDto>>>;



    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, RequestRespones<List<ProductDto>>>
    {
        private readonly IGenaricRepository<Product> _productRepo;

        public GetAllProductsHandler(IGenaricRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<RequestRespones<List<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // جلب كل المنتجات
                var products = _productRepo.GetAll()
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Stock = p.Stock,
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
                    .ToList();

                return RequestRespones<List<ProductDto>>.Success(products, 200, "Products retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                return RequestRespones<List<ProductDto>>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
