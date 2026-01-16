using Domain_Layer.DTOs._ِCategoryDtos;
using Domain_Layer.Entites;
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
            try
            {

                if (!_cache.TryGetValue(CacheKey, out IEnumerable<CategoryLookupDto> categories))
                {
                   
                    categories = await _categoryRepo.GetAll()
                        .AsNoTracking() 
                        .Select(c => new CategoryLookupDto
                        {
                            Id = c.Id,
                            Name = c.Name
                        })
                        .ToListAsync(cancellationToken);

                    _cache.Set(CacheKey, categories, TimeSpan.FromHours(1));
                }
                if (!categories!.Any())
                {
                    return RequestRespones<IEnumerable<CategoryLookupDto>>.Fail("there is no categories Was Found", 404);
                }


                return RequestRespones<IEnumerable<CategoryLookupDto>>.Success(categories, 200, "Success");
            }
            catch (Exception ex)
            {
                return RequestRespones<IEnumerable<CategoryLookupDto>>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
