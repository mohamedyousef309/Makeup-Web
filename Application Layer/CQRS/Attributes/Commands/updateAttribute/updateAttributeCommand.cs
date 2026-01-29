using Domain_Layer.DTOs.Attribute;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Attributes.Commands.updateAttribute
{
    public record updateAttributeCommand(UpdateAttributeDto UpdateData) : ICommand<RequestRespones<bool>>;
    public class updateAttributeCommandHandler : IRequestHandler<updateAttributeCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Attribute> _attributeRepository;

        public updateAttributeCommandHandler(IGenaricRepository<Domain_Layer.Entites.Attribute> attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }

        public async Task<RequestRespones<bool>> Handle(updateAttributeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.UpdateData;

               
                var existingAttribute = await _attributeRepository.GetByIdQueryable(dto.Id)
                    .Include(x => x.Values)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingAttribute == null)
                    return RequestRespones<bool>.Fail("Attribute not found", 404);

                existingAttribute.Name = dto.AttributeName;

                
                existingAttribute.Values.Clear();
                if (dto.Values != null)
                {
                    foreach (var val in dto.Values.Where(v => !string.IsNullOrEmpty(v)))
                    {
                        existingAttribute.Values.Add(new AttributeValue { Value = val! });
                    }
                }

               
                _attributeRepository.SaveInclude(existingAttribute);

                
                await _attributeRepository.SaveChanges();

                return RequestRespones<bool>.Success(true, 200, "Updated successfully using SaveInclude");
            }
            catch (Exception ex)
            {
                return RequestRespones<bool>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
