using Domain_Layer.DTOs._ِCategoryDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Caegories.Queries.GetCategoriesLookupQuery
{
    public record GetCategoryLookupQuery() : IRequest<RequestRespones<IEnumerable<CategoryLookupDto>>>;

    
    public class GetCategoryLookupHandler : IRequestHandler<GetCategoryLookupQuery, RequestRespones<IEnumerable<CategoryLookupDto>>>
    {
        private readonly IGenaricRepository<Category> _categoryRepo;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "CategoriesLookupCache";

        public GetCategoryLookupHandler(IGenaricRepository<Category> categoryRepo, IMemoryCache cache)
        {
            _categoryRepo = categoryRepo;
            _cache = cache;
        }

        public async Task<RequestRespones<IEnumerable<CategoryLookupDto>>> Handle(GetCategoryLookupQuery request, CancellationToken cancellationToken)
        {
            var categories = await _cache.GetOrCreateAsync(CacheKey, async entry =>  // to solve Race condition
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(25);
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(50));
                entry.Priority = CacheItemPriority.Normal;

                return await _categoryRepo.GetAll()
                    .Select(c => new CategoryLookupDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .ToListAsync(cancellationToken);
            });

            if (categories == null || !categories.Any())
            {
                return RequestRespones<IEnumerable<CategoryLookupDto>>.Fail("No categories found", 404);
            }

            return RequestRespones<IEnumerable<CategoryLookupDto>>.Success(categories);
        }
    }
}
