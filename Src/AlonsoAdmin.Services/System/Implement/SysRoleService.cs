using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Extensions;
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
    public class SysRoleService : ISysRoleService
    {
        private readonly IMapper _mapper;
        private readonly ICache _cache;
        private readonly ISysRoleRepository _sysRoleRepository;
        private readonly ISysRRoleResourceRepository _sysRRoleResourceRepository;
        private readonly IRoleDomain _roleDomain;
        public SysRoleService(
            IMapper mapper,
            ICache cache,
            ISysRoleRepository sysRoleRepository,
            ISysRRoleResourceRepository sysRRoleResourceRepository,
            IRoleDomain roleDomain
            )
        {
            _mapper = mapper;
            _cache = cache;
            _sysRoleRepository = sysRoleRepository;
            _sysRRoleResourceRepository = sysRRoleResourceRepository;
            _roleDomain = roleDomain;
        }

        #region 通用接口服务实现 对应通用接口
        public async Task<IResponseEntity> CreateAsync(RoleAddRequest req)
        {
            var item = _mapper.Map<SysRoleEntity>(req);
            var result = await _sysRoleRepository.InsertAsync(item);
            return ResponseEntity.Result(result != null && result?.Id != "");
        }

        public async Task<IResponseEntity> UpdateAsync(RoleEditRequest req)
        {

            if (req == null || req?.Id == "")
            {
                return ResponseEntity.Error("更新的实体主键丢失");
            }

            //var entity = await _sysRoleRepository.GetAsync(req.Id);
            //if (entity == null || entity?.Id == "")
            //{
            //    return ResponseEntity.Error("找不到更新的实体！");
            //}
            //_mapper.Map(req, entity);

            var entity = _mapper.Map<SysRoleEntity>(req);
            await _sysRoleRepository.UpdateAsync(entity);
            return ResponseEntity.Ok("更新成功");
        }

        public async Task<IResponseEntity> DeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysRoleRepository.DeleteAsync(id);
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> DeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysRoleRepository.Where(m => ids.Contains(m.Id)).ToDelete().ExecuteAffrowsAsync();
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> SoftDeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysRoleRepository.SoftDeleteAsync(id);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> SoftDeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysRoleRepository.SoftDeleteAsync(ids);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> GetItemAsync(string id)
        {

            var result = await _sysRoleRepository.GetAsync(id);
            var data = _mapper.Map<RoleForItemResponse>(result);
            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetListAsync(RequestEntity<RoleFilterRequest> req)
        {
            var key = req.Filter?.Key;
            var withDisable = req.Filter != null ? req.Filter.WithDisable : false;
            var list = await _sysRoleRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key) || a.Description.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .Count(out var total)               
                .OrderBy(true, a => a.OrderIndex)
                .Page(req.CurrentPage, req.PageSize)
                .ToListAsync();

            var data = new PageEntity<RoleForListResponse>()
            {
                List = _mapper.Map<List<RoleForListResponse>>(list),
                Total = total
            };

            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetAllAsync(RoleFilterRequest req)
        {
            var key = req?.Key;
            var withDisable = req != null ? req.WithDisable : false;
            var list = await _sysRoleRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key) || a.Description.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .OrderBy(true, a => a.OrderIndex)
                .ToListAsync();

            var result = _mapper.Map<List<RoleForListResponse>>(list);
            return ResponseEntity.Ok(result);
        }
        #endregion

        #region 特殊接口服务实现

       
        public async Task<IResponseEntity> RoleAssignResourcesAsync(RoleResourceAssignRequest req)
        {
         
            var result = await _roleDomain.RoleAssignResourcesAsync(req.RoleId, req.ResourceIds);
            //清除权限缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.PermissionResourceList) ;

            return ResponseEntity.Result(result);
        }


        public async Task<IResponseEntity> GetResourceIdsByIdAsync(string roleId)
        {
            var resourceIds = await _sysRRoleResourceRepository
             .Select.Where(d => d.RoleId == roleId)
             .ToListAsync(a => a.ResourceId);

            return ResponseEntity.Ok(resourceIds);
        }

        #endregion

    }
}
