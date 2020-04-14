using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Repository.System;
using AlonsoAdmin.Services.System.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AlonsoAdmin.Entities.System;

namespace AlonsoAdmin.Services.System.Implement
{
    public class SysUserService : ISysUserService
    {
        private readonly IAuthUser _authUser;
        private readonly ICache _cache;
        private readonly IMapper _mapper;
        private readonly ISysUserRepository _sysUserRepository;
        public SysUserService(
            IAuthUser authUser,
            ICache cache,
            IMapper mapper,
            ISysUserRepository sysUserRepository
            )
        {
            _authUser = authUser;
            _cache = cache;
            _mapper = mapper;
            _sysUserRepository = sysUserRepository;

        }
        public async Task<IEnumerable<string>> GetCurrentUserRolesAsync()
        {
            //var userPermissoins = _sysUserRepository.Select.LeftJoin(a => a.Permission.Id == a.PermissionId);

            var userPermissoins = await _sysUserRepository.GetCurrentUserRolesAsync();
            return userPermissoins;
          
        }
    }
}
