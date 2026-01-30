using Application_Layer.Services;
using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
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

namespace Application_Layer.CQRS.Products.Commands.CreateProduct
{
    public record CreateProductCommand(CreateProductDto CreateProductDto): ICommand<RequestRespones<ProductWithVariantsDto>>;
   
    public class CreateProductHandler: IRequestHandler<CreateProductCommand, RequestRespones<ProductWithVariantsDto>>
    {
        private readonly IGenaricRepository<Product> _productRepo;
        private readonly IAttachmentService attachmentService;

        public CreateProductHandler(IGenaricRepository<Product> productRepo,IAttachmentService attachmentService)
        {
            _productRepo = productRepo;
            this.attachmentService = attachmentService;
        }

        public async Task<RequestRespones<ProductWithVariantsDto>> Handle(CreateProductCommand request,CancellationToken cancellationToken)
        {
            string? imageUrl = null;
            var imageFile = request.CreateProductDto.Productpecture;

            if (imageFile != null)
            {
                var originalFileName = Path.GetFileName(imageFile.FileName);

                var existingPath = await _productRepo.GetAll()
                    .Where(p => p.ImageUrl != null && p.ImageUrl.EndsWith("_" + originalFileName))
                    .Select(p => p.ImageUrl)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingPath != null)
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingPath.TrimStart('/'));
                    if (File.Exists(fullPath))
                    {
                        imageUrl = existingPath; 
                    }
                }

                if (imageUrl == null)
                {
                    imageUrl = attachmentService.UploadImage(imageFile, "Images/ProductImages");
                }
            }


            var product = new Product
            {
               Name = request.CreateProductDto.Name,
               Description = request.CreateProductDto.Description,
               CategoryId = request.CreateProductDto.CategoryId,
               ImageUrl= imageUrl,
               IsActive = true,
                    
            };

                await _productRepo.addAsync(product);

                

                await _productRepo.SaveChanges();


            var resultDto = new ProductWithVariantsDto
            {
                Id = product.Id, 
                Name = product.Name,
                Description = product.Description,
                IsActive = product.IsActive,

                Variants = new List<ProductVariantDto>()
            };

            return RequestRespones<ProductWithVariantsDto>
                .Success(resultDto, Message: "Product created successfully");


        }
    }
}
