using Domain_Layer.DTOs.Attribute;
using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Queries
{
   
    public record GetProductByIdQuery(int Id) : IRequest<RequestRespones<ProductDetailsDto>>;

    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, RequestRespones<ProductDetailsDto>>
    {
        private readonly IGenaricRepository<Product> _productRepo;

        public GetProductByIdHandler(IGenaricRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<RequestRespones<ProductDetailsDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
               
                var product = await _productRepo.GetByCriteriaQueryable(p => p.Id == request.Id).Select(p => new ProductDetailsDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,

                    AllOptions = p.Variants.SelectMany(v => v.ProductVariantAttributeValues)
                    .Select(va => va.AttributeValue).
                    GroupBy(av => av.Attribute.Name)
                    .Select(g => new AttributeGroupDto
                    {
                        Name = g.Key,
                        Values = g.Select(v => new AttributeValueSelectionDto
                        {
                            Id = v.Id,
                            Value = v.Value
                        })
                    .Distinct() 
                    .ToList()
                    }).ToList(),
                    Variants = p.Variants.Select(v => new ProductVariantDto
                    {
                        Id = v.Id,
                        ProductId = v.ProductId,
                        Price = v.Price,
                        Stock = v.Stock,
                        VariantImage= v.ImageUrl,



                        SelectedAttributes = v.ProductVariantAttributeValues.Select(pva => new AttributeValueResponseDto
                        {
                            Id = pva.AttributeValueId,
                            AttributeName = pva.AttributeValue.Attribute.Name,
                            Value = pva.AttributeValue.Value
                        }).ToList()
                    }).ToList()
                }).FirstOrDefaultAsync(); // تجميع باسم الخاصية (Color, Size)









                if (product == null)
                    return RequestRespones<ProductDetailsDto>.Fail($"Product with Id {request.Id} not found.", 404);

                return RequestRespones<ProductDetailsDto>.Success(product, 200, "Product retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                return RequestRespones<ProductDetailsDto>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
