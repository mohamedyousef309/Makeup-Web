using Domain_Layer.DTOs;
using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Queries
{
   
    public record GetAllProductsQuery(
        int PageSize = 10,
        int PageIndex = 1,
        string? SortBy = "id",
        string? SortDir = "asc",
        string? SearchTerm = null
    ) : IRequest<RequestRespones<PaginatedListDto<ProductDto>>>;

    
    public class GetAllProductsHandler : BaseQueryHandler,
        IRequestHandler<GetAllProductsQuery, RequestRespones<PaginatedListDto<ProductDto>>>
    {
        private readonly IGenaricRepository<Product> _productRepo;

        public GetAllProductsHandler(IGenaricRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<RequestRespones<PaginatedListDto<ProductDto>>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {

            var query = _productRepo.GetAll();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = ApplySearch(query, request.SearchTerm, p => p.Name);
            }

            var sortColumns = new Dictionary<string, System.Linq.Expressions.Expression<System.Func<Product, object>>>
    {
        { "id", p => p.Id },
        { "name", p => p.Name },
        { "price", p => p.Price },
        { "stock", p => p.Stock }
    };

            query = ApplySorting(query, request.SortBy, request.SortDir, sortColumns);

            var totalCount = await query.CountAsync(cancellationToken);

            query = ApplayPagination(query, request.PageIndex, request.PageSize);

            var items = await query.Select(p => new ProductDto
            {
                Name = p.Name,
                Price = p.Price,
                ImageUrl=p.ImageUrl,
                Description = p.Description
            }).ToListAsync(cancellationToken);

            var result = new PaginatedListDto<ProductDto>
            {
                Items = items,
                PageNumber = request.PageIndex,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return RequestRespones<PaginatedListDto<ProductDto>>.Success(
                result,
                200,
                "Products retrieved successfully."
            );
        }
    }
}
