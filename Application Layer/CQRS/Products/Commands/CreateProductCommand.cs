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

namespace Application_Layer.CQRS.Products.Commands
{
    public class CreateProductCommand : CreateProductDto, IRequest<RequestRespones<ProductDto>>
    {
    }
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, RequestRespones<ProductDto>>
    {
        private readonly IGenaricRepository<Product> _productRepo;
        private readonly IGenaricRepository<ProductVariant> _variantRepo;

        public CreateProductHandler(
            IGenaricRepository<Product> productRepo,
            IGenaricRepository<ProductVariant> variantRepo)
        {
            _productRepo = productRepo;
            _variantRepo = variantRepo;
        }

        public async Task<RequestRespones<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var product = new Product
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Stock = request.Stock,
                    CategoryId = request.CategoryId,
                    IsActive = true
                };

                // Add product
                await _productRepo.addAsync(product);

                // Add variants if exist
                if (request.Variants != null && request.Variants.Any())
                {
                    foreach (var v in request.Variants)
                    {
                        var variant = new ProductVariant
                        {
                            Product = product,
                            VariantName = v.VariantName,
                            VariantValue = v.VariantValue,
                            Stock = v.Stock
                        };

                        await _variantRepo.addAsync(variant);
                    }
                }

                // Save changes once — منتظر الـ Transaction Middleware لعمل commit
                await _productRepo.SaveChanges();

                // Map result
                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    CategoryId = product.CategoryId,
                    IsActive = product.IsActive,
                    Variants = product.Variants?.Select(v => new ProductVariantDto
                    {
                        Id = v.Id,
                        VariantName = v.VariantName,
                        VariantValue = v.VariantValue,
                        Stock = v.Stock
                    }).ToList() ?? new System.Collections.Generic.List<ProductVariantDto>()
                };

                return RequestRespones<ProductDto>.Success(productDto, 201, "Product created successfully");
            }
            catch (System.Exception ex)
            {
                return RequestRespones<ProductDto>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
