using AlonsoAdmin.Entities.System;
using AlonsoAdmin.MultiTenant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlonsoAdmin.Repository.System
{
    public interface ISysApiRepository : IRepositoryBase<SysApiEntity>
    {

        /// <summary>
        /// API入库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> GenerateApisAsync(List<SysApiEntity> list);
    }
}
