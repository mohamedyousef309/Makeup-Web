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
            string AllProducts_cacheKey = "AllProducts";


            product.Name = dto.Name;
            product.Description = dto.Description;
            product.CategoryId = dto.CategoryId;
            product.IsActive = dto.IsActive;

            if (request.Dto.ImageFile != null)
            {
                string? imageUrl = null;

                var originalFileName = Path.GetFileName(request.Dto.ImageFile.FileName);

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
                    imageUrl = attachmentService.UploadImage(request.Dto.ImageFile, "Images/ProductImages");
                }

                product.ImageUrl = imageUrl; 

            }


            _productRepo.SaveInclude(product);
            await _productRepo.SaveChanges();

            memoryCache.Remove(cacheKey);
            memoryCache.Remove(AllProducts_cacheKey);

            return RequestRespones<bool>.Success(true, 200, "Product updated successfully");
        }
    }
}
