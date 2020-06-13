using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.ResponseEntity;
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


        private async Task<AuthLoginResponse> getUserItem(SysUserEntity user)
        {

            var res = _mapper.Map<AuthLoginResponse>(user);

            #region 得到菜单数据
            AuthResourceResponse authResource = new AuthResourceResponse();
            var cacheKey = string.Format(CacheKeyTemplate.PermissionResourceList, user.PermissionId);
            if (await _cache.ExistsAsync(cacheKey))
            {
                authResource = await _cache.GetAsync<AuthResourceResponse>(cacheKey);
            }
            else
            {

                var resourceList = await _permissionDomain.GetPermissionResourcesAsync(user.PermissionId);
                var menuList = resourceList.Where(x => new[] { ResourceType.Group, ResourceType.Menu }.Contains(x.ResourceType));
                var functionPointList = resourceList.Where(x => x.ResourceType == ResourceType.Func);

                authResource.Menus = _mapper.Map<List<ResourceForMenuResponse>>(menuList);
                authResource.FunctionPoints = functionPointList.Select(x => menuList.Where(y => y.Id == x.ParentId).FirstOrDefault()?.Code + "." + x.Code).ToList();

                // 写入缓存
                await _cache.SetAsync(cacheKey, authResource);
            }
            #endregion


            res.Menus = authResource.Menus;
            res.FunctionPoints = authResource.FunctionPoints;

            return res;

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
            
            var res = await getUserItem(user);
            return ResponseEntity.Ok(res);
        }

        /// <summary>
        /// 得到当前用户信息（主要用于前端F5刷新）
        /// </summary>
        /// <returns></returns>
        public async Task<IResponseEntity> GetUserInfoAsync()
        {
            var user = _userRepository.Get(_authUser.Id);
            var res = await getUserItem(user);
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

        /// <summary>
        /// 验证当前用户API访问权限
        /// </summary>
        /// <returns></returns>
        public async Task<bool> VerifyUserAccessApiAsync(string api)
        {

            // 方案1(目前采用)：
            // 未注册的API，需授权才可访问

            // 方案2：
            // 未注册的API，视为不用授权即可访问，需求先验证API在数据库中存在与否

            var cacheKey = string.Format(CacheKeyTemplate.PermissionApiList, _authUser.PermissionId);
            List<SysApiEntity> data = new List<SysApiEntity>();
            if (await _cache.ExistsAsync(cacheKey))
            {
                data = await _cache.GetAsync<List<SysApiEntity>>(cacheKey);
            }
            else
            {


                data = await _permissionDomain.GetPermissionApisAsync(_authUser.PermissionId);
                await _cache.SetAsync(cacheKey, data);
            }

            var filterApis = data.Where(x => x.Url.ToLower() == api.ToLower());


            if (filterApis.Count() > 0)
            {
                return true;
            }

            return false;
        }

    }
}
