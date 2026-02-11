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

namespace Application_Layer.CQRS.Attributes.Quries.GetAttributesWithValues
{
    public record GetAttributesWithValuesQuery: IRequest<RequestRespones<IEnumerable<AttributeWithValueDTo>>>;

    public class GetAttributesWithValuesQueryhandler:IRequestHandler<GetAttributesWithValuesQuery, RequestRespones<IEnumerable<AttributeWithValueDTo>>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository;
        private readonly IMemoryCache memoryCache;

        private const string AttributesWithValuesCacheKey = "AttributesWithValues_List";

        public GetAttributesWithValuesQueryhandler(IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository,IMemoryCache memoryCache)
        {
            this.genaricRepository = genaricRepository;
            this.memoryCache = memoryCache;
        }
        public async Task<RequestRespones<IEnumerable<AttributeWithValueDTo>>> Handle(GetAttributesWithValuesQuery request, CancellationToken cancellationToken)
        {
            if (!memoryCache.TryGetValue(AttributesWithValuesCacheKey, out List<AttributeWithValueDTo>? attributes))
            {
                attributes = await genaricRepository.GetAll()
                    .GroupBy(a => new { a.Id, a.Name })
                    .Select(g => new AttributeWithValueDTo
                    {
                        Attributeid = g.Key.Id,
                        Name = g.Key.Name,
                        Attributes = g.SelectMany(a => a.Values)
                                      .Select(v => new AttributeValueDto
                                      {
                                          id = v.Id,
                                          AttributeId = v.AttributeId, 
                                          Value = v.Value
                                      })
                                      .Distinct()
                                      .ToList()
                    })
                    .ToListAsync(cancellationToken);

                if (attributes != null && attributes.Any())
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(20))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
                        .SetPriority(CacheItemPriority.Normal);

                    memoryCache.Set(AttributesWithValuesCacheKey, attributes, cacheOptions);
                }
            }

            if (attributes == null || !attributes.Any())
            {
                return RequestRespones<IEnumerable<AttributeWithValueDTo>>.Fail("There is no attributes for now", 404);
            }

            return RequestRespones<IEnumerable<AttributeWithValueDTo>>.Success(attributes);
        }
    }


}
