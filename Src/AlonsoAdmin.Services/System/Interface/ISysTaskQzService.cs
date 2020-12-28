using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Request;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace AlonsoAdmin.Services.System.Interface
{
    public interface ISysTaskQzService : IBaseService<TaskQzFilterRequest, TaskQzAddRequest, TaskQzEditRequest>
    {

        #region 特殊接口 在此定义
        /// <summary>
        /// 根据jobid获取单个job
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<SysTaskQzEntity> GetSysTaskQzEntity(string jobId);

        /// <summary>
        /// 更新计划任务
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateTaskQz(SysTaskQzEntity entity);

        #endregion
    }

}
