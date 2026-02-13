using Domain_Layer.DTOs.Attribute;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Application_Layer.CQRS.Attributes.Commands.AddAttributeValues
{
    public record AddAttributeValueCommand( int AddAttributeId, IEnumerable<string> Values) :ICommand<RequestRespones<bool>>;

    
    public class AddAttributeValueCommandHandler : IRequestHandler<AddAttributeValueCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<AttributeValue> genaricRepository;
        private readonly IMemoryCache memoryCache;
        private const string AttributesWithValuesCacheKey = "AttributesWithValues_List";


        public AddAttributeValueCommandHandler(IGenaricRepository<AttributeValue> genaricRepository,IMemoryCache memoryCache)
        {
            this.genaricRepository = genaricRepository;
            this.memoryCache = memoryCache;
        }
        public async Task<RequestRespones<bool>> Handle(AddAttributeValueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var attributeValues = request.Values.Select(value => new AttributeValue
                {
                    AttributeId = request.AddAttributeId,
                    Value = value
                }).ToList();

                await genaricRepository.AddRangeAsync(attributeValues);
                await genaricRepository.SaveChanges();

                memoryCache.Remove(memoryCache);
                return RequestRespones<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return RequestRespones<bool>.Fail($"Error while adding Attribute Values: {ex.Message}", 500);
            }

        }
    }
}
