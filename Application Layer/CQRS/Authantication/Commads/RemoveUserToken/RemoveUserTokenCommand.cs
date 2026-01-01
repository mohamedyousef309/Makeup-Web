using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application_Layer.CQRS.Authantication.Commads.RemoveUserToken
{
    public record RemoveUserTokenCommand(UserToken Token):ICommand<bool>;

    public class RemoveUserTokenCommandHandler : IRequestHandler<RemoveUserTokenCommand, bool>
    {
        private readonly IGenaricRepository<UserToken> genaricRepository;

        public RemoveUserTokenCommandHandler(IGenaricRepository<UserToken> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<bool> Handle(RemoveUserTokenCommand request, CancellationToken cancellationToken)
        {
           var token= await genaricRepository.GetByIdQueryable(request.Token.Id).FirstOrDefaultAsync();
            if (token == null)
            {
                return false;
            }
            genaricRepository.Delete(token);
            await genaricRepository.SaveChanges();
            return true;
        }
    }


}
