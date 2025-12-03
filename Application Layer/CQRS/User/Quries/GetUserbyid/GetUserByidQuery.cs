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

namespace Application_Layer.CQRS.User.Quries.GetUserbyid
{
    public record GetUserByidQuery(int userid):IRequest<RequestRespones<UsetToReturnDTo>>;

    public class GetUserByidQueryHandler:IRequestHandler<GetUserByidQuery,RequestRespones<UsetToReturnDTo>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> _genaricRepository;

        public GetUserByidQueryHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository)
        {
            this._genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<UsetToReturnDTo>> Handle(GetUserByidQuery request, CancellationToken cancellationToken)
        {
            var user = await _genaricRepository.GetByIdQueryable(request.userid).Select(u => new UsetToReturnDTo
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Picture = u.Picture,
                UserAddress = u.UserAddress,

                UserPermissions = u.userPermissions.Select(p => new UserPermissionsDTo
                {
                    PermissionId = p.PermissionId,
                    PermissionName = p.permission.Name
                }).ToList(),

                UserRoles = u.UserRoles.Select(r => new UserRolsDTo
                {
                    RoleId = r.Roleid,
                    RoleName = r.role.Name
                }).ToList()
            }).FirstOrDefaultAsync(cancellationToken);

            if (user==null)
            {
                return RequestRespones<UsetToReturnDTo>.Fail("User not found",404);

            }

            return RequestRespones<UsetToReturnDTo>.Success(user);


        }
    }




}
