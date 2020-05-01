using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Domain.System.Interface
{
    public interface IResourceDomain
    {
        /// <summary>
        /// 更新指定资源的API集合
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="ApiIds"></param>
        /// <returns></returns>
        Task<bool> UpdateResourceApisByIdAsync(string resourceId, List<string> ApiIds);

    }
}
