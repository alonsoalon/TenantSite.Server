using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Repository.System.Interface;
using FreeSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlonsoAdmin.Repository.System.Implement
{
    public class SysUserRepository : RepositoryBase<SysUserEntity>, ISysUserRepository
    {
      
        private readonly IAuthUser _user;
        private readonly ILogger<SysUserRepository> _logger;

        public SysUserRepository(IMultiTenantDbFactory dbFactory, IAuthUser user, ILogger<SysUserRepository> logger) : base(dbFactory.Db(Constants.SystemDbKey), user)
        {
            _user = user;
            _logger = logger;
        }
        public async Task<List<string>> GetCurrentUserRolesAsync() {

            var db = base.Orm;
            var list = db.Select<SysUserEntity, SysRPermissionRoleEntity, SysRoleEntity>()
                  .LeftJoin((a, b, c) => a.PermissionId == b.PermissionId)
                  .LeftJoin((a, b, c) => b.RoleId == c.Id)
                  .Where((a, b, c) => a.Id == _user.Id)
                  .ToListAsync((a, b, c) => c.Code);


            return await list;
        }

        public async Task<List<string>> GetCurrentUserApisAsync()
        {
            var db = base.Orm;
            var list = db.Select<SysUserEntity, SysRPermissionRoleEntity, SysRRoleResourceEntity, SysRResourceApiEntity, SysApiEntity>()
                  .LeftJoin((a, b, c, d, e) => a.PermissionId == b.PermissionId)
                  .LeftJoin((a, b, c, d, e) => b.RoleId == c.RoleId)
                  .LeftJoin((a, b, c, d, e) => c.ResourceId == d.ResourceId)
                  .LeftJoin((a, b, c, d, e) => d.ResourceId == e.Id)             
                  .Where((a, b, c, d, e) => a.Id == _user.Id)
                  .Distinct()
                  .ToListAsync((a, b, c, d, e) => e.Url);
            return await list;
        }

        public async Task<List<SysResourceEntity>> GetCurrentUserMenus()
        {
            var db = base.Orm;
            var list = db.Select<SysUserEntity, SysRPermissionRoleEntity, SysRRoleResourceEntity, SysResourceEntity>()
                  .LeftJoin((a, b, c, d) => a.PermissionId == b.PermissionId)
                  .LeftJoin((a, b, c, d) => b.RoleId == c.RoleId)
                  .LeftJoin((a, b, c, d) => c.ResourceId == d.Id)
                  .Where((a, b, c, d) => a.Id == _user.Id)
                  .Distinct()
                  .ToListAsync((a, b, c, d) => d);
            return await list;
        }

    }
}
