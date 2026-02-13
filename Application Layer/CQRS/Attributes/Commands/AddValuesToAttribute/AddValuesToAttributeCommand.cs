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

namespace Application_Layer.CQRS.Attributes.Commands.AddValuesToAttribute
{
    public record AddValuesToAttributeCommand(AddValuesToAttributeDto AddData)
         : ICommand<RequestRespones<bool>>;
    public class AddValuesToAttributeHandler : IRequestHandler<AddValuesToAttributeCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<AttributeValue> _valueRepo;
        private readonly IMemoryCache memoryCache;
        private const string AttributesWithValuesCacheKey = "AttributesWithValues_List";

        public AddValuesToAttributeHandler(IGenaricRepository<AttributeValue> valueRepo,IMemoryCache memoryCache)
        {
            _valueRepo = valueRepo;
            this.memoryCache = memoryCache;
        }

        public async Task<RequestRespones<bool>> Handle(AddValuesToAttributeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.AddData;

               
                var newValues = dto.Values
                    .Where(v => !string.IsNullOrEmpty(v))
                    .Select(v => new AttributeValue
                    {
                        AttributeId = dto.AttributeId,
                        Value = v
                    }).ToList();

                if (!newValues.Any())
                    return RequestRespones<bool>.Fail("No valid values provided to add.", 400);

                // إضافة القيم للجدول
                await _valueRepo.AddRangeAsync(newValues);
                await _valueRepo.SaveChanges();

                memoryCache.Remove(AttributesWithValuesCacheKey);
                return RequestRespones<bool>.Success(true, 200, "New values added to attribute successfully.");
            }
            catch (Exception ex)
            {
                return RequestRespones<bool>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
