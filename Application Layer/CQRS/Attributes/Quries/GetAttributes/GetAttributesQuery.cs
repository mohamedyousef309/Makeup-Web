using Domain_Layer.DTOs;
using Domain_Layer.DTOs.Attribute;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
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
using System.Windows.Input;

namespace Application_Layer.CQRS.Attributes.Quries.GetAttributes
{
    public record GetAttributesQuery(
        int PageNumber = 1,
        int PageSize = 10
    ) : IRequest<RequestRespones<PaginatedListDto<AttributeDto>>>;

    public class GetAttributesQueryHandler :BaseQueryHandler, IRequestHandler<GetAttributesQuery, RequestRespones<PaginatedListDto<AttributeDto>>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository;
        private readonly IMemoryCache memoryCache;
        private const string AttributesCacheKey = "AllAttributesList"; 

        public GetAttributesQueryHandler(IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository, IMemoryCache memoryCache)
        {
            this.genaricRepository = genaricRepository;
            this.memoryCache = memoryCache;
        }
        public async Task<RequestRespones<PaginatedListDto<AttributeDto>>> Handle(GetAttributesQuery request, CancellationToken cancellationToken)
        {
            if (!memoryCache.TryGetValue(AttributesCacheKey, out List<AttributeDto> cachedAttributes))
            {
                cachedAttributes = await genaricRepository.GetAll()
                    .Select(a => new AttributeDto
                    {
                        id = a.Id,
                        AttributeName = a.Name
                    })
                    .Distinct()
                    .ToListAsync(cancellationToken);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(40)) 
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1)) 
                    .SetPriority(CacheItemPriority.Normal);

                memoryCache.Set(AttributesCacheKey, cachedAttributes, cacheOptions);
            }

            var totalCount = cachedAttributes.Count;

            if (totalCount == 0)
                return RequestRespones<PaginatedListDto<AttributeDto>>.Fail("There is no attributes yet", 404);

            var items = cachedAttributes
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var result = new PaginatedListDto<AttributeDto>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return RequestRespones<PaginatedListDto<AttributeDto>>.Success(result, 200);

        }
    }
}
