using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure_Layer.DynamicRBASystem
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public const string PolicyPrefix = "Permission:";
        private readonly IMemoryCache memoryCache;
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

        public PermissionPolicyProvider(IConfiguration configuration, IMemoryCache memoryCache, IOptions<AuthorizationOptions> options)
        {
            this.memoryCache = memoryCache;
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return await _fallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return await _fallbackPolicyProvider.GetFallbackPolicyAsync();
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return _fallbackPolicyProvider.GetPolicyAsync(policyName);
            }

            if (memoryCache.TryGetValue(policyName, out AuthorizationPolicy? cached))
            {
                return Task.FromResult(cached);
            }

            var withoutPrefix = policyName.Substring(PolicyPrefix.Length);  //Permission:ALL:Read,Write

            var idx = withoutPrefix.IndexOf(':');

            if (idx <= 0) return Task.FromResult<AuthorizationPolicy?>(null);

            var mode = withoutPrefix.Substring(0, idx).ToUpperInvariant();

            var perms = withoutPrefix.Substring(idx + 1)
                            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var requireAll = mode == "ALL";

            var policy = new AuthorizationPolicyBuilder()
                            .AddRequirements(new PermissionAuthorizationRequirement(perms, requireAll))
                            .Build();

            memoryCache.Set(policyName, policy, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }
    }

}
