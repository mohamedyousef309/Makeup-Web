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

namespace Application_Layer.CQRS.Permission.Queries.GetPermssionsByIds
{
    public record GetPermssionsByIdsQuery(IEnumerable<int> Ids): IRequest<RequestRespones<IEnumerable<UserPermissionsDTo>>>;

    public class GetPermssionsByIdsQueryHandler : IRequestHandler<GetPermssionsByIdsQuery, RequestRespones<IEnumerable<UserPermissionsDTo>>>
    {
        private readonly IGenaricRepository<Permissions> genaricRepository;

        public GetPermssionsByIdsQueryHandler(IGenaricRepository<Permissions> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<IEnumerable<UserPermissionsDTo>>> Handle(GetPermssionsByIdsQuery request, CancellationToken cancellationToken)
        {
            var Pernissions= await genaricRepository.GetByCriteriaQueryable(x=>request.Ids.Contains(x.Id)).Select(x=>new UserPermissionsDTo
            {
                PermissionId=x.Id,
            }).ToListAsync();

            if (!Pernissions.Any())
            {
                return RequestRespones<IEnumerable<UserPermissionsDTo>>.Fail("There is NO Permissins with this Name", 404);
            }

            return RequestRespones<IEnumerable<UserPermissionsDTo>>.Success(Pernissions);
        }
    }
}
