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

namespace Application_Layer.CQRS.Authantication.Quries.GetPermissionsByUserid
{
    public record GetPermissionsByUseridQuery(int userid):IRequest<RequestRespones<IEnumerable<UserPermissionsDTo>>>;



    public class GetPermissionsByUseridHandler : IRequestHandler<GetPermissionsByUseridQuery, RequestRespones<IEnumerable<UserPermissionsDTo>>>
    {
        private readonly IGenaricRepository<Permissions> genaricRepository;

        public GetPermissionsByUseridHandler(IGenaricRepository<Permissions> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<IEnumerable<UserPermissionsDTo>>> Handle(GetPermissionsByUseridQuery request, CancellationToken cancellationToken)
        {
            var UserPermissions= await genaricRepository.GetByIdQueryable(request.userid).
                Select(x=>new UserPermissionsDTo
                { 
                    PermissionId=x.Id,
                    PermissionName=x.Name
                }
                ).ToListAsync();

            if (!UserPermissions.Any())
            {
                return RequestRespones<IEnumerable<UserPermissionsDTo>>.Fail("No permissions found for this user.", 404);

            }

            return RequestRespones<IEnumerable<UserPermissionsDTo>>.Success(UserPermissions);
        }
    }
}
