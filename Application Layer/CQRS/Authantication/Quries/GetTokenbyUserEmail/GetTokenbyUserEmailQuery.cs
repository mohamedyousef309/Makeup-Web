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

namespace Application_Layer.CQRS.Authantication.Quries.GetTokenbyUserEmail
{
    public record GetTokenbyUserEmailQuery(string UserEmail):IRequest<RequestRespones<UserToken>>;

    public class GetTokenbyUserEmailQueryHandler: IRequestHandler<GetTokenbyUserEmailQuery, RequestRespones<UserToken>>
    {
        private readonly IGenaricRepository<UserToken> genaricRepository;

        public GetTokenbyUserEmailQueryHandler(IGenaricRepository<UserToken> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<UserToken>> Handle(GetTokenbyUserEmailQuery request, CancellationToken cancellationToken)
        {
            var UserToken = await genaricRepository.GetByCriteriaQueryable(ut => ut.User.Email == request.UserEmail)
                .OrderByDescending(ut => ut.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (UserToken==null)
            {
                return RequestRespones<UserToken>.Fail("Token not found",404);

            }

            return RequestRespones<UserToken>.Success(UserToken,200);
        }
    }





}
