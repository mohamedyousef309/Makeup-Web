using Domain_Layer.Entites.Authantication;
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

namespace Application_Layer.CQRS.Permission.Command.RemoveUserPerrmission
{
    public record RemoveUserPermissionCommand(int Userid , int Permissionid):ICommand<RequestRespones<bool>>;

    public class RemoveUserPermissionCommandHandler : IRequestHandler<RemoveUserPermissionCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<UserPermissions> genaricRepository;

        public RemoveUserPermissionCommandHandler(IGenaricRepository<UserPermissions> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(RemoveUserPermissionCommand request, CancellationToken cancellationToken)
        {
            var UserPermission = await genaricRepository.GetByCriteriaQueryable(x=>x.Userid==request.Userid&& x.PermissionId==request.Permissionid)
                .FirstOrDefaultAsync();

            if (UserPermission==null)
            {
                return RequestRespones<bool>.Fail("This User Do not Have THis Permission", 404);
            }

             genaricRepository.Delete(UserPermission);

            await genaricRepository.SaveChanges();

            return RequestRespones<bool>.Success(true, 200, "Permission Removed Succesfully");

        }
    }
}
