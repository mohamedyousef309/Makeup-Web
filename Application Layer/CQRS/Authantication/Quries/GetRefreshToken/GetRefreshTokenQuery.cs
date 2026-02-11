using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Repositryinterfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Quries.GetRefreshToken
{
    public record GetRefreshTokenQuery(string token):IRequest<Domain_Layer.Entites.Authantication.RefreshTokens?>;

    public class GetRefreshTokenQueryHandler : IRequestHandler<GetRefreshTokenQuery, Domain_Layer.Entites.Authantication.RefreshTokens?>
    {
        private readonly IGenaricRepository<RefreshTokens> genaricRepository;
        private readonly IMemoryCache _memoryCache;


        public GetRefreshTokenQueryHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.RefreshTokens> genaricRepository,IMemoryCache memoryCache)
        {
            this.genaricRepository = genaricRepository;
            this._memoryCache = memoryCache;
        }
        public async Task<Domain_Layer.Entites.Authantication.RefreshTokens?> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"RT_{request.token}";
            if (_memoryCache.TryGetValue(cacheKey, out RefreshTokens CachedToken))
            {
                if (CachedToken.IsActive) 
                {
                   return CachedToken;

                }
                else 
                {
                    _memoryCache.Remove(cacheKey);

                }
            }



        var token = await genaricRepository.GetByCriteriaQueryable(x =>
            x.Token == request.token &&
            x.RevokedOn == null &&
            x.ExpiresOn > DateTime.UtcNow &&
            !x.IsUsed)
        .Select(x => new RefreshTokens
        {
            Id = x.Id,
            Token = x.Token,
            ExpiresOn = x.ExpiresOn,
            CreatedAt = x.CreatedAt,
            IsUsed = x.IsUsed,
            RevokedOn = x.RevokedOn,
            userid = x.userid,
            User = new Domain_Layer.Entites.Authantication.User
            {
                Id = x.User.Id,
                Username = x.User.Username,
                Email = x.User.Email,
                UserRoles = x.User.UserRoles.Select(ur => new UserRole
                {
                    role = new Role { Name = ur.role.Name }
                }).ToList(),
                userPermissions = x.User.userPermissions.Select(up => new UserPermissions
                {
                    permission = new Permissions { Name = up.permission.Name }
                }).ToList()
            }
        }).FirstOrDefaultAsync(cancellationToken);

            if (token != null && token.IsActive)
            {
                var timeUntilExpiry = token.ExpiresOn - DateTime.UtcNow;

                var cacheDuration = timeUntilExpiry < TimeSpan.FromHours(1)
                                    ? timeUntilExpiry
                                    : TimeSpan.FromHours(1);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(cacheDuration) 
                    .SetSlidingExpiration(TimeSpan.FromMinutes(35)) 
                    .SetPriority(CacheItemPriority.High);

                _memoryCache.Set(cacheKey, token, cacheOptions);
            }

            return token;

        }
    }


}
