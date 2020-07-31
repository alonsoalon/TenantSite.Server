using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Common.RequestEntity;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Domain.System.Interface;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Repository.System;
using AlonsoAdmin.Repository.System.Interface;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using AlonsoAdmin.Services.System.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Implement
{
    public class SysConditionService : ISysConditionService
    {
        private readonly IMapper _mapper;
        private readonly ICache _cache;
        private readonly IAuthUser _authUser;
        private readonly ISysConditionRepository _sysConditionRepository;

        /// <summary>
        /// 构造函数 参数对象系统自动注入
        /// </summary>
        /// <param name="mapper">autoMapper 映射工具对象</param>
        /// <param name="cache">缓存对象</param>
        /// <param name="authUser">授权用户对象</param>
        /// <param name="sysConditionRepository">仓储对象</param>
        public SysConditionService(
            IMapper mapper,
            ICache cache,
            IAuthUser authUser,
            ISysConditionRepository sysConditionRepository
            )
        {
            _mapper = mapper;
            _cache = cache;
            _authUser = authUser;
            _sysConditionRepository = sysConditionRepository;
        }

        #region 通用接口服务实现(实现IBaseService.CS中定义的接口)
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="req">DTO:新增实体</param>
        /// <returns></returns>
        public async Task<IResponseEntity> CreateAsync(ConditionAddRequest req)
        {
            var code = req.Code;
            if (code == null || code == "")
            {
                return ResponseEntity.Error("字典编码不能为空");
            }
            //var count = _sysConditionRepository.Select.Where(x => x.Code == code).Count();
            //if (count > 0)
            //{
            //    return ResponseEntity.Error("已存在相同字典编码，请更换");
            //}

            var item = _mapper.Map<SysConditionEntity>(req);
            var result = await _sysConditionRepository.InsertAsync(item);
            return ResponseEntity.Result(result != null && result?.Id != "");
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="req">DTO:编辑实体</param>
        /// <returns></returns>
        public async Task<IResponseEntity> UpdateAsync(ConditionEditRequest req)
        {
            if (req.Id == "" || req.Code == "")
            {
                return ResponseEntity.Error("更新的实体主键丢失");
            }
            //var count = _sysConditionRepository.Select.Where(x => x.Code == req.Code && x.Id != req.Id).Count();
            //if (count > 0)
            //{
            //    return ResponseEntity.Error("已存在相同字典编码，请更换");
            //}

            var entity = _mapper.Map<SysConditionEntity>(req);
            await _sysConditionRepository.UpdateAsync(entity);
            return ResponseEntity.Ok("更新成功");
        }
        /// <summary>
        /// 删除 - 单条物理删除
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<IResponseEntity> DeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysConditionRepository.DeleteAsync(id);
            return ResponseEntity.Result(result > 0);
        }
        /// <summary>
        /// 删除 - 批量物理删除
        /// </summary>
        /// <param name="ids">待删除的ID集合</param>
        /// <returns></returns>
        public async Task<IResponseEntity> DeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysConditionRepository.Where(m => ids.Contains(m.Id)).ToDelete().ExecuteAffrowsAsync();
            return ResponseEntity.Result(result > 0);
        }
        /// <summary>
        /// 删除 - 单条逻辑删除（软删除）
        /// </summary>
        /// <param name="id">待删除的ID</param>
        /// <returns></returns>
        public async Task<IResponseEntity> SoftDeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysConditionRepository.SoftDeleteAsync(id);
            return ResponseEntity.Result(result);
        }
        /// <summary>
        /// 删除 - 批量逻辑删除（软删除）
        /// </summary>
        /// <param name="ids">待删除的ID集合</param>
        /// <returns></returns>
        public async Task<IResponseEntity> SoftDeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysConditionRepository.SoftDeleteAsync(ids);
            return ResponseEntity.Result(result);
        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<IResponseEntity> GetItemAsync(string id)
        {
            var result = await _sysConditionRepository.GetAsync(id);
            var data = _mapper.Map<ConditionForItemResponse>(result);
            return ResponseEntity.Ok(data);
        }

        /// <summary>
        /// 得到查询条件的分页列表数据
        /// </summary>
        /// <param name="req">带分页属性的DTO查询对象</param>
        /// <returns></returns>
        public async Task<IResponseEntity> GetListAsync(RequestEntity<ConditionFilterRequest> req)
        {
            var key = req.Filter?.Key;
            var withDisable = req.Filter != null ? req.Filter.WithDisable : false;
            var list = await _sysConditionRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key) || a.Description.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .Count(out var total)
                .OrderBy(true, a => a.OrderIndex)
                .Page(req.CurrentPage, req.PageSize)
                .ToListAsync();
            var data = new ResponsePageEntity<ConditionForListResponse>()
            {
                List = _mapper.Map<List<ConditionForListResponse>>(list),
                Total = total
            };
            return ResponseEntity.Ok(data);
        }
        /// <summary>
        /// 得到查询条件的所有数据
        /// </summary>
        /// <param name="req">DTO查询对象</param>
        /// <returns></returns>
        public async Task<IResponseEntity> GetAllAsync(ConditionFilterRequest req)
        {
            var key = req?.Key;
            var withDisable = req != null ? req.WithDisable : false;
            var list = await _sysConditionRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key) || a.Description.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .OrderBy(true, a => a.OrderIndex)
                .ToListAsync();

            var result = _mapper.Map<List<ConditionForListResponse>>(list);
            return ResponseEntity.Ok(result);
        }
        #endregion

        #region 特殊接口服务实现

        #endregion

    }
}
