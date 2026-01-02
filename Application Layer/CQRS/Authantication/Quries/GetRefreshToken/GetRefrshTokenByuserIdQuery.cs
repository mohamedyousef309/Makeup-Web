using Domain_Layer.DTOs.AthanticationDtos;
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

namespace Application_Layer.CQRS.Authantication.Quries.GetRefreshToken
{
    public record GetRefrshTokenByuserIdQuery(int Userid):IRequest<RequestRespones<RefreshTokens>>;

    public class GetRefrshTokenByuserIdQueryHandler : IRequestHandler<GetRefrshTokenByuserIdQuery, RequestRespones<RefreshTokens>>
    {
        private readonly IGenaricRepository<RefreshTokens> genaricRepository;

        public GetRefrshTokenByuserIdQueryHandler(IGenaricRepository<RefreshTokens> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<RefreshTokens>> Handle(GetRefrshTokenByuserIdQuery request, CancellationToken cancellationToken)
        {
            var RefreshToken = await genaricRepository.GetByCriteriaQueryable(x=> x.userid == request.Userid).Where(r =>
            r.RevokedOn == null &&
            r.ExpiresOn > DateTime.UtcNow &&!r.IsUsed)

                .OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();

            if (RefreshToken == null) 
            {
                return RequestRespones<RefreshTokens>.Fail("No RefreshToken Found For This User", 404);
            }

            return RequestRespones<RefreshTokens>.Success(RefreshToken);


        }
    }



}
