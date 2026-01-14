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
using System.Windows.Input;

namespace Application_Layer.CQRS.User.Commands.BlockUser
{
    public record BlockUserCommand(int userid):ICommand<RequestRespones<bool>>;

    public class BlockUserHandler : IRequestHandler<BlockUserCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository;

        public BlockUserHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await genaricRepository.GetByCriteriaQueryable(x=>x.Id==request.userid).FirstOrDefaultAsync(cancellationToken);

            if (user == null) 
            {
                return RequestRespones<bool>.Fail("User not found", 404);
            }

            user.IsBlocked = true;

            genaricRepository.SaveInclude(user,nameof(user.IsBlocked));
            await genaricRepository.SaveChanges();

            return RequestRespones<bool>.Success(true, 200);

        }
    }


}
