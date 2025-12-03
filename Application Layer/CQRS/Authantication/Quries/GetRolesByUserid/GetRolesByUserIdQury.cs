using Domain_Layer.DTOs.AthanticationDtos;
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

namespace Application_Layer.CQRS.Authantication.Quries.GetRolesByUserid
{
    public record GetRolesByUserIdQury(int userid):IRequest<RequestRespones<IEnumerable<UserRolsDTo>>>;

    public class GetRolesByUserIdQuryHandler:IRequestHandler<GetRolesByUserIdQury,RequestRespones<IEnumerable<UserRolsDTo>>>
    {
        private readonly IGenaricRepository<Role> genaricRepository;

        public GetRolesByUserIdQuryHandler(IGenaricRepository<Role> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<IEnumerable<UserRolsDTo>>> Handle(GetRolesByUserIdQury request, CancellationToken cancellationToken)
        {
            var UserRoles = await genaricRepository.GetByIdQueryable(request.userid)
                .Select(x => new UserRolsDTo
                {
                    RoleId = x.Id
                    ,RoleName = x.Name,
                }).ToListAsync(cancellationToken);


            if (!UserRoles.Any())
            {
                return RequestRespones<IEnumerable<UserRolsDTo>>.Fail("No Roles Found For This User",404);

            }
            return RequestRespones<IEnumerable<UserRolsDTo>>.Success(UserRoles);
        }
    }



}
