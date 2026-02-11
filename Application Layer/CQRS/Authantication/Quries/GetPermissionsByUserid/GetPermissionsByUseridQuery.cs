using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache memoryCache;
        private readonly string cacheKey = "PermissionsKey";
        public GetPermissionsByUseridHandler(IGenaricRepository<Permissions> genaricRepository,IMemoryCache memoryCache)
        {
            this.genaricRepository = genaricRepository;
            this.memoryCache = memoryCache;

        }
        public async Task<RequestRespones<IEnumerable<UserPermissionsDTo>>> Handle(GetPermissionsByUseridQuery request, CancellationToken cancellationToken)
        {
            var Cash_Key_User= $"{cacheKey}_{request.userid}";
            if (memoryCache.TryGetValue(Cash_Key_User,out IEnumerable<UserPermissionsDTo>? userPermissionsDTos))
            {
                return RequestRespones<IEnumerable<UserPermissionsDTo>>.Success(userPermissionsDTos!);

            }
            var UserPermissions= await genaricRepository.GetByCriteriaQueryable(x=>x.userPermissions.Any(up => up.Userid == request.userid)).
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

            var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(45))
                    .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(1))
                   .SetPriority(CacheItemPriority.High);

            memoryCache.Set(Cash_Key_User, UserPermissions, cacheOptions);

            return RequestRespones<IEnumerable<UserPermissionsDTo>>.Success(UserPermissions);
        }
    }
}
