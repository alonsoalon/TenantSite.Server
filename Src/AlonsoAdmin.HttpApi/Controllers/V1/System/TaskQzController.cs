using AlonsoAdmin.Common.RequestEntity;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using AlonsoAdmin.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Controllers.V1.System
{
    [Description("计划任务")]
    public class TaskQzController : ModuleBaseController
    {
        private readonly IMapper _mapper;
        private readonly ISysTaskQzService _SysTaskQzService;
        private readonly ISchedulerCenter _schedulerCenter;

        public TaskQzController(IMapper mapper, ISysTaskQzService SysTaskQzService, ISchedulerCenter schedulerCenter)
        {
            _mapper = mapper;
            _SysTaskQzService = SysTaskQzService;
            _schedulerCenter = schedulerCenter;
        }


        #region 通用API方法
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> Create(TaskQzAddRequest req)
        {
            return await _SysTaskQzService.CreateAsync(req);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> Update(TaskQzEditRequest req)
        {
            return await _SysTaskQzService.UpdateAsync(req);
        }

        /// <summary>
        /// 物理删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> Delete(string id)
        {

            return await _SysTaskQzService.DeleteAsync(id);
        }

        /// <summary>
        /// 物理删除 - 批量 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> DeleteBatch(string[] ids)
        {
            return await _SysTaskQzService.DeleteBatchAsync(ids);
        }

        /// <summary>
        /// 软删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> SoftDelete(string id)
        {
            return await _SysTaskQzService.SoftDeleteAsync(id);
        }



        /// <summary>
        /// 软删除 - 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> SoftDeleteBatch(string[] ids)
        {
            return await _SysTaskQzService.SoftDeleteBatchAsync(ids);

        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetItem(string id)
        {
            return await _SysTaskQzService.GetItemAsync(id);
        }

        /// <summary>
        /// 取得条件下分页列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetList(RequestEntity<TaskQzFilterRequest> req)
        {
            var result = await _SysTaskQzService.GetListAsync(req);
            return result;
        }

        /// <summary>
        /// 取得条件下所有的数据（不分页）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetAll(TaskQzFilterRequest req)
        {
            return await _SysTaskQzService.GetAllAsync(req);
        }
        #endregion

        #region 特殊API方法

        /// <summary>
        /// 启动一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [Description("启动一个计划任务")]
        [HttpGet]
        public async Task<IResponseEntity> StartJob(string jobId)
        {
            var job = await _SysTaskQzService.GetSysTaskQzEntity(jobId);
            var res = await _schedulerCenter.AddScheduleJobAsync(job);
            if (res.Success)
            {
                job.IsStart = true;
                await _SysTaskQzService.UpdateTaskQz(job);
            }
            return res;
        }

        /// <summary>
        /// 恢复重启一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet]
        [Description("恢复重启一个计划任务")]
        public async Task<IResponseEntity> ResumeJob(string jobId)
        {
            var job = await _SysTaskQzService.GetSysTaskQzEntity(jobId);
            var res = await _schedulerCenter.ResumeJob(job);
            return res;
        }

        /// <summary>
        /// 暂停一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [Description("暂停一个计划任务")]
        [HttpGet]
        public async Task<IResponseEntity> PauseJob(string jobId)
        {
            var job = await _SysTaskQzService.GetSysTaskQzEntity(jobId);
            var res = await _schedulerCenter.PauseScheduleJobAsync(job);
            return res;
        }

        /// <summary>
        /// 停止一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet]
        [Description("停止一个计划任务")]
        public async Task<IResponseEntity> StopJob(string jobId)
        {
            var job = await _SysTaskQzService.GetSysTaskQzEntity(jobId);
            var res = await _schedulerCenter.StopScheduleJobAsync(job);
            if (res.Success)
            {
                job.IsStart = false;
                await _SysTaskQzService.UpdateTaskQz(job);
            }
            return res;
        }

        #endregion



    }
}
