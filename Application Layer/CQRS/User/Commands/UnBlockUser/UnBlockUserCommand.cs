using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.User.Commands.UnBlockUser
{
     public record UnBlockUserCommand(int Userid):ICommand<RequestRespones<bool>>;

    public class UnBlockUserCommandHandler : IRequestHandler<UnBlockUserCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository;

        public UnBlockUserCommandHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(UnBlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await genaricRepository.GetByCriteriaQueryable(x=>x.Id==request.Userid).Select(x=> new Domain_Layer.Entites.Authantication.User 
            { 
                Id=x.Id,
                IsBlocked=x.IsBlocked
            }).FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return RequestRespones<bool>.Fail("User not found", 404);
            }

            user.IsBlocked = false;

            genaricRepository.SaveInclude(user,nameof(user.IsBlocked));

            await genaricRepository.SaveChanges();

            return RequestRespones<bool>.Success(true);
        }
    }
}
