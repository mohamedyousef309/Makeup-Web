using Application_Layer.CQRS.User.Quries.GetUserbyEmail;
using Application_Layer.CQRS.User.Quries.GetUserbyEmailForValidation;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.ValidateUserVerificationCode
{
    public record ValidateVerificationCodeOrchestrator(string token,string userEmail):IRequest<RequestRespones<bool>>;

    public class ValidateVerificationCodeOrchestratorHandler : IRequestHandler<ValidateVerificationCodeOrchestrator,RequestRespones< bool>>
    {
        private readonly IMediator mediator;
        private readonly IGenaricRepository<UserToken> genaricRepository;

        public ValidateVerificationCodeOrchestratorHandler(IMediator mediator,IGenaricRepository<UserToken> genaricRepository)
        {
            this.mediator = mediator;
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(ValidateVerificationCodeOrchestrator request, CancellationToken cancellationToken)
        {
            var userToken = await genaricRepository.GetByCriteriaQueryable(t =>
                    t.Token == request.token &&
                    t.User.Email == request.userEmail).OrderByDescending(t => t.CreatedAt).FirstOrDefaultAsync();

            if (userToken == null)
            {
                return RequestRespones<bool>.Fail("Please make sure the email address you entered is correct.",400);

            }
         

            var validation = CheckTokenValidity(userToken, request.token);

            if (!validation.IsSuccess)
                return validation;

            userToken!.IsVerified = true;

            genaricRepository.SaveInclude(userToken, nameof(UserToken.IsVerified));

            return RequestRespones<bool>.Success(true, 200, "Verified successfully.");





        }

        private RequestRespones<bool> CheckTokenValidity(UserToken? userToken, string providedToken)
        {
            if (userToken == null)
            {
                return RequestRespones<bool>.Fail("Invalid verification code.", 400);
            }

            if (userToken.Token != providedToken)
            {
                return RequestRespones<bool>.Fail("The verification code is incorrect.", 400);
            }

            if (userToken.IsVerified)
            {
                return RequestRespones<bool>.Fail("This code has already been used.", 400);
            }

            if (userToken.ExpiredDate < DateTime.UtcNow)
            {
                return RequestRespones<bool>.Fail("The code has expired.", 400);
            }

            return RequestRespones<bool>.Success(true);
        }
    }


}
