using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Repository.System;
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
    public class SysPermissionService : ISysPermissionService
    {
        private readonly IMapper _mapper;
        private readonly ISysPermissionRepository _sysPermissionRepository;
        public SysPermissionService(
            IMapper mapper,
            ISysPermissionRepository sysPermissionRepository
            )
        {
            _mapper = mapper;
            _sysPermissionRepository = sysPermissionRepository;
        }

        #region 通用接口服务实现 对应通用接口
        public async Task<IResponseEntity> CreateAsync(PermissionAddRequest req)
        {
            var item = _mapper.Map<SysPermissionEntity>(req);
            var result = await _sysPermissionRepository.InsertAsync(item);
            return ResponseEntity.Result(result != null && result?.Id != "");
        }

        public async Task<IResponseEntity> UpdateAsync(PermissionEditRequest req)
        {

            if (req == null || req?.Id == "")
            {
                return ResponseEntity.Error("更新的实体主键丢失");
            }

            //var entity = await _sysPermissionRepository.GetAsync(req.Id);
            //if (entity == null || entity?.Id == "")
            //{
            //    return ResponseEntity.Error("找不到更新的实体！");
            //}
            //_mapper.Map(req, entity);

            var entity = _mapper.Map<SysPermissionEntity>(req);
            await _sysPermissionRepository.UpdateAsync(entity);
            return ResponseEntity.Ok("更新成功");
        }

        public async Task<IResponseEntity> DeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysPermissionRepository.DeleteAsync(id);
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> DeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysPermissionRepository.Where(m => ids.Contains(m.Id)).ToDelete().ExecuteAffrowsAsync();
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> SoftDeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysPermissionRepository.SoftDeleteAsync(id);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> SoftDeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysPermissionRepository.SoftDeleteAsync(ids);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> GetItemAsync(string id)
        {

            var result = await _sysPermissionRepository.GetAsync(id);
            var data = _mapper.Map<PermissionForItemResponse>(result);
            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetListAsync(RequestEntity<PermissionFilterRequest> req)
        {
            var key = req.Filter?.Key;
            var withDisable = req.Filter != null ? req.Filter.WithDisable : false;

            var list = await _sysPermissionRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key) || a.Description.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .Count(out var total)
                .OrderBy(true, a => a.OrderIndex)
                .Page(req.CurrentPage, req.PageSize)
                .ToListAsync();

            var data = new PageEntity<PermissionForListResponse>()
            {
                List = _mapper.Map<List<PermissionForListResponse>>(list),
                Total = total
            };

            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetAllAsync(PermissionFilterRequest req)
        {
            var key = req?.Key;
            var withDisable = req != null ? req.WithDisable : false;
            var list = await _sysPermissionRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key) || a.Description.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .OrderBy(true, a => a.OrderIndex)
                .ToListAsync();

            var result = _mapper.Map<List<PermissionForListResponse>>(list);
            return ResponseEntity.Ok(result);
        }
        #endregion

        #region 特殊接口服务实现


        #endregion

    }
}
