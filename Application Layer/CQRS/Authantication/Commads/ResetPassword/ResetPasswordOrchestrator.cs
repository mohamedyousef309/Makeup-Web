using Application_Layer.CQRS.Authantication.Commads.RemoveUserToken;
using Application_Layer.CQRS.Authantication.Quries.GetTokenbyUserEmail;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application_Layer.CQRS.Authantication.Commads.ResetPassword
{
    public record ResetPasswordOrchestrator(string UserEmail, string NewPassword):ICommand<RequestRespones<bool>>;

    public class ResetPasswordOrchestratorHandler : IRequestHandler<ResetPasswordOrchestrator, RequestRespones<bool>>
    {
        private readonly IMediator mediator;

        public ResetPasswordOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<RequestRespones<bool>> Handle(ResetPasswordOrchestrator request, CancellationToken cancellationToken)
        {
            var UserTokenResult = await mediator.Send(new GetTokenbyUserEmailQuery(request.UserEmail));
            if (!UserTokenResult.IsSuccess|| UserTokenResult.Data==null)
            {
                return RequestRespones<bool>.Fail(UserTokenResult.Message ?? "there is no token for this user", 400);

            }

            bool isValid= ValidateUserToken(UserTokenResult.Data);

            if (!isValid) { return RequestRespones<bool>.Fail("invalid Code", 400); }

            var resetPasswordResult = await mediator.Send(new ResetPasswordCommand(request.UserEmail, request.NewPassword));

            if (!resetPasswordResult.IsSuccess)
            {
                return RequestRespones<bool>.Fail(resetPasswordResult.Message ?? "Error while reseting password",400);
            }

            var RemoveTokenResult= await mediator.Send(new RemoveUserTokenCommand(UserTokenResult.Data));

            if (!RemoveTokenResult)
            {
                throw new Exception("Failed to remove user token. Rolling back password reset.");
            }


            return RequestRespones<bool>.Success(true, 200, "Password reset successfully.");
        }

        private bool ValidateUserToken(UserToken userToken) 
        {
            if (userToken == null)
                return false;

            if (!userToken.IsVerified)
                return false;

            if (userToken.ExpiredDate < DateTime.UtcNow)
                return false;

            if (userToken.UserId <= 0)
                return false;

            return true;

        }
    }
}

