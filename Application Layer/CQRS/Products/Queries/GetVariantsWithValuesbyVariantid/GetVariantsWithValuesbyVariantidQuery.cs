using Domain_Layer.DTOs.Attribute;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Queries.GetVariantsWithValuesbyVariantid
{
    public record GetVariantsWithValuesbyVariantidQuery(int Variantid):IRequest<RequestRespones<VariantWithAtrputeValuesDto>>;

    public class GetVariantsWithValuesbyVariantidQueryHandler : IRequestHandler<GetVariantsWithValuesbyVariantidQuery, RequestRespones<VariantWithAtrputeValuesDto>>
    {
        private readonly IGenaricRepository<ProductVariant> genaricRepository;

        public GetVariantsWithValuesbyVariantidQueryHandler(IGenaricRepository<ProductVariant> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<VariantWithAtrputeValuesDto>> Handle(GetVariantsWithValuesbyVariantidQuery request, CancellationToken cancellationToken)
        {
            var productVariant = await genaricRepository
                .GetByCriteriaQueryable(x => x.Id == request.Variantid)
                .Select(x => new VariantWithAtrputeValuesDto
                {
                    Id = x.Id,
                    Price = x.Price,
                    Stock = x.Stock,
                    Productid=x.ProductId,
                    ImageUrl= x.ImageUrl,
                    VariantName= x.VariantName,
                    attributeValues = x.ProductVariantAttributeValues
                    .GroupBy(av => av.AttributeValue.Attribute.Id)
                    .Select(g => new AttributeGroupDto
                    {
                        AttributeId = g.Key,
                        Name = g.First().AttributeValue.Attribute.Name,
                        Values = g.Select(v => new AttributeValueSelectionDto
                        {
                            Id = v.AttributeValue.Id,
                            Value = v.AttributeValue.Value
                        })
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);


            if (productVariant == null)
            {
                return RequestRespones<VariantWithAtrputeValuesDto>.Fail("This ProductVariant was not found",404);
            }

            return RequestRespones<VariantWithAtrputeValuesDto>.Success(productVariant);
        }
    }
}
