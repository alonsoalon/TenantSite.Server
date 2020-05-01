using AlonsoAdmin.Domain.System.Interface;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Repository;
using AlonsoAdmin.Repository.System;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Domain.System.Implement
{
    public class ResourceDomain : IResourceDomain
    {

        private readonly IFreeSql _systemDb;
        public ResourceDomain(IMultiTenantDbFactory multiTenantDbFactory)
        {
            _systemDb = multiTenantDbFactory.Db(Constants.SystemDbKey);
           
        }

        /// <summary>
        /// 更新资源的API集合
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="ApiIds"></param>
        /// <returns></returns>
        public async Task<bool> UpdateResourceApisByIdAsync(string resourceId, List<string> ApiIds)
        {

            using (var uow = _systemDb.CreateUnitOfWork())
            {
                var db = uow.GetRepository<SysRResourceApiEntity>();

                //查询该资源已经在库ApiId集合
                var dbApiIds = await db.Select.Where(d => d.ResourceId == resourceId).ToListAsync(m => m.ApiId);

                //删除已经取消的Api集合
                var cancelApis = dbApiIds.Where(d => !ApiIds.Contains(d));

                //插入新的Api集合
                var insertList = new List<SysRResourceApiEntity>();
                var insertApiIds = ApiIds.Where(d => !dbApiIds.Contains(d));
                if (insertApiIds.Count() > 0)
                {
                    foreach (var apiId in insertApiIds)
                    {
                        insertList.Add(new SysRResourceApiEntity()
                        {
                            ResourceId = resourceId,
                            ApiId = apiId
                        });
                    }

                    await db.InsertAsync(insertList);
                }

                if (cancelApis.Count() > 0)
                {
                    await db.DeleteAsync(m => m.ResourceId == resourceId && cancelApis.Contains(m.ApiId));
                }

                uow.Commit();
            }
               

            return true;
        }
    }
}
