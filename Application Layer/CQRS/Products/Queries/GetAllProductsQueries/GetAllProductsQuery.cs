using Domain_Layer.DTOs;
using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        string? SortDir = "desc",
        string? SearchTerm = null,
        int? CategoryId=null
    ) : IRequest<RequestRespones<PaginatedListDto<GetAllProductsDto>>>;

    
    public class GetAllProductsHandler : BaseQueryHandler,
        IRequestHandler<GetAllProductsQuery, RequestRespones<PaginatedListDto<GetAllProductsDto>>>
    {
        private readonly IGenaricRepository<Product> _productRepo;
        private readonly IMemoryCache memoryCache;

        public GetAllProductsHandler(IGenaricRepository<Product> productRepo,IMemoryCache memoryCache)
        {
            _productRepo = productRepo;
            this.memoryCache = memoryCache;
        }

        public async Task<RequestRespones<PaginatedListDto<GetAllProductsDto>>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {

            bool isCachable = string.IsNullOrEmpty(request.SearchTerm) && request.PageIndex <= 5;

            string cacheKey = $"Products_Default_P{request.CategoryId??0}{request.PageIndex}_{request.PageSize}";

            if (isCachable&& memoryCache.TryGetValue(cacheKey, out PaginatedListDto<GetAllProductsDto>? cachedResult))
            {
                return RequestRespones<PaginatedListDto<GetAllProductsDto>>.Success(
                    cachedResult!, 200, "Products retrieved from cache.");
            }

            var query = _productRepo.GetAll();

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(x => x.CategoryId == request.CategoryId.Value);
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = ApplySearch(query, request.SearchTerm, p => p.Name);
            }

            var sortColumns = new Dictionary<string, System.Linq.Expressions.Expression<System.Func<Product, object>>>
    {
        { "id", p => p.Id },
        { "name", p => p.Name },
    };

            query = ApplySorting(query, request.SortBy, request.SortDir, sortColumns);

            var totalCount = await query.CountAsync(cancellationToken);

            query = ApplayPagination(query, request.PageIndex, request.PageSize);

            var items = await query
                .Select(p => new GetAllProductsDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl ?? "",
                    Description = p.Description
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedListDto<GetAllProductsDto>
            {
                Items = items,
                PageNumber = request.PageIndex,
                PageSize = request.PageSize,
                TotalCount = totalCount  
            };

            if (isCachable)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10)) 
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1))   
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1); 

                memoryCache.Set(cacheKey, result, cacheOptions);


            }

            return RequestRespones<PaginatedListDto<GetAllProductsDto>>.Success(
                result,
                200,
                "Products retrieved successfully."
            );
        }
    }
}
