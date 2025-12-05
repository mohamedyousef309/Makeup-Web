using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenaricRepository<Product> _productRepo;
        private readonly IGenaricRepository<ProductVariant> _variantRepo;
        private readonly IunitofWork _unitOfWork;

        public ProductService(
            IGenaricRepository<Product> productRepo,
            IGenaricRepository<ProductVariant> variantRepo,
            IunitofWork unitOfWork)
        {
            _productRepo = productRepo;
            _variantRepo = variantRepo;
            _unitOfWork = unitOfWork;
        }

        
        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
            };

            await _productRepo.addAsync(product);

            // Add variants if exist
            if (dto.Variants != null && dto.Variants.Any())
            {
                foreach (var v in dto.Variants)
                {
                    await _variantRepo.addAsync(new ProductVariant
                    {
                        Product = product,
                        VariantName = v.VariantName,
                        VariantValue = v.VariantValue,
                        Stock = v.Stock
                    });
                }
            }

            await _unitOfWork.CommitTransactionAsync();
            await _productRepo.SaveChanges();

            return MapToProductDto(product);
        }

       
        public async Task<ProductDto> UpdateProductAsync(UpdateProductDto dto)
        {
            var product = await _productRepo.GetByCriteriaAsync(x => x.Id == dto.Id);

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.IsActive = dto.IsActive;
            product.CategoryId = dto.CategoryId;
            

         
            if (dto.Variants != null)
            {
                foreach (var v in dto.Variants)
                {
                    var existingVariant = await _variantRepo.GetByCriteriaAsync(x => x.Id == v.Id);

                    existingVariant.VariantName = v.VariantName;
                    existingVariant.VariantValue = v.VariantValue;
                    existingVariant.Stock = v.Stock;

                    _variantRepo.Update(existingVariant);
                }
            }

            _productRepo.Update(product);

            await _productRepo.SaveChanges();
            return MapToProductDto(product);
        }

       
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepo.GetByCriteriaAsync(x => x.Id == id);

            if (product == null)
                return false;

            _productRepo.Delete(product);
            await _productRepo.SaveChanges();

            return true;
        }

        
        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var productQuery = _productRepo.GetByIdQueryable(id);

            var product = productQuery
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
                .FirstOrDefault();

            return product!;
        }

    
        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
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

            return products;
        }

     
        public async Task<bool> ExistsAsync(int id)
        {
            return await _productRepo.ExistsAsync(id);
        }

     
        private ProductDto MapToProductDto(Product p)
        {
            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                Variants = p.Variants.Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    VariantName = v.VariantName,
                    VariantValue = v.VariantValue,
                    Stock = v.Stock
                }).ToList()
            };
        }
    }
}
