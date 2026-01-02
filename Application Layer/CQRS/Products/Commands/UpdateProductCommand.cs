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
using global::Application_Layer.CQRS.Products.Commands.Application_Layer.CQRS.Products.Commands;

namespace Application_Layer.CQRS.Products.Commands
{
    using Domain_Layer.DTOs.ProductDtos;
    using Domain_Layer.Respones;
    using global::Application_Layer.CQRS.Products.Commands.Application_Layer.CQRS.Products.Commands;
    using MediatR;

    namespace Application_Layer.CQRS.Products.Commands
    {
        public record UpdateProductCommand(UpdateProductDto UpdateProductDto)
            : IRequest<RequestRespones<ProductDto>>;
    }


    // Handler


    public class UpdateProductHandler
         
        : IRequestHandler<UpdateProductCommand, RequestRespones<ProductDto>>
    {
        private readonly IGenaricRepository<Product> _productRepo;
        private readonly IGenaricRepository<ProductVariant> _variantRepo;

        public UpdateProductHandler(
            IGenaricRepository<Product> productRepo,
            IGenaricRepository<ProductVariant> variantRepo)
        {
            _productRepo = productRepo;
            _variantRepo = variantRepo;
        }

        public async Task<RequestRespones<ProductDto>> Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.UpdateProductDto;

                var product = _productRepo.GetAll()
                    .FirstOrDefault(p => p.Id == dto.Id);

                if (product == null)
                    return RequestRespones<ProductDto>
                        .Fail($"Product with Id {dto.Id} not found.", 404);

                // Update product fields
                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Price = dto.Price;
                product.Stock = dto.Stock;
                product.CategoryId = dto.CategoryId;
                product.IsActive = dto.IsActive;

                // Update / Add variants
                if (dto.Variants != null)
                {
                    foreach (var v in dto.Variants)
                    {
                        var existingVariant = _variantRepo.GetAll()
                            .FirstOrDefault(ev => ev.Id == v.Id);

                        if (existingVariant != null)
                        {
                            existingVariant.VariantName = v.VariantName;
                            existingVariant.VariantValue = v.VariantValue;
                            existingVariant.Stock = v.Stock;
                        }
                        else
                        {
                            var newVariant = new ProductVariant
                            {
                                ProductId = product.Id,
                                VariantName = v.VariantName,
                                VariantValue = v.VariantValue,
                                Stock = v.Stock
                            };

                            await _variantRepo.addAsync(newVariant);
                        }
                    }
                }

                _productRepo.SaveInclude(product);
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
                    Variants = _variantRepo.GetAll()
                        .Where(v => v.ProductId == product.Id)
                        .Select(v => new ProductVariantDto
                        {
                            Id = v.Id,
                            VariantName = v.VariantName,
                            VariantValue = v.VariantValue,
                            Stock = v.Stock
                        }).ToList()
                };

                return RequestRespones<ProductDto>
                    .Success(productDto, 200, "Product updated successfully.");
            }
            catch (Exception ex)
            {
                return RequestRespones<ProductDto>
                    .Fail($"Error: {ex.Message}", 500);
            }
        }
    }

}


