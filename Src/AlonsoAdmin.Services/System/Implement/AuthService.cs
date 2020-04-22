using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Utils;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Entities.System.Enums;
using AlonsoAdmin.Repository.System;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using AlonsoAdmin.Services.System.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Implement
{
    public class AuthService : IAuthService
    {
        private readonly ICache _cache;
        private readonly IMapper _mapper;
        private readonly ISysUserRepository _userRepository;
        private readonly ISysResourceRepository _sysResourceRepository;

        private readonly ISysRRoleResourceRepository _rRoleResourceRepository;
        private readonly IAuthUser _authUser;
        public AuthService(
            ICache cache,
            IMapper mapper,
            ISysUserRepository userRepository,
            ISysResourceRepository resourceRepository,
            ISysRRoleResourceRepository rRoleResourceRepository,
            IAuthUser authUser
            )
        {
            _cache = cache;
            _mapper = mapper;
            _userRepository = userRepository;
            _sysResourceRepository = resourceRepository;
            _rRoleResourceRepository = rRoleResourceRepository;
            _authUser = authUser;
        }

        

        public async Task<IResponseEntity> LoginAsync(AuthLoginRequest req)
        {

            var password = MD5Encrypt.Encrypt32(req.Password);

            var user = await _userRepository.GetAsync(a => a.UserName == req.UserName && a.Password == password);

            if (user == null || user?.Id == "")
            {
                return ResponseEntity.Error("账号或密码错误!");
            }

            var res = _mapper.Map<AuthLoginResponse>(user);

            List<ResourceForMenuResponse> list = new List<ResourceForMenuResponse>();
            var cacheKey = string.Format(CacheKeyTemplate.UserPermissionList, user.Id);
            if (await _cache.ExistsAsync(cacheKey))
            {
                list = await _cache.GetAsync<List<ResourceForMenuResponse>>(cacheKey);
            }
            else
            {
                var menus = await _sysResourceRepository.Select
                .Where(a => a.IsDisabled == false)
                .Where(a=> new[] { ResourceType.Group, ResourceType.Menu }.Contains(a.ResourceType))
                .OrderBy(true, a => a.OrderIndex)
                .ToListAsync();
                list = _mapper.Map<List<ResourceForMenuResponse>>(menus);
                // 写入缓存
                await _cache.SetAsync(cacheKey, list);
            }

            res.Menus = list;

            return ResponseEntity.Ok(res);

        }


        public async Task<IResponseEntity> GetUserInfoAsync()
        {
            var user = _userRepository.Get(_authUser.Id);
              //.ToOneAsync(m => new {
              //    m.DisplayName,
              //    m.UserName,
              //    m.Avatar
              //});
            

            //var db = _userRepository.Orm;
            //var menus = await db.Select<SysUserEntity, SysRPermissionRoleEntity, SysRRoleResourceEntity, SysResourceEntity>()
            //      .LeftJoin((a, b, c, d) => a.PermissionId == b.PermissionId)
            //      .LeftJoin((a, b, c, d) => b.RoleId == c.RoleId)
            //      .LeftJoin((a, b, c, d) => c.ResourceId == d.Id && new[] { ResourceType.Group, ResourceType.Menu }.Contains(d.ResourceType))
            //      .Where((a, b, c, d) => a.Id == _authUser.Id)
            //      .Distinct()
            //      .ToListAsync((a, b, c, d) => d);

            var res = _mapper.Map<AuthLoginResponse>(user);


            List<ResourceForMenuResponse> list = new List<ResourceForMenuResponse>();
            var cacheKey = string.Format(CacheKeyTemplate.UserPermissionList, user.Id);
            if (await _cache.ExistsAsync(cacheKey))
            {
                list = await _cache.GetAsync<List<ResourceForMenuResponse>>(cacheKey);
            }
            else
            {
                var menus = await _sysResourceRepository.Select
                .Where(a => a.IsDisabled == false)
                .Where(a => new[] { ResourceType.Group, ResourceType.Menu }.Contains(a.ResourceType))
                .OrderBy(true, a => a.OrderIndex)
                .ToListAsync();
                list = _mapper.Map<List<ResourceForMenuResponse>>(menus);
                // 写入缓存
                await _cache.SetAsync(cacheKey, list);
            }

            res.Menus =list;

            return ResponseEntity.Ok(res);
        }
    }
}
