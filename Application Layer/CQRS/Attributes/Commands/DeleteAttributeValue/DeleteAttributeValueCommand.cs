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
using Microsoft.Extensions.Caching.Memory;

namespace Application_Layer.CQRS.Attributes.Commands.DeleteAttributeValue
{
    public record DeleteAttributeValueCommand(int Id) : ICommand<RequestRespones<bool>>;
    public class DeleteAttributeValueHandler : IRequestHandler<DeleteAttributeValueCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<AttributeValue> _repository;

        private const string AttributesWithValuesCacheKey = "AttributesWithValues_List";
        private readonly IMemoryCache memoryCache;

        public DeleteAttributeValueHandler(IGenaricRepository<AttributeValue> repository,IMemoryCache memoryCache)
        {
            _repository = repository;
            this.memoryCache = memoryCache;
        }

        public async Task<RequestRespones<bool>> Handle(DeleteAttributeValueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                var attrValue = await _repository.GetByIdQueryable(request.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (attrValue == null)
                    return RequestRespones<bool>.Fail("Attribute value not found", 404);

                _repository.Delete(attrValue);
                await _repository.SaveChanges();

                memoryCache.Remove(AttributesWithValuesCacheKey);
                return RequestRespones<bool>.Success(true, 200, "Value deleted successfully");
            }
            catch (Exception ex)
            {
                return RequestRespones<bool>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
