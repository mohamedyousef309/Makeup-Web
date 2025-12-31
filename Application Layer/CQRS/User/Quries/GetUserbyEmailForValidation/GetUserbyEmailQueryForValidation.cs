using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.User.Quries.GetUserbyEmailForValidation
{
    public record GetUserbyEmailQueryForValidation(string UserEmail) : IRequest<RequestRespones<Domain_Layer.Entites.Authantication.User>>;

    public class GetUserbyEmailQueryForValidationHandler : IRequestHandler<GetUserbyEmailQueryForValidation, RequestRespones<Domain_Layer.Entites.Authantication.User>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository;

        public GetUserbyEmailQueryForValidationHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<Domain_Layer.Entites.Authantication.User>> Handle(GetUserbyEmailQueryForValidation request, CancellationToken cancellationToken)
        {
            var user = await genaricRepository.GetByCriteriaQueryable(u => u.Email == request.UserEmail).Select(x => new Domain_Layer.Entites.Authantication.User
            {
                Id=x.Id,
                userTokens=x.userTokens,

            }).FirstOrDefaultAsync();

            if (user == null)
            {
                return RequestRespones<Domain_Layer.Entites.Authantication.User>.Fail("User not found", 404);
            }

            return  RequestRespones<Domain_Layer.Entites.Authantication.User>.Success(user, 200);
        }
    }
}
