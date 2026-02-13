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

namespace Application_Layer.CQRS.Attributes.Commands.deleteAttribute
{
    public record deleteAttributeCommand(int Id) : ICommand<RequestRespones<bool>>;
    public class deleteAttributeCommandHandler : IRequestHandler<deleteAttributeCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Attribute> _attributeRepo;
        private readonly IMemoryCache memoryCache;

        private const string AttributesCacheKey1 = "AllAttributesList";
        private const string AttributesCacheKey2 = "Attributes_List_Key";
        public deleteAttributeCommandHandler(IGenaricRepository<Domain_Layer.Entites.Attribute> attributeRepo,IMemoryCache memoryCache)
        {
            _attributeRepo = attributeRepo;
            this.memoryCache = memoryCache;
        }

        public async Task<RequestRespones<bool>> Handle(deleteAttributeCommand request, CancellationToken cancellationToken)
        {
            try
            {
               
                var attribute = await _attributeRepo.GetByIdQueryable(request.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (attribute == null)
                    return RequestRespones<bool>.Fail($"Attribute with Id {request.Id} not found.", 404);

                _attributeRepo.Delete(attribute);
                await _attributeRepo.SaveChanges();

                memoryCache.Remove(AttributesCacheKey1);
                memoryCache.Remove(AttributesCacheKey2);

                return RequestRespones<bool>.Success(true, 200, "Attribute deleted successfully.");
            }
            catch (Exception ex)
            {
                return RequestRespones<bool>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
