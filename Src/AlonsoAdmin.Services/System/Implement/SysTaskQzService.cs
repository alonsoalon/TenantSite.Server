using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Common.RequestEntity;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Domain.System.Interface;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.MultiTenant;
using AlonsoAdmin.Repository;
using AlonsoAdmin.Repository.System;
using AlonsoAdmin.Repository.System.Interface;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using AlonsoAdmin.Services.System.Response;
using AlonsoAdmin.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace AlonsoAdmin.Services.System.Implement
{
    public class SysTaskQzService : ISysTaskQzService
    {
        private readonly IMapper _mapper;
        private readonly ICache _cache;
        private readonly ISysTaskQzRepository _SysTaskQzRepository;
        private readonly IAuthUser _authUser;
        private readonly ISchedulerCenter _schedulerCenter;
        public SysTaskQzService(
            IMapper mapper,
            ICache cache,
            ISysTaskQzRepository SysTaskQzRepository,
            IAuthUser authUser,
            ISchedulerCenter schedulerCenter
            )
        {
            _mapper = mapper;
            _cache = cache;
            _SysTaskQzRepository = SysTaskQzRepository;
            _authUser = authUser;
            _schedulerCenter = schedulerCenter;
        }

        #region 通用接口服务实现 对应通用接口
        public async Task<IResponseEntity> CreateAsync(TaskQzAddRequest req)
        {
            var item = _mapper.Map<SysTaskQzEntity>(req);
            if (item.IsDefaultDatabase)
            {
                DbInfo dbInfo = _authUser.Tenant.DbOptions.Where(x => x.Key == Constants.SystemDbKey).FirstOrDefault();
                DbConnectionString connectionString = dbInfo.ConnectionStrings.Where(x => x.UseType == DbUseType.Master).FirstOrDefault();

                item.ConnectionParam = JsonConvert.SerializeObject(new
                {
                    ConnectionString = connectionString.ConnectionString,
                    DbType = Convert.ToInt32(dbInfo.DbType)
                });
            }
            var result = await _SysTaskQzRepository.InsertAsync(item);
            return ResponseEntity.Result(result != null && result?.Id != "");
        }

        public async Task<IResponseEntity> UpdateAsync(TaskQzEditRequest req)
        {

            if (req == null || req?.Id == "")
            {
                return ResponseEntity.Error("更新的实体主键丢失");
            }
            var entity = _mapper.Map<SysTaskQzEntity>(req);

            if (entity.IsDefaultDatabase)
            {
                DbInfo dbInfo = _authUser.Tenant.DbOptions.Where(x => x.Key == Constants.SystemDbKey).FirstOrDefault();
                DbConnectionString connectionString = dbInfo.ConnectionStrings.Where(x => x.UseType == DbUseType.Master).FirstOrDefault();

                entity.ConnectionParam = JsonConvert.SerializeObject(new
                {
                    ConnectionString = connectionString.ConnectionString,
                    DbType = Convert.ToInt32(dbInfo.DbType)
                });
            }

            await _SysTaskQzRepository.UpdateAsync(entity);
            if (entity.IsStart)
            {
                var res = await _schedulerCenter.AddScheduleJobAsync(entity);
            }
            return ResponseEntity.Ok("更新成功");
        }

        public async Task<IResponseEntity> DeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _SysTaskQzRepository.DeleteAsync(id);
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> DeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _SysTaskQzRepository.Where(m => ids.Contains(m.Id)).ToDelete().ExecuteAffrowsAsync();
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> SoftDeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _SysTaskQzRepository.SoftDeleteAsync(id);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> SoftDeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _SysTaskQzRepository.SoftDeleteAsync(ids);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> GetItemAsync(string id)
        {

            var result = await _SysTaskQzRepository.GetAsync(id);
            var data = _mapper.Map<TaskQzForItemResponse>(result);
            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetListAsync(RequestEntity<TaskQzFilterRequest> req)
        {
            var key = req.Filter?.Key;
            var withDisable = req.Filter != null ? req.Filter.WithDisable : false;
            var list = await _SysTaskQzRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Name.Contains(key) || a.Remark.Contains(key) || a.JobGroup.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .Count(out var total)
                .OrderBy(true, a => a.CreatedTime)
                .Page(req.CurrentPage, req.PageSize)
                .ToListAsync();

            var data = new ResponsePageEntity<TaskQzForListResponse>()
            {
                List = _mapper.Map<List<TaskQzForListResponse>>(list),
                Total = total
            };
            data.List.ToList().ForEach(x => {
                x.TriggerState = _schedulerCenter.GetJobRunTriggerState(x.Id.ToString(), x.JobGroup);
                x.NextFireTime = _schedulerCenter.GetJobNextFireTime(x.Id.ToString(), x.JobGroup);
            });
            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetAllAsync(TaskQzFilterRequest req)
        {
            var key = req?.Key;
            var withDisable = req != null ? req.WithDisable : false;
            var list = await _SysTaskQzRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Name.Contains(key) || a.Remark.Contains(key) || a.JobGroup.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .OrderBy(true, a => a.CreatedTime)
                .ToListAsync();

            var result = _mapper.Map<List<TaskQzForListResponse>>(list);
            return ResponseEntity.Ok(result);
        }
        #endregion

        #region 特殊接口服务实现

        /// <summary>
        /// 根据jobid获取单个job
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<SysTaskQzEntity> GetSysTaskQzEntity(string jobId)
        {

            return await _SysTaskQzRepository.FindAsync(jobId);
        }


        /// <summary>
        /// 更新计划任务
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateTaskQz(SysTaskQzEntity entity)
        {
            return await _SysTaskQzRepository.UpdateAsync(entity);
        }


        #endregion

    }

}
