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

namespace Application_Layer.CQRS.User.Quries.GetUserByRefreshToken
{
    public record GetUserByRefreshTokenQuery(string RefreshToken):IRequest<RequestRespones<Domain_Layer.Entites.Authantication.User>>;

    public class GetUserByRefreshTokenQueryHandler : IRequestHandler<GetUserByRefreshTokenQuery, RequestRespones<Domain_Layer.Entites.Authantication.User>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.RefreshTokens> genaricRepository;

        public GetUserByRefreshTokenQueryHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.RefreshTokens> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<Domain_Layer.Entites.Authantication.User>> Handle(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await genaricRepository.GetAll().Where(rt => rt.Token == request.RefreshToken 
            && rt.IsActive &&!rt.IsUsed)
                .Include(rt => rt.User).ThenInclude(u => u.UserRoles).ThenInclude(ur => ur.role).
                Include(rt => rt.User).ThenInclude(u => u.userPermissions).ThenInclude(up => up.permission)
                .Select(rt => rt.User).FirstOrDefaultAsync(cancellationToken);


            if (user==null)
            {
                return RequestRespones<Domain_Layer.Entites.Authantication.User>.Fail("Invalid refresh token", 404);
            }
            return RequestRespones<Domain_Layer.Entites.Authantication.User>.Success(user);

        }
    }
}
