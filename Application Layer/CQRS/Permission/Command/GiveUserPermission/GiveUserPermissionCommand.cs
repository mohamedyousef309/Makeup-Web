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

namespace Application_Layer.CQRS.Permission.Command.GiveUserPermission
{
    public record GiveUserPermissionCommand(int Userid, IEnumerable<int> permissonid):ICommand<RequestRespones<bool>>;

    public class GiveUserPermissionCommandHandler : IRequestHandler<GiveUserPermissionCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<UserPermissions> genaricRepository;

        public GiveUserPermissionCommandHandler(IGenaricRepository<UserPermissions> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(GiveUserPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userPermissionsList = request.permissonid.Select(pId => new UserPermissions
                {
                    Userid = request.Userid,
                    PermissionId = pId
                }).ToList();

                await genaricRepository.AddRangeAsync(userPermissionsList);
                return RequestRespones<bool>.Success(true);
            }
            catch (Exception ex)
            {

                return RequestRespones<bool>.Fail($"An error occurred while assigning permissions: {ex.Message}", 500);
            }
           

        }
    }
}
