using Domain_Layer.DTOs.Attribute;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public GetAttributesWithValuesQueryhandler(IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<IEnumerable<AttributeWithValueDTo>>> Handle(GetAttributesWithValuesQuery request, CancellationToken cancellationToken)
        {
            var attributes = await genaricRepository.GetAll().Select(a => new AttributeWithValueDTo 
            {
                Attributeid = a.Id,
                Name = a.Name,
                Attributes = a.Values.Select(v => new AttributeValueDto 
                {
                    id = v.Id,
                    Value = v.Value
                }).ToList()
            })
                .ToListAsync(cancellationToken);

            if (!attributes.Any())
            {
                RequestRespones<IEnumerable<AttributeWithValueDTo>>.Fail("There is no attributes for now", 404);
            }

            return RequestRespones<IEnumerable<AttributeWithValueDTo>>.Success(attributes);
        }
    }


}
