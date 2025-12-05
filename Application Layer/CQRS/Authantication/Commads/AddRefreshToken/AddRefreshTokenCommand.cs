using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.AddRefreshToken
{
    public record AddRefreshTokenCommand(RefreshTokens RefreshTokens):IRequest<RequestRespones<bool>>;

    public class AddRefreshTokenCommandHandler:IRequestHandler<AddRefreshTokenCommand,RequestRespones<bool>>
    {
        private readonly IGenaricRepository<RefreshTokens> genaricRepository;

        public AddRefreshTokenCommandHandler(IGenaricRepository<RefreshTokens> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var RefreshToken = new RefreshTokens
                {
                    Token = request.RefreshTokens.Token,
                    ExpiresOn = request.RefreshTokens.ExpiresOn,
                    CreatedAt = request.RefreshTokens.CreatedAt,
                    userid = request.RefreshTokens.userid,

                    IsUsed = false,
                    RevokedOn = null 
                };

                await genaricRepository.addAsync(RefreshToken);
                await genaricRepository.SaveChanges();
                return RequestRespones<bool>.Success(true);
            }
            catch (Exception ex) 
            {
                
                return RequestRespones<bool>.Fail("error while adding refreshtoken", 400);
            }
        }
    }


}
