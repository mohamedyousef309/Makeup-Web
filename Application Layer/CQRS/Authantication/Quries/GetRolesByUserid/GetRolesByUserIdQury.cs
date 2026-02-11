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

namespace Application_Layer.CQRS.Authantication.Quries.GetRolesByUserid
{
    public record GetRolesByUserIdQury(int userid):IRequest<RequestRespones<IEnumerable<UserRolsDTo>>>;

    public class GetRolesByUserIdQuryHandler:IRequestHandler<GetRolesByUserIdQury,RequestRespones<IEnumerable<UserRolsDTo>>>
    {
        private readonly IGenaricRepository<Role> genaricRepository;
        private readonly IMemoryCache memoryCache;
        private readonly string cacheKey = "RoleKey";


        public GetRolesByUserIdQuryHandler(IGenaricRepository<Role> genaricRepository,IMemoryCache memoryCache)
        {
            this.genaricRepository = genaricRepository;
            this.memoryCache = memoryCache;
        }
        public async Task<RequestRespones<IEnumerable<UserRolsDTo>>> Handle(GetRolesByUserIdQury request, CancellationToken cancellationToken)
        {
            var Cash_Key_User = $"{cacheKey}_{request.userid}";
            if (memoryCache.TryGetValue(Cash_Key_User,out IEnumerable<UserRolsDTo>? CashedUserRole))
            {
                return RequestRespones<IEnumerable<UserRolsDTo>>.Success(CashedUserRole!);
            }
            var UserRoles = await genaricRepository.GetByCriteriaQueryable(x=>x.UserRoles.Any(x=>x.Userid==request.userid))
                .Select(x => new UserRolsDTo
                {
                    RoleId = x.Id
                    ,RoleName = x.Name,
                }).ToListAsync(cancellationToken);


            if (!UserRoles.Any())
            {
                return RequestRespones<IEnumerable<UserRolsDTo>>.Fail("No Roles Found For This User",404);

            }

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(35))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1)).
                SetPriority(CacheItemPriority.High);

            memoryCache.Set(Cash_Key_User, UserRoles, cacheOptions);

            return RequestRespones<IEnumerable<UserRolsDTo>>.Success(UserRoles);
        }
    }



}
