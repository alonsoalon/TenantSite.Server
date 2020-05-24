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
    public class RoleDomain : IRoleDomain
    {

        private readonly IFreeSql _systemDb;
        public RoleDomain(IMultiTenantDbFactory multiTenantDbFactory)
        {
            _systemDb = multiTenantDbFactory.Db(Constants.SystemDbKey);

        }


        public async Task<bool> RoleAssignResourcesAsync(string roleId, List<string> resourceIds)
        {
            using (var uow = _systemDb.CreateUnitOfWork())
            {
                var db = uow.GetRepository<SysRRoleResourceEntity>();

                //查询该角色已经在库资源
                var oldResourceIds = await db.Select.Where(d => d.RoleId == roleId).ToListAsync(m => m.ResourceId);

                //删除已经取消赋权的资源
                var cancelResourceIds = oldResourceIds.Where(d => !resourceIds.Contains(d));
                if (cancelResourceIds.Count() > 0)
                {
                    await db.DeleteAsync(m => m.RoleId == roleId && cancelResourceIds.Contains(m.ResourceId));
                }

                //插入新赋权的资源
                var insertList = new List<SysRRoleResourceEntity>();
                var insertResourceIds = resourceIds.Where(d => !oldResourceIds.Contains(d));
                if (insertResourceIds.Count() > 0)
                {
                    foreach (var resourceId in insertResourceIds)
                    {
                        insertList.Add(new SysRRoleResourceEntity()
                        {
                            RoleId = roleId,
                            ResourceId = resourceId
                        });
                    }
                    await db.InsertAsync(insertList);
                }

                uow.Commit();
            }

            return true;
        }
    }
}
