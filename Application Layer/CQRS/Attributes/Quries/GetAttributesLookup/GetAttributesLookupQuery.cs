using Domain_Layer.DTOs.Attribute;
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

namespace Application_Layer.CQRS.Attributes.Quries.GetAttributesLookup
{
    public record GetAttributesLookupQuery() : IRequest<RequestRespones<List<AttributeDto>>>;

    public class GetAttributesLookupQueryHandler : IRequestHandler<GetAttributesLookupQuery, RequestRespones<List<AttributeDto>>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Attribute> _repository;
        private readonly IMemoryCache cache;
        private const string AttributesCacheKey = "Attributes_List_Key";
        public GetAttributesLookupQueryHandler(IGenaricRepository<Domain_Layer.Entites.Attribute> repository,IMemoryCache cache)
        {
            _repository = repository;
            this.cache = cache;
        }

        public async Task<RequestRespones<List<AttributeDto>>> Handle(GetAttributesLookupQuery request, CancellationToken cancellationToken)
        {
            if (!cache.TryGetValue(AttributesCacheKey, out List<AttributeDto> cachedAttributes))
            {
                cachedAttributes = await _repository.GetAll()
                    .Select(a => new AttributeDto
                    {
                        id = a.Id,
                        AttributeName = a.Name
                    })
                    .Distinct()
                    .ToListAsync(cancellationToken);

                var cacheOptions = new MemoryCacheEntryOptions()
                                    .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(45))
                                    .SetPriority(CacheItemPriority.Normal);

                cache.Set(AttributesCacheKey, cachedAttributes);
            }

            if (cachedAttributes == null || !cachedAttributes.Any())
                return RequestRespones<List<AttributeDto>>.Fail("There is no attributes yet", 404);

            return RequestRespones<List<AttributeDto>>.Success(cachedAttributes, 200);
        }
    }
}
