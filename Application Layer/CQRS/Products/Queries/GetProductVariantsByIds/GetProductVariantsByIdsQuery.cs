using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Queries.GetProductVariantsByIds
{
    public record GetProductVariantsByIdsQuery(IEnumerable<int> VariantId):ICommand<RequestRespones<IEnumerable<VariantDbDto>>>;

    public class GetProductVariantsByIdsQueryHandler : IRequestHandler<GetProductVariantsByIdsQuery, RequestRespones<IEnumerable<VariantDbDto>>>
    {
        private readonly IGenaricRepository<ProductVariant> genaricRepository;

        public GetProductVariantsByIdsQueryHandler(IGenaricRepository<ProductVariant> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<IEnumerable<VariantDbDto>>> Handle(GetProductVariantsByIdsQuery request, CancellationToken cancellationToken)
        {
            var ProductVariants = await genaricRepository.GetByCriteriaQueryable(v=>request.VariantId.Contains(v.Id)).
                Select(x => new VariantDbDto
                {
                    id=x.Id,
                    price=x.Price,
                    productid=x.Product.Id,
                    ProductName =x.Product.Name,
                    //VariantValue = x.VariantValue,
                }).ToListAsync(cancellationToken);

            if (!ProductVariants.Any())
            {
                return RequestRespones<IEnumerable<VariantDbDto>>.Fail("No product variants found for the provided IDs.", 404);
            }

            return RequestRespones<IEnumerable<VariantDbDto>>.Success(ProductVariants, 200, "Product variants retrieved successfully.");
        }
    }
}
