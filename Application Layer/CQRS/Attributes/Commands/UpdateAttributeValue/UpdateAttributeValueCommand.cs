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
using Microsoft.EntityFrameworkCore;

namespace Application_Layer.CQRS.Attributes.Commands.UpdateAttributeValue
{
   
    public record UpdateAttributeValueCommand(UpdateAttributeValueDto UpdateData)
        : ICommand<RequestRespones<bool>>;
    public class UpdateAttributeValueHandler : IRequestHandler<UpdateAttributeValueCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<AttributeValue> _valueRepo;

        public UpdateAttributeValueHandler(IGenaricRepository<AttributeValue> valueRepo)
        {
            _valueRepo = valueRepo;
        }

        public async Task<RequestRespones<bool>> Handle(UpdateAttributeValueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.UpdateData;

               
                var existingValue = await _valueRepo.GetByIdQueryable(dto.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingValue == null)
                    return RequestRespones<bool>.Fail($"Value with Id {dto.Id} not found.", 404);

               
                existingValue.Value = dto.Value;
                existingValue.AttributeId = dto.AttributeId;

               
                _valueRepo.SaveInclude(existingValue);
                await _valueRepo.SaveChanges();

                return RequestRespones<bool>.Success(true, 200, "Attribute value updated successfully.");
            }
            catch (Exception ex)
            {
                return RequestRespones<bool>.Fail($"Error while updating value: {ex.Message}", 500);
            }
        }
    }
}
