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
    public class SysConfigService : ISysConfigService
    {
        private readonly IMapper _mapper;
        private readonly ICache _cache;
        private readonly ISysConfigRepository _sysConfigRepository;
        public SysConfigService(
            IMapper mapper,
            ICache cache,
            ISysConfigRepository sysRoleRepository,
            IRoleDomain roleDomain
            )
        {
            _mapper = mapper;
            _cache = cache;
            _sysConfigRepository = sysRoleRepository;
        }

        #region 通用接口服务实现 对应通用接口
        public async Task<IResponseEntity> CreateAsync(ConfigAddRequest req)
        {
            var item = _mapper.Map<SysConfigEntity>(req);
            var result = await _sysConfigRepository.InsertAsync(item);
            return ResponseEntity.Result(result != null && result?.Id != "");
        }

        public async Task<IResponseEntity> UpdateAsync(ConfigEditRequest req)
        {

            if (req == null || req?.Id == "")
            {
                return ResponseEntity.Error("更新的实体主键丢失");
            }

            var entity = _mapper.Map<SysConfigEntity>(req);
            await _sysConfigRepository.UpdateAsync(entity);
            return ResponseEntity.Ok("更新成功");
        }

        public async Task<IResponseEntity> DeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysConfigRepository.DeleteAsync(id);
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> DeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysConfigRepository.Where(m => ids.Contains(m.Id)).ToDelete().ExecuteAffrowsAsync();
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> SoftDeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysConfigRepository.SoftDeleteAsync(id);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> SoftDeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysConfigRepository.SoftDeleteAsync(ids);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> GetItemAsync(string id)
        {

            var result = await _sysConfigRepository.GetAsync(id);
            var data = _mapper.Map<RoleForItemResponse>(result);
            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetListAsync(RequestEntity<ConfigFilterRequest> req)
        {
            var key = req.Filter?.Key;
            var withDisable = req.Filter != null ? req.Filter.WithDisable : false;
            var list = await _sysConfigRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key) || a.Description.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .Count(out var total)               
                .OrderBy(true, a => a.OrderIndex)
                .Page(req.CurrentPage, req.PageSize)
                .ToListAsync();

            var data = new ResponsePageEntity<ConfigForListResponse>()
            {
                List = _mapper.Map<List<ConfigForListResponse>>(list),
                Total = total
            };

            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetAllAsync(ConfigFilterRequest req)
        {
            var key = req?.Key;
            var withDisable = req != null ? req.WithDisable : false;
            var list = await _sysConfigRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key) || a.Description.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .OrderBy(true, a => a.OrderIndex)
                .ToListAsync();

            var result = _mapper.Map<List<ConfigForListResponse>>(list);
            return ResponseEntity.Ok(result);
        }
        #endregion

        #region 特殊接口服务实现


        #endregion

    }
}
