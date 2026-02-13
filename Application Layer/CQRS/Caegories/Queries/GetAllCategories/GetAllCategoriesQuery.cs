using Domain_Layer.DTOs;
using Domain_Layer.DTOs._ِCategoryDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application_Layer.CQRS.Caegories.Queries.GetAllCategories
{
    public record GetAllCategoriesQuery()
         : IRequest<RequestRespones<PaginatedListDto<CategoryDto>>>;
    public class GetAllCategoriesHandler
      : BaseQueryHandler,IRequestHandler<GetAllCategoriesQuery, RequestRespones<PaginatedListDto<CategoryDto>>>
    {
        private readonly IGenaricRepository<Category> _categoryRepo;
        private readonly IMemoryCache _memoryCache; 

        public GetAllCategoriesHandler(IGenaricRepository<Category> categoryRepo, IMemoryCache memoryCache)
        {
            _categoryRepo = categoryRepo;
            _memoryCache = memoryCache;
        }

        public async Task<RequestRespones<PaginatedListDto<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "Categories_All_Default";

            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out PaginatedListDto<CategoryDto>? cachedResult))
                {
                    return RequestRespones<PaginatedListDto<CategoryDto>>.Success(cachedResult!, 200, "Categories loaded from cache.");
                }

                var categoriesQuery = _categoryRepo.GetAll().AsQueryable();

                categoriesQuery = categoriesQuery.OrderBy(x => x.Name);

                var count = await categoriesQuery.CountAsync(cancellationToken);

                var paginatedQuery = ApplayPagination(categoriesQuery, 1, 20);

                var items = await paginatedQuery.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                }).ToListAsync(cancellationToken);

                var result = new PaginatedListDto<CategoryDto>
                {
                    Items = items,
                    PageSize = 20,
                    PageNumber = 1,
                    TotalCount = count
                };

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(45))
                    .SetPriority(CacheItemPriority.High);

                _memoryCache.Set(cacheKey, result, cacheOptions);

                return RequestRespones<PaginatedListDto<CategoryDto>>.Success(result, 200, "Categories loaded successfully.");
            }
            catch (Exception ex)
            {
                return RequestRespones<PaginatedListDto<CategoryDto>>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
