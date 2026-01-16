using Application_Layer.CQRS.User.Commands.MakeUserAdmine;
using Application_Layer.CQRS.User.Quries.GetUserEmailbyUserid;
using Domain_Layer.Constants;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.User.Commands.MakeAdmine
{
    public record MakeUserAdmineOrchestrator(int Userid):ICommand<RequestRespones<bool>>;

    public class MakeUserAdmineOrchestratorHandler : IRequestHandler<MakeUserAdmineOrchestrator, RequestRespones<bool>>
    {
        private readonly IMediator mediator;

        public MakeUserAdmineOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<RequestRespones<bool>> Handle(MakeUserAdmineOrchestrator request, CancellationToken cancellationToken)
        {
            var UserResult = await mediator.Send(new GetUserEmailbyUseridQuery(request.Userid), cancellationToken);

            if (!UserResult.IsSuccess||UserResult.Data==null)
            {
                return RequestRespones<bool>.Fail(UserResult.Message??"User Not Found", UserResult.StatusCode);
            }

            var AddRoleResult= await mediator.Send(new MakeUserAdmineCommand(UserResult.Data.Id, RoleConstants.Admin_id), cancellationToken);

            if (!AddRoleResult.IsSuccess)
            {
                return RequestRespones<bool>.Fail(AddRoleResult.Message ?? "Error while adding Role", AddRoleResult.StatusCode);
            }

            return RequestRespones<bool>.Success(true);

        }
    }
}
