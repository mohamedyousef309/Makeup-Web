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

namespace Application_Layer.CQRS.Attributes.Commands.addAttribute
{
    public record addAttributeCommand(string AttributeName, IEnumerable<string?> Values) :ICommand<RequestRespones<bool>>;

    public class addAttributeCommandHandler : IRequestHandler<addAttributeCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository;
        private readonly IMemoryCache memoryCache;
        private const string AttributesCacheKey1 = "AllAttributesList";
        private const string AttributesCacheKey2 = "Attributes_List_Key";


        public addAttributeCommandHandler(IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository,IMemoryCache memoryCache)
        {
            this.genaricRepository = genaricRepository;
            this.memoryCache = memoryCache;
        }
        public async Task<RequestRespones<bool>> Handle(addAttributeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var Attribute = new Domain_Layer.Entites.Attribute
                {
                    Name = request.AttributeName,
                    Values = (request.Values ?? Enumerable.Empty<string>())
                        .Where(v => !string.IsNullOrEmpty(v))
                        .Select(v => new AttributeValue { Value = v })
                        .ToList()
                };

                await genaricRepository.addAsync(Attribute);

                await genaricRepository.SaveChanges();

                memoryCache.Remove(AttributesCacheKey1);
                memoryCache.Remove(AttributesCacheKey2);

                return RequestRespones<bool>.Success(true, 200, "Attribute added successfully");
            }
            catch (Exception ex)
            {

                return RequestRespones<bool>.Fail($"Error while Adding  Attribute {ex.Message}", 500);
            }
           

        }
    }
}
