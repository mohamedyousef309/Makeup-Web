using Application_Layer.CQRS.Authantication.Commads.CreateUserToken;
using Application_Layer.CQRS.User.Quries.GetUserbyEmail;
using Domain_Layer.DTOs;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Interfaces.ServiceInterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application_Layer.CQRS.Authantication.Commads.ForgetPasswrd
{
    public record ForgetpasswordOrchestrator(string UserGmail):ICommand<RequestRespones<bool>>;

    public class ForgetpasswordOrchestratorHandler : IRequestHandler<ForgetpasswordOrchestrator, RequestRespones<bool>>
    {
        private readonly IEMailService EMailService;
        private readonly IMediator mediator;

        public ForgetpasswordOrchestratorHandler(IEMailService EMailService,IMediator mediator)
        {
            this.EMailService = EMailService;
            this.mediator = mediator;
        }
        public async Task<RequestRespones<bool>> Handle(ForgetpasswordOrchestrator request, CancellationToken cancellationToken)
        {
            var GetUserByEmailResult = await mediator.Send(new GetUserbyEmailQuery(request.UserGmail));
            if (!GetUserByEmailResult.IsSuccess|| GetUserByEmailResult.Data==null)
            {
                return RequestRespones<bool>.Fail(GetUserByEmailResult.Message ?? "there is no user with this gmail",404);
            }
            var VerificationCode = EMailService.GenerateVerificationCode();
            var CreateUserTokenResult = await mediator.Send(new CreateUserTokenCommand(GetUserByEmailResult.Data.Id, VerificationCode));

            if (!CreateUserTokenResult)
            {
                return RequestRespones<bool>.Fail("error while generating ResetPassword Token",400);

            }

            var EmailDTo = new EmailDTo
            {
                Subject = "MakeUp-Wep ResetPasword",
                Body = $"Your GenerateVerificationCode is{VerificationCode}",
                To=GetUserByEmailResult.Data.Email

            };
            await EMailService.SendEmail(EmailDTo);

            return RequestRespones<bool>.Success(true,200,"Verification code sent to your email");
        }
    }





}
