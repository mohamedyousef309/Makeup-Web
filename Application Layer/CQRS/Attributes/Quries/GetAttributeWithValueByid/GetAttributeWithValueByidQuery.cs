using Domain_Layer.DTOs.Attribute;
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

namespace Application_Layer.CQRS.Attributes.Quries.GetAttributeWithValueByid
{
    public record GetAttributeWithValueByidQuery(int Attributeid) : IRequest<RequestRespones<AttributeWithValueDTo>>;

    public class GetAttributeWithValueByidQueryHandler:IRequestHandler<GetAttributeWithValueByidQuery,RequestRespones<AttributeWithValueDTo>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository;

        public GetAttributeWithValueByidQueryHandler(IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository,IMemoryCache memoryCache)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<AttributeWithValueDTo>> Handle(GetAttributeWithValueByidQuery request, CancellationToken cancellationToken)
        {

            
                var attribute = await genaricRepository.GetByCriteriaQueryable(x => x.Id == request.Attributeid)
                    .Select(x => new AttributeWithValueDTo
                    {
                        Attributeid = x.Id,
                        Name = x.Name,
                        Attributes = x.Values.Select(av => new AttributeValueDto
                        {
                            id = av.Id,
                            AttributeId = av.AttributeId, 
                            Value = av.Value
                        }).ToList()
                    }).FirstOrDefaultAsync(cancellationToken);

               
            

            if (attribute == null)
            {
                return RequestRespones<AttributeWithValueDTo>
                    .Fail("There is no attribute found with this id", 404);
            }

            return RequestRespones<AttributeWithValueDTo>
                .Success(attribute, 200, "Attribute loaded successfully");

        }
    }



}
