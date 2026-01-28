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

namespace Application_Layer.CQRS.Products.Queries.GetProductVariantsByProductid
{
    public record GetProductVariantsByProductidQuery(int Productid):ICommand<RequestRespones<IEnumerable<ProductVariantDto>>>;

    public class GetProductVariantsByProductidQueryHandler : IRequestHandler<GetProductVariantsByProductidQuery, RequestRespones<IEnumerable<ProductVariantDto>>>
    {
        private readonly IGenaricRepository<ProductVariant> genaricRepository;

        public GetProductVariantsByProductidQueryHandler(IGenaricRepository<ProductVariant> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<IEnumerable<ProductVariantDto>>> Handle(GetProductVariantsByProductidQuery request, CancellationToken cancellationToken)
        {
            var variants = await genaricRepository.GetAll().Where(x => x.ProductId == request.Productid)
                .Select(x => new ProductVariantDto
                {
                    Id = x.Id,
                    //VariantName = x.VariantName,
                    //VariantValue = x.VariantValue,
                    Price=x.Price,
                    Stock=x.Stock
                }).ToListAsync();

            if (!variants.Any())
            {
                return RequestRespones<IEnumerable<ProductVariantDto>>.Fail("No variants found for the specified product", 404);
            }

            return RequestRespones<IEnumerable<ProductVariantDto>>.Success(variants, 200);
        }
    }
}
