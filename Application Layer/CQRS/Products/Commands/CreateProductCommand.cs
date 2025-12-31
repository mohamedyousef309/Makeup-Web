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
    public record CreateProductCommand(CreateProductDto CreateProductDto)
       : IRequest<RequestRespones<ProductDto>>;
    public class CreateProductHandler
        : IRequestHandler<CreateProductCommand, RequestRespones<ProductDto>>
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

        public async Task<RequestRespones<ProductDto>> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.CreateProductDto;

                var product = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    Stock = dto.Stock,
                    CategoryId = dto.CategoryId,
                    IsActive = true
                };

                await _productRepo.addAsync(product);

                if (dto.Variants != null && dto.Variants.Any())
                {
                    foreach (var v in dto.Variants)
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

                await _productRepo.SaveChanges();

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
                    }).ToList() ?? new List<ProductVariantDto>()
                };

                return RequestRespones<ProductDto>
                    .Success(productDto, 201, "Product created successfully");
            }
            catch (Exception ex)
            {
                return RequestRespones<ProductDto>
                    .Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
