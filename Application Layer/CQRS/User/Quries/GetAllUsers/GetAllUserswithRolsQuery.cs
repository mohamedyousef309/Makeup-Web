using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.DTOs.UserDTOS;
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

namespace Application_Layer.CQRS.User.Quries.GetAllUsers
{
    public record GetAllUserswithRolsQuery:IRequest<RequestRespones<IEnumerable<UserToReturnWithRolsDTo>>>;

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUserswithRolsQuery, RequestRespones<IEnumerable<UserToReturnWithRolsDTo>>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository;

        public GetAllUsersQueryHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<IEnumerable<UserToReturnWithRolsDTo>>> Handle(GetAllUserswithRolsQuery request, CancellationToken cancellationToken)
        {
            var users = await genaricRepository.GetAll().Select(x => new UserToReturnWithRolsDTo
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Picture = x.Picture,
                UserAddress = x.UserAddress,
                UserPermissions = x.userPermissions.Select(UP => new UserPermissionsDTo
                {
                    PermissionId = UP.Id,
                    PermissionName = UP.permission.Name
                }).ToList(),
                UserRoles = x.UserRoles.Select(UP => new UserRolsDTo
                {
                    RoleId = UP.Id,
                    RoleName = UP.role.Name
                }).ToList()
            }).ToListAsync();
            if (!users.Any())
            {
                return RequestRespones<IEnumerable<UserToReturnWithRolsDTo>>.Fail("There is no Users", 400);
            }
            return RequestRespones<IEnumerable<UserToReturnWithRolsDTo>>.Success(users);


        }
    }
}
