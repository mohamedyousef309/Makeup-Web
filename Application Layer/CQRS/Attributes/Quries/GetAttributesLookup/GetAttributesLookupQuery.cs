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

namespace Application_Layer.CQRS.Attributes.Quries.GetAttributesLookup
{
    public record GetAttributesLookupQuery() : IRequest<RequestRespones<List<AttributeDto>>>;

    public class GetAttributesLookupQueryHandler : IRequestHandler<GetAttributesLookupQuery, RequestRespones<List<AttributeDto>>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Attribute> _repository;

        public GetAttributesLookupQueryHandler(IGenaricRepository<Domain_Layer.Entites.Attribute> repository)
        {
            _repository = repository;
        }

        public async Task<RequestRespones<List<AttributeDto>>> Handle(GetAttributesLookupQuery request, CancellationToken cancellationToken)
        {
            var attributes = await _repository.GetAll()
                .Select(a => new AttributeDto
                {
                    id = a.Id,
                    AttributeName = a.Name
                })
                .ToListAsync(cancellationToken);

            return RequestRespones<List<AttributeDto>>.Success(attributes, 200);
        }
    }
}
