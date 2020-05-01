using AlonsoAdmin.Entities.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Domain.System.Interface
{
    public interface IApiDomain
    {
        /// <summary>
        /// API入库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> GenerateApisAsync(List<SysApiEntity> list);
    }
}
