using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Common.RequestEntity;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Entities.Dictionary;
using AlonsoAdmin.Repository.Dictionary.Interface;
using AlonsoAdmin.Services.Dictionary.Interface;
using AlonsoAdmin.Services.Dictionary.Request;
using AlonsoAdmin.Services.Dictionary.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.Dictionary.Implement
{
    public class DictionaryEntryService: IDictionaryEntryService
    {
        private readonly IMapper _mapper; 
        private readonly ICache _cache; 
        private readonly IAuthUser _authUser; 
        private readonly IDictionaryEntryRepository _dictionaryEntryRepository; 

        /// <summary>
        /// 构造函数 参数对象系统自动注入
        /// </summary>
        /// <param name="mapper">autoMapper 映射工具对象</param>
        /// <param name="cache">缓存对象</param>
        /// <param name="authUser">授权用户对象</param>
        /// <param name="dictionaryEntryRepository">仓储对象</param>
        public DictionaryEntryService(
            IMapper mapper,
            ICache cache,
            IAuthUser authUser,
            IDictionaryEntryRepository dictionaryEntryRepository
            )
        {
            _mapper = mapper;
            _cache = cache;
            _authUser = authUser;
            _dictionaryEntryRepository = dictionaryEntryRepository;
        }

        #region 通用接口服务实现(实现IBaseService.CS中定义的接口)
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="req">DTO:新增实体</param>
        /// <returns></returns>
        public async Task<IResponseEntity> CreateAsync(DictionaryEntryAddRequest req)
        {
            var item = _mapper.Map<DictionaryEntryEntity>(req);
            var result = await _dictionaryEntryRepository.InsertAsync(item);
            return ResponseEntity.Result(result != null && result?.Id != "");
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="req">DTO:编辑实体</param>
        /// <returns></returns>
        public async Task<IResponseEntity> UpdateAsync(DictionaryEntryEditRequest req)
        {
            if (req == null || req?.Id == "")
            {
                return ResponseEntity.Error("更新的实体主键丢失");
            }
            var entity = _mapper.Map<DictionaryEntryEntity>(req);
            await _dictionaryEntryRepository.UpdateAsync(entity);
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
            var result = await _dictionaryEntryRepository.DeleteAsync(id);
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
            var result = await _dictionaryEntryRepository.Where(m => ids.Contains(m.Id)).ToDelete().ExecuteAffrowsAsync();
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
            var result = await _dictionaryEntryRepository.SoftDeleteAsync(id);
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
            var result = await _dictionaryEntryRepository.SoftDeleteAsync(ids);
            return ResponseEntity.Result(result);
        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<IResponseEntity> GetItemAsync(string id)
        {
            var result = await _dictionaryEntryRepository.GetAsync(id);
            var data = _mapper.Map<DictionaryHeaderForItemResponse>(result);
            return ResponseEntity.Ok(data);
        }

        /// <summary>
        /// 得到查询条件的分页列表数据
        /// </summary>
        /// <param name="req">带分页属性的DTO查询对象</param>
        /// <returns></returns>
        public async Task<IResponseEntity> GetListAsync(RequestEntity<DictionaryEntryFilterRequest> req)
        {
            var key = req.Filter?.Key;
            var headerId = req.Filter?.DictionaryHeaderId;
            var withDisable = req.Filter != null ? req.Filter.WithDisable : false;
            var list = await _dictionaryEntryRepository.Select
                .WhereIf(headerId.IsNotNull(), a => a.DictionaryHeaderId == headerId)
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key) || a.Description.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .Count(out var total)
                .OrderBy(true, a => a.OrderIndex)
                .Page(req.CurrentPage, req.PageSize)
                .ToListAsync();
            var data = new ResponsePageEntity<DictionaryEntryForListResponse>()
            {
                List = _mapper.Map<List<DictionaryEntryForListResponse>>(list),
                Total = total
            };
            return ResponseEntity.Ok(data);
        }
        /// <summary>
        /// 得到查询条件的所有数据
        /// </summary>
        /// <param name="req">DTO查询对象</param>
        /// <returns></returns>
        public async Task<IResponseEntity> GetAllAsync(DictionaryEntryFilterRequest req)
        {
            var key = req?.Key;
            var headerId = req?.DictionaryHeaderId;
            var withDisable = req != null ? req.WithDisable : false;
            var list = await _dictionaryEntryRepository.Select
                .WhereIf(headerId.IsNotNull(), a => a.DictionaryHeaderId == headerId)
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key) || a.Description.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .OrderBy(true, a => a.OrderIndex)
                .ToListAsync();

            var result = _mapper.Map<List<DictionaryEntryForListResponse>>(list);
            return ResponseEntity.Ok(result);
        }
        #endregion

        #region 特殊接口服务实现

        #endregion

    }
}
