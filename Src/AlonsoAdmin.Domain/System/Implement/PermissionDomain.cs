using AlonsoAdmin.Domain.System.Interface;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Domain.System.Implement
{
    public class PermissionDomain : IPermissionDomain
    {
        private readonly IFreeSql _systemDb;
        public PermissionDomain(IMultiTenantDbFactory multiTenantDbFactory)
        {
            _systemDb = multiTenantDbFactory.Db(Constants.SystemDbKey);
        }

        public async Task<bool> PermissionAssignPowerAsync(string permissionId, List<string> roleIds, List<string> groupIds)
        {

            using (var uow = _systemDb.CreateUnitOfWork())
            {
                #region 权限与角色的关系处理
                var dbPermissionRole = uow.GetRepository<SysRPermissionRoleEntity>();

                //查询已经在库资源
                var oldRoleIds = await dbPermissionRole.Where(d => d.PermissionId == permissionId).ToListAsync(m => m.RoleId);

                //删除已经取消赋权的记录
                var cancelRoleIds = oldRoleIds.Where(d => !roleIds.Contains(d));
                if (cancelRoleIds.Count() > 0)
                {
                    await dbPermissionRole.DeleteAsync(m => m.PermissionId == permissionId && cancelRoleIds.Contains(m.RoleId));
                }

                //插入新赋权的记录
                var insertRoleList = new List<SysRPermissionRoleEntity>();
                var insertRoleIds = roleIds.Where(d => !oldRoleIds.Contains(d));
                if (insertRoleIds.Count() > 0)
                {
                    foreach (var roleId in insertRoleIds)
                    {
                        insertRoleList.Add(new SysRPermissionRoleEntity()
                        {
                            PermissionId = permissionId,
                            RoleId = roleId
                        });
                    }
                    await dbPermissionRole.InsertAsync(insertRoleList);
                }

                #endregion

                #region 权限与角色的关系处理
                var dbPermissionGroup = uow.GetRepository<SysRPermissionGroupEntity>();

                //查询已经在库资源
                var oldGroupIds = await dbPermissionGroup.Where(d => d.PermissionId == permissionId).ToListAsync(m => m.GroupId);

                //删除已经取消赋权的记录
                var cancelGroupIds = oldGroupIds.Where(d => !groupIds.Contains(d));
                if (cancelGroupIds.Count() > 0)
                {
                    await dbPermissionGroup.DeleteAsync(m => m.PermissionId == permissionId && cancelGroupIds.Contains(m.GroupId));
                }

                //插入新赋权的记录
                var insertGroupList = new List<SysRPermissionGroupEntity>();
                var insertGroupIds = groupIds.Where(d => !oldGroupIds.Contains(d));
                if (insertGroupIds.Count() > 0)
                {
                    foreach (var roleId in insertGroupIds)
                    {
                        insertGroupList.Add(new SysRPermissionGroupEntity()
                        {
                            PermissionId = permissionId,
                            GroupId = roleId
                        });
                    }
                    await dbPermissionGroup.InsertAsync(insertGroupList);
                }

                #endregion

                uow.Commit();
            }
            return true;

        }
    }
}
