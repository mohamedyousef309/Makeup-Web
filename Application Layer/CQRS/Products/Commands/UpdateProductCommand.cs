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
    public class UpdateProductCommand : UpdateProductDto, IRequest<RequestRespones<ProductDto>>
    {
    }

    // Handler
    

    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, RequestRespones<ProductDto>>
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

        public async Task<RequestRespones<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                var product = _productRepo.GetAll().FirstOrDefault(p => p.Id == request.Id);
                if (product == null)
                    return RequestRespones<ProductDto>.Fail($"Product with Id {request.Id} not found.", 404);

                
                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;
                product.Stock = request.Stock;
                product.CategoryId = request.CategoryId;
                product.IsActive = request.IsActive;

                
                if (request.Variants != null)
                {
                    foreach (var v in request.Variants)
                    {
                        var existingVariant = _variantRepo.GetAll().FirstOrDefault(ev => ev.Id == v.Id);
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

                return RequestRespones<ProductDto>.Success(productDto, 200, "Product updated successfully.");
            }
            catch (System.Exception ex)
            {
                return RequestRespones<ProductDto>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }

}


