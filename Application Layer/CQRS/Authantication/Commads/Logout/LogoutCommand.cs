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

namespace Application_Layer.CQRS.Authantication.Commads.Logout
{
    public record LogoutCommand(int userid):IRequest<RequestRespones<bool>>;

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<RefreshTokens> genaricRepository;

        public LogoutCommandHandler(IGenaricRepository<RefreshTokens> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userTokens = await genaricRepository.GetByCriteriaQueryable(rt => rt.userid == request.userid).ToListAsync();
            if (userTokens==null)
            {
                return RequestRespones<bool>.Success(true);
            }

            genaricRepository.DeleteRange(userTokens);

            await genaricRepository.SaveChanges();

            return RequestRespones<bool>.Success(true);
        }
    }


}
