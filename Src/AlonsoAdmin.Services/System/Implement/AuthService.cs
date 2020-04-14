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
        private readonly ISysResourceRepository _resourceRepository;

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
            _resourceRepository = resourceRepository;
            _rRoleResourceRepository = rRoleResourceRepository;
            _authUser = authUser;
        }

        

        public async Task<IResponseEntity> LoginAsync(AuthLoginRequest req)
        {

            var password = MD5Encrypt.Encrypt32(req.Password);

            var user = await _userRepository.GetAsync(a => a.UserName == req.UserName && a.Password == password);

            if (!(user?.Id > 0))
            {
                return ResponseEntity.Error("账号或密码错误!");
            }

            var res = _mapper.Map<AuthLoginResponse>(user);

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
            

            var db = _userRepository.Orm;
            var menus = await db.Select<SysUserEntity, SysRPermissionRoleEntity, SysRRoleResourceEntity, SysResourceEntity>()
                  .LeftJoin((a, b, c, d) => a.PermissionId == b.PermissionId)
                  .LeftJoin((a, b, c, d) => b.RoleId == c.RoleId)
                  .LeftJoin((a, b, c, d) => c.ResourceId == d.Id && new[] { ResourceType.Group, ResourceType.Menu }.Contains(d.ResourceType))
                  .Where((a, b, c, d) => a.Id == _authUser.Id)
                  .Distinct()
                  .ToListAsync((a, b, c, d) => d);

            var res = _mapper.Map<AuthLoginResponse>(user);
            res.Menus = menus == null|| menus[0] == null ? null : menus;

            return ResponseEntity.Ok(res);
        }
    }
}
