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
using System.Windows.Input;

namespace Application_Layer.CQRS.Products.Commands.Createvariants
{
    public record CreatevariantsCommand(int productid,IEnumerable<CreateProductVariantDto> UpdateProductVariantDtos):ICommand<RequestRespones<bool>>;

    public class CreatevariantsCommandHandler : IRequestHandler<CreatevariantsCommand, RequestRespones<bool>>
{
        private readonly IGenaricRepository<ProductVariant> genaricRepository;
        private readonly IAttachmentService attachmentService;

        public CreatevariantsCommandHandler(IGenaricRepository<ProductVariant> genaricRepository,IAttachmentService attachmentService)
        {
            this.genaricRepository = genaricRepository;
            this.attachmentService = attachmentService;
        }
        public async Task<RequestRespones<bool>> Handle(CreatevariantsCommand request, CancellationToken cancellationToken)
        {

            var variants = new List<ProductVariant>();

            var currentRequestImages = new Dictionary<string, string>();

            foreach (var item in request.UpdateProductVariantDtos)
            {
                string? imageUrl = null;

                if (item.Variantpecture != null)
                {
                    var originalFileName = Path.GetFileName(item.Variantpecture.FileName);

                    if (currentRequestImages.ContainsKey(originalFileName))
                    {
                        imageUrl = currentRequestImages[originalFileName];
                    }

                    else
                    {
                        var existingPath = await genaricRepository.GetAll()
                            .Where(v => v.ImageUrl != null && v.ImageUrl.EndsWith("_" + originalFileName))
                            .Select(v => v.ImageUrl)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (existingPath!=null)
                        {
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingPath.TrimStart('/'));
                            if (File.Exists(fullPath))
                            {
                                imageUrl = existingPath;
                                currentRequestImages[originalFileName] = imageUrl;
                            }

                        }

                        if (imageUrl == null)
                        {
                            imageUrl = attachmentService.UploadImage(item.Variantpecture, "Images/VariantImges");
                        }

                        if (imageUrl != null)
                        {
                            currentRequestImages[originalFileName] = imageUrl;
                        }

                    }
                }

                variants.Add(new ProductVariant
                {
                    VariantName= item.VariantName,
                    ProductId = request.productid,
                    Price = item.Price,
                    Stock = item.Stock,
                    ImageUrl = imageUrl,
                    ProductVariantAttributeValues = item.AttributeValueId.Select(avId => new VariantAttributeValue
                    {
                        AttributeValueId = avId
                    }).ToList()
                });
            }




            await genaricRepository.AddRangeAsync(variants);

            await genaricRepository.SaveChanges();  

            return new RequestRespones<bool>(true);

        }
    }
}
