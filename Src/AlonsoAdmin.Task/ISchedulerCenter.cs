using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Entities.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Tasks
{
    public interface ISchedulerCenter
    {
        /// <summary>
        /// 添加一个计划任务（映射程序集指定IJob实现类）
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        Task<IResponseEntity> AddScheduleJobAsync(SysTaskQzEntity tasksQz);

        /// <summary>
        /// 暂停一个指定的计划任务
        /// </summary>
        /// <returns></returns>
        Task<IResponseEntity> PauseScheduleJobAsync(SysTaskQzEntity sysSchedule);

        /// <summary>
        /// 停止一个指定的计划任务
        /// </summary>
        /// <returns></returns>
        Task<IResponseEntity> StopScheduleJobAsync(SysTaskQzEntity sysSchedule);

        /// <summary>
        /// 恢复指定的计划任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<IResponseEntity> ResumeJob(SysTaskQzEntity sysSchedule);

        /// <summary>
        /// 获取计划任务运行状态
        /// </summary>
        /// <param name="key"></param>
        /// <param name="jobGroup"></param>
        /// <returns></returns>
        string GetJobRunTriggerState(string key, string jobGroup);

        /// <summary>
        /// 获取任务下一次执行时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="jobGroup"></param>
        /// <returns></returns>
        DateTime? GetJobNextFireTime(string key, string jobGroup);
    }

}
