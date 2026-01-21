using Application_Layer.CQRS.Permission.Queries.GetPermssionsByIds;
using Application_Layer.CQRS.User.Quries.GetUserbyid;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Permission.Command.GiveUserPermission
{
    public record GiveUserPermissionOrechestrator(int userid,IEnumerable<int>PermssionsIds):ICommand<RequestRespones<bool>>;

    public class GiveUserPermissionOrechestratorHandler:IRequestHandler<GiveUserPermissionOrechestrator,RequestRespones<bool>>
    {
        private readonly IMediator mediator;
        public GiveUserPermissionOrechestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<RequestRespones<bool>> Handle(GiveUserPermissionOrechestrator request, CancellationToken cancellationToken)
        {
            var UserResult= await mediator.Send(new GetUserByidQuery(request.userid),cancellationToken);

            if (!UserResult.IsSuccess||UserResult.Data==null)
            {
                return RequestRespones<bool>.Fail(UserResult.Message, 404);
            }

            var PermissionResult = await mediator.Send(new GetPermssionsByIdsQuery(request.PermssionsIds));

            if (!PermissionResult.IsSuccess||PermissionResult.Data==null)
            {
                return RequestRespones<bool>.Fail(PermissionResult.Message, 404);

            }

            var validPermissionIds = PermissionResult.Data.Select(x => x.PermissionId);

            return await mediator.Send(new GiveUserPermissionCommand(UserResult.Data.Id, validPermissionIds), cancellationToken);
        }
    }


}
