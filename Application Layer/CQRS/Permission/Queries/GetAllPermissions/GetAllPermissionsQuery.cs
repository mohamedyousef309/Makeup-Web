using Domain_Layer.DTOs.AthanticationDtos;
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

namespace Application_Layer.CQRS.Permission.Queries.GetAllPermissions
{
    public record GetAllPermissionsQuery(): IRequest<RequestRespones<IEnumerable<UserPermissionsDTo>>>;

    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, RequestRespones<IEnumerable<UserPermissionsDTo>>>
    {
        private readonly IGenaricRepository<Permissions> genaricRepository;

        public GetAllPermissionsQueryHandler(IGenaricRepository<Permissions> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<IEnumerable<UserPermissionsDTo>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            var Permissions= await genaricRepository.GetAll().Select(x=>new UserPermissionsDTo 
            {
                PermissionId=x.Id,
                PermissionName=x.Name,
            } ).ToListAsync();

            if (!Permissions.Any())
            {
                return RequestRespones<IEnumerable<UserPermissionsDTo>>.Fail("There is No Permissions For now", 404);
            }

            return RequestRespones<IEnumerable<UserPermissionsDTo>>.Success(Permissions);
        }
    }
}
