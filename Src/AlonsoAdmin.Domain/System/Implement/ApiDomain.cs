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
    public class ApiDomain: IApiDomain
    {

        private readonly IFreeSql _systemDb;
        public ApiDomain(IMultiTenantDbFactory multiTenantDbFactory)
        {
            _systemDb = multiTenantDbFactory.Db(Constants.SystemDbKey);
        }


        public async Task<bool> GenerateApisAsync(List<SysApiEntity> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }

            using (var uow = _systemDb.CreateUnitOfWork())
            {
                var dbApi = uow.GetRepository<SysApiEntity>();

                //数据库中已有的API
                var dbApis = dbApi.Select.ToList();

                //待更新的API
                List<SysApiEntity> updateItems = new List<SysApiEntity>();

                //待插入的API
                List<SysApiEntity> insertItems = new List<SysApiEntity>();

                foreach (var item in list)
                {
                    var dbItem = dbApis.Where(x => x.Url == item.Url).FirstOrDefault();
                    if (dbItem == null)
                    {
                        insertItems.Add(item);
                    }
                    else
                    {
                        dbItem.Category = item.Category;
                        dbItem.Title = item.Title;
                        if (dbItem.Description.Trim() == "")
                        {
                            dbItem.Description = item.Description;
                        }
                        dbItem.HttpMethod = item.HttpMethod;
                        updateItems.Add(dbItem);
                    }

                }

                if (updateItems.Count > 0)
                {
                    await dbApi.UpdateAsync(updateItems);
                }

                if (insertItems.Count > 0)
                {
                    await dbApi.InsertAsync(insertItems);
                }

                uow.Commit();
            }
            return true;
        }
    }
}
