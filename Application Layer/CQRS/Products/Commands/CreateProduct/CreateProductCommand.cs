using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Interfaces.ServiceInterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.CreateProduct
{
    public record CreateProductCommand(CreateProductDto CreateProductDto): ICommand<RequestRespones<ProductDto>>;
   
    public class CreateProductHandler: IRequestHandler<CreateProductCommand, RequestRespones<ProductDto>>
    {
        private readonly IGenaricRepository<Product> _productRepo;

        public CreateProductHandler(IGenaricRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<RequestRespones<ProductDto>> Handle(CreateProductCommand request,CancellationToken cancellationToken)
        {
            
            

                var product = new Product
                {
                    Name = request.CreateProductDto.Name,
                    Description = request.CreateProductDto.Description,
                    Price = request.CreateProductDto.Price,
                    Stock = request.CreateProductDto.Stock,
                    CategoryId = request.CreateProductDto.CategoryId,
                    IsActive = true,
                    
                };

                await _productRepo.addAsync(product);

                

                await _productRepo.SaveChanges();


            var resultDto = new ProductDto
            {
                Id = product.Id, 
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
            
                Variants = new List<ProductVariantDto>()
            };

            return RequestRespones<ProductDto>
                .Success(resultDto, Message: "Product created successfully");


        }
    }
}
