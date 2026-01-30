using Domain_Layer.DTOs.ProductDtos;
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

namespace Application_Layer.CQRS.Products.Queries.GetProductsByIds
{
    public record GetProductsByIdsQuery(IEnumerable<int> ProductIds) : IRequest<RequestRespones<IEnumerable<ProductWithVariantsDto>>>;

    public class GetProductsByIdsQueryHandler : IRequestHandler<GetProductsByIdsQuery, RequestRespones<IEnumerable<ProductWithVariantsDto>>>
    {
        private readonly IGenaricRepository<Product> genaricRepository;

        public GetProductsByIdsQueryHandler(IGenaricRepository<Domain_Layer.Entites.Product> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<IEnumerable<ProductWithVariantsDto>>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
        {
            var Products = await genaricRepository.GetByCriteriaQueryable(p => request.ProductIds.Contains(p.Id))/*.Where(x=>x.Stock>0)*/
                 .Select(p => new ProductWithVariantsDto
                 {
                     Id = p.Id,
                     Name = p.Name,
                     Description = p.Description,
                     //Price = p.Price,
                     //Stock = p.Stock,
                     //CategoryId = p.CategoryId,
                     IsActive = p.IsActive,
                 }).ToListAsync(cancellationToken);
            if (!Products.Any())
            {
                return RequestRespones<IEnumerable<ProductWithVariantsDto>>.Fail("No products found for the provided IDs", 404);

            }
            return RequestRespones<IEnumerable<ProductWithVariantsDto>>.Success(Products);
        }
    }
}
