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
    public record GetProductsByIdsQuery(IEnumerable<int> ProductIds) : IRequest<RequestRespones<IEnumerable<ProductDto>>>;

    public class GetProductsByIdsQueryHandler : IRequestHandler<GetProductsByIdsQuery, RequestRespones<IEnumerable<ProductDto>>>
    {
        private readonly IGenaricRepository<Product> genaricRepository;

        public GetProductsByIdsQueryHandler(IGenaricRepository<Domain_Layer.Entites.Product> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<IEnumerable<ProductDto>>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
        {
            var Products = await genaricRepository.GetByCriteriaQueryable(p => request.ProductIds.Contains(p.Id))
                 .Select(p => new ProductDto
                 {
                     Id = p.Id,
                     Name = p.Name,
                     Description = p.Description,
                     Price = p.Price,
                     Stock = p.Stock,
                     CategoryId = p.CategoryId,
                     IsActive = p.IsActive,
                     ProductStock = p.productStock,
                 }).ToListAsync(cancellationToken);
            if (!Products.Any())
            {
                return RequestRespones<IEnumerable<ProductDto>>.Fail("No products found for the provided IDs", 404);

            }
            return RequestRespones<IEnumerable<ProductDto>>.Success(Products);
        }
    }
}
