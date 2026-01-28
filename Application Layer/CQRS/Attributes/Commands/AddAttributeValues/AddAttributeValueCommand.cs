using Domain_Layer.DTOs.Attribute;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
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

        public AddAttributeValueCommandHandler(IGenaricRepository<AttributeValue> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
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

                return RequestRespones<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return RequestRespones<bool>.Fail($"Error while adding Attribute Values: {ex.Message}", 500);
            }

        }
    }
}
