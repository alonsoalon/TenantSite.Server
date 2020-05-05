using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Utils;
using AlonsoAdmin.Domain.System.Interface;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Entities.System.Enums;
using AlonsoAdmin.Repository.System;
using AlonsoAdmin.Repository.System.Interface;
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
        private readonly IAuthUser _authUser;
        private readonly IPermissionDomain _permissionDomain;
        public AuthService(
            ICache cache,
            IMapper mapper,
            ISysUserRepository userRepository,
            ISysResourceRepository resourceRepository,
            IAuthUser authUser,
            IPermissionDomain permissionDomain
            )
        {
            _cache = cache;
            _mapper = mapper;
            _userRepository = userRepository;
            _sysResourceRepository = resourceRepository;
            _authUser = authUser;
            _permissionDomain = permissionDomain;
        }

        

        public async Task<IResponseEntity> LoginAsync(AuthLoginRequest req)
        {

            var password = MD5Encrypt.Encrypt32(req.Password);
            SysUserEntity user = new SysUserEntity();
            using (_userRepository.DataFilter.Disable("Group"))
            {
                user = await _userRepository.GetAsync(a => a.UserName == req.UserName && a.Password == password);
            }

            if (user?.Id == "")
            {
                return ResponseEntity.Error("账号或密码错误!");
            }

            var res = _mapper.Map<AuthLoginResponse>(user);

            List<ResourceForMenuResponse> list = new List<ResourceForMenuResponse>();
            var cacheKey = string.Format(CacheKeyTemplate.PermissionMenuList, user.PermissionId);
            if (await _cache.ExistsAsync(cacheKey))
            {
                list = await _cache.GetAsync<List<ResourceForMenuResponse>>(cacheKey);
            }
            else
            {

                var menus = await _permissionDomain.GetPermissionMenusAsync(user.PermissionId);

                list = _mapper.Map<List<ResourceForMenuResponse>>(menus);
                // 写入缓存
                await _cache.SetAsync(cacheKey, list);
            }

            res.Menus = list;

            return ResponseEntity.Ok(res);

        }

        /// <summary>
        /// 得到当前用户信息（主要用于前端F5刷新）
        /// </summary>
        /// <returns></returns>
        public async Task<IResponseEntity> GetUserInfoAsync()
        {
            var user = _userRepository.Get(_authUser.Id);

            var res = _mapper.Map<AuthLoginResponse>(user);

            List<ResourceForMenuResponse> list = new List<ResourceForMenuResponse>();
            var cacheKey = string.Format(CacheKeyTemplate.PermissionMenuList, user.Id);
            if (await _cache.ExistsAsync(cacheKey))
            {
                list = await _cache.GetAsync<List<ResourceForMenuResponse>>(cacheKey);
            }
            else
            {

                var menus = await _permissionDomain.GetPermissionMenusAsync(user.PermissionId);    
                list = _mapper.Map<List<ResourceForMenuResponse>>(menus);
                // 写入缓存
                await _cache.SetAsync(cacheKey, list);
            }

            res.Menus =list;

            return ResponseEntity.Ok(res);
        }

        /// <summary>
        /// 得到当前用户的数据归属组
        /// </summary>
        /// <returns></returns>
        public async Task<IResponseEntity> GetUserGroupsAsync() {

            var cacheKey = string.Format(CacheKeyTemplate.PermissionGroupList, _authUser.PermissionId);
            List<SysGroupEntity> data = new List<SysGroupEntity>();
            if (await _cache.ExistsAsync(cacheKey))
            {
                data = await _cache.GetAsync<List<SysGroupEntity>>(cacheKey);
            }
            else
            {
                data = await _permissionDomain.GetPermissionGroupsAsync(_authUser.PermissionId);
                await _cache.SetAsync(cacheKey, data);
            }
            var result = _mapper.Map<List<GroupForListResponse>>(data);
            return ResponseEntity.Ok(result);
        }

    }
}
