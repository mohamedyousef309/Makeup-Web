using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Interfaces.ServiceInterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application_Layer.CQRS.Products.Commands.UpdateProduct
{
    
    public record UpdateProductCommand(UpdateProductDto Dto)
        : ICommand<RequestRespones<bool>>;

   
    public class UpdateProductHandler
        : IRequestHandler<UpdateProductCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Product> _productRepo;
        private readonly IAttachmentService attachmentService;
        private readonly IMemoryCache memoryCache;

        public UpdateProductHandler(IGenaricRepository<Product> productRepo,IAttachmentService attachmentService,IMemoryCache memoryCache)
        {
            _productRepo = productRepo;
            this.attachmentService = attachmentService;
            this.memoryCache = memoryCache;
        }

        public async Task<RequestRespones<bool>> Handle(UpdateProductCommand request,CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var product = await _productRepo.GetByCriteriaQueryable(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (product == null)
                return RequestRespones<bool>
                    .Fail("Product not found", 404);

            string cacheKey = $"ProductDetails_{product.Id}";


            product.Name = dto.Name;
            product.Description = dto.Description;
            product.CategoryId = dto.CategoryId;
            product.IsActive = dto.IsActive;

            if (dto.ImageFile!=null)
            {
                var newImageUrl = attachmentService.UploadImage(dto.ImageFile, "Images");

                if (!string.IsNullOrEmpty(newImageUrl))
                {
                    var oldImageUrl = product.ImageUrl;

                    product.ImageUrl = newImageUrl;


                    if (!string.IsNullOrEmpty(oldImageUrl))
                    {
                        attachmentService.DeleteImage(oldImageUrl);
                    }
                }


            }

            _productRepo.SaveInclude(product);
            await _productRepo.SaveChanges();

            memoryCache.Remove(cacheKey);

            return RequestRespones<bool>.Success(true, 200, "Product updated successfully");
        }
    }
}
