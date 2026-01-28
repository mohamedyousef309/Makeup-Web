using Application_Layer.CQRS.Attributes.Commands.addAttribute;
using Application_Layer.CQRS.Attributes.Commands.AddAttributeValues;
using Domain_Layer.DTOs.Attribute;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Attributes.Commands.AddAttributesWithValues
{
    public record AddAttributesWithValuesOrchestrator(string AttributeName, IEnumerable<string>Values) :ICommand<RequestRespones<bool>>;

    public class AddAttributesWithValuesOrchestratorHandler : IRequestHandler<AddAttributesWithValuesOrchestrator, RequestRespones<bool>>
    {
        private readonly IMediator mediator;
        public AddAttributesWithValuesOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<RequestRespones<bool>> Handle(AddAttributesWithValuesOrchestrator request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }


}
