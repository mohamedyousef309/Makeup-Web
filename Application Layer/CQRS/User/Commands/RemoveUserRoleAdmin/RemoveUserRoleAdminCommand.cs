using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.User.Commands.RemoveUserRoleAdmin
{
    public record RemoveUserRoleAdminCommand(int Userid):ICommand<RequestRespones<bool>>;

    public class RemoveUserRoleAdminCommandHandler : IRequestHandler<RemoveUserRoleAdminCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<UserRole> genaricRepository;
        private readonly ILogger<RemoveUserRoleAdminCommand> logger;

        public RemoveUserRoleAdminCommandHandler(IGenaricRepository<UserRole> genaricRepository, ILogger<RemoveUserRoleAdminCommand> logger)
        {
            this.genaricRepository = genaricRepository;
            this.logger = logger;
        }
        public async Task<RequestRespones<bool>> Handle(RemoveUserRoleAdminCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var userRoleEntry = await genaricRepository
                  .GetByCriteriaQueryable(x => x.Userid == request.Userid)
                  .FirstOrDefaultAsync(cancellationToken);

                if (userRoleEntry == null)
                {
                    return RequestRespones<bool>.Fail("NO user was Found", 404);
                }

                userRoleEntry.Roleid = 3;

                genaricRepository.SaveInclude(userRoleEntry, nameof(userRoleEntry.Roleid));

                await genaricRepository.SaveChanges();

                return RequestRespones<bool>.Success(true);
            }

            catch (Exception ex)
            {

                logger.LogError(ex, "Error while  Deleteing The role");
                return RequestRespones<bool>.Fail("Error while  Deleteing The role", 400);
            }

        }


    }
}
