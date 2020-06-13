using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Services.System.Interface;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Implement
{
    public class CacheService: ICacheService
    {
        private readonly ICache _cache;
        private readonly IAuthUser _authUser;
        private readonly ILogger<CacheService> _logger;
        public CacheService(ICache cache, IAuthUser authUser, ILogger<CacheService> logger)
        {
            _cache = cache;
            _authUser = authUser;
            _logger = logger;
        }

        public IResponseEntity GetCacheKeyTemplates()
        {
            var list = new List<object>();
            var cacheKey = typeof(CacheKeyTemplate);
            var fields = cacheKey.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (var field in fields)
            {
                var descriptionAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

                list.Add(new
                {
                    field.Name,
                    Value = field.GetRawConstantValue().ToString(),
                    descriptionAttribute?.Description
                });
            }

            return ResponseEntity.Ok(list);
        }


        public async Task<IResponseEntity> ClearAsync(string cacheKey)
        {
            _logger.LogWarning($"清除缓存[{cacheKey}]，操作人：{_authUser.Id}.{_authUser.UserName}");
            await _cache.RemoveByPatternAsync(cacheKey);
            return ResponseEntity.Ok();
        }
    }
}
