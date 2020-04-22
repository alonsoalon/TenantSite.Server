using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using FreeSql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.Repository.System
{
    public class SysRRoleResourceRepository : RepositoryBase<SysRRoleResourceEntity>, ISysRRoleResourceRepository
    {
        public SysRRoleResourceRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.Dbkey), user)
        {

        }
    

        public async Task<bool> RoleAssignResourcesAsync(string roleId, List<string> resourceIds)
        {

            using (var uow = base.Orm.CreateUnitOfWork())
            {
                var db = uow.GetRepository<SysRRoleResourceEntity>();

                //查询该角色已经在库资源
                var oldResourceIds = await Select.Where(d => d.RoleId == roleId).ToListAsync(m => m.ResourceId);

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
