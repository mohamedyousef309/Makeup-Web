using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Repositryinterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.CreateUserToken
{
    public record CreateUserTokenCommand(int userid, string token) : IRequest<bool>;

    public class CreateUserTokenCommandHanler : IRequestHandler<CreateUserTokenCommand, bool>
    {
        private readonly IGenaricRepository<UserToken> genaricRepository;

        public CreateUserTokenCommandHanler(IGenaricRepository<UserToken> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<bool> Handle(CreateUserTokenCommand request, CancellationToken cancellationToken)
        {
            var userToken = new UserToken
            {
                UserId = request.userid,
                Token = request.token,
                CreatedAt = DateTime.UtcNow,
                IsVerified = false,
                ExpiredDate = DateTime.UtcNow.AddMinutes(5),
                
            };
           await genaricRepository.addAsync(userToken);
            await genaricRepository.SaveChanges();

            return true;


        }
    }





}
