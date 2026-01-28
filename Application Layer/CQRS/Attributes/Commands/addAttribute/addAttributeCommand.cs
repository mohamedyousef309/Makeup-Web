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

namespace Application_Layer.CQRS.Attributes.Commands.addAttribute
{
    public record addAttributeCommand(string AttributeName, IEnumerable<string?> Values) :ICommand<RequestRespones<bool>>;

    public class addAttributeCommandHandler : IRequestHandler<addAttributeCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository;

        public addAttributeCommandHandler(IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
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

                return RequestRespones<bool>.Success(true, 200, "Attribute added successfully");
            }
            catch (Exception ex)
            {

                return RequestRespones<bool>.Fail($"Error while Adding  Attribute {ex.Message}", 500);
            }
           

        }
    }
}
