using Domain_Layer.CQRS.Authantication;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.Login
{
    public record LoginOrchestrator(string UserEmial, string UserPassword):IRequest<RequestRespones<AuthModleDto>>;

    public class LoginOrchestratorHandler:IRequestHandler<LoginOrchestrator,RequestRespones<AuthModleDto>>
    {
        private readonly IMediator _mediator;
        public LoginOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<RequestRespones<AuthModleDto>> Handle(LoginOrchestrator request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }



}
