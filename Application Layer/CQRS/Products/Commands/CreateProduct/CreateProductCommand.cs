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
    public record CreateProductCommand(CreateProductDto CreateProductDto): ICommand<RequestRespones<bool>>;
   
    public class CreateProductHandler: IRequestHandler<CreateProductCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Product> _productRepo;

        public CreateProductHandler(IGenaricRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<RequestRespones<bool>> Handle(CreateProductCommand request,CancellationToken cancellationToken)
        {
            
            

                var product = new Product
                {
                    Name = request.CreateProductDto.Name,
                    Description = request.CreateProductDto.Description,
                    Price = request.CreateProductDto.Price,
                    Stock = request.CreateProductDto.Stock,
                    CategoryId = request.CreateProductDto.CategoryId,
                    IsActive = true,
                    productStock = request.CreateProductDto.Stock,
                    
                };

                await _productRepo.addAsync(product);

                

                await _productRepo.SaveChanges();


                return RequestRespones<bool>
                    .Success(true,Message:"Product created successfully");
            
            
        }
    }
}
