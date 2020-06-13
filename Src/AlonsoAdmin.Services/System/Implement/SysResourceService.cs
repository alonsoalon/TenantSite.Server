using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Common.RequestEntity;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Domain.System.Interface;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Entities.System.Enums;
using AlonsoAdmin.Repository;
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
    public class SysResourceService : ISysResourceService
    {
        private readonly IMapper _mapper;
        private readonly ICache _cache;
        private readonly ISysResourceRepository _sysResourceRepository;
        private readonly ISysRResourceApiRepository _sysRResourceApiRepository;
        private readonly IResourceDomain _resourceDomain;
        public SysResourceService(
            IMapper mapper,
            ICache cache,
            ISysResourceRepository sysResourceRepository,
            ISysRResourceApiRepository sysRResourceApiRepository,
            IResourceDomain resourceDomain
            )
        {
            _mapper = mapper;
            _cache = cache;
            _sysResourceRepository = sysResourceRepository;
            _sysRResourceApiRepository = sysRResourceApiRepository;
            _resourceDomain = resourceDomain;

        }

        #region 通用接口服务实现 对应通用接口
        public async Task<IResponseEntity> CreateAsync(ResourceAddRequest req)
        {
            var item = _mapper.Map<SysResourceEntity>(req);
            var result = await _sysResourceRepository.InsertAsync(item);
            //清除缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.PermissionResourceList);
            return ResponseEntity.Result(result != null && result?.Id != "");
        }

        public async Task<IResponseEntity> UpdateAsync(ResourceEditRequest req)
        {

            if (req == null || req?.Id == "")
            {
                return ResponseEntity.Error("更新的实体主键丢失");
            }

            //var entity = await _sysResourceRepository.GetAsync(req.Id);
            //if (entity == null || entity?.Id == "")
            //{
            //    return ResponseEntity.Error("找不到更新的实体！");
            //}
            //_mapper.Map(req, entity);

            var entity = _mapper.Map<SysResourceEntity>(req);
            await _sysResourceRepository.UpdateAsync(entity);
            //清除缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.PermissionResourceList);
            return ResponseEntity.Ok("更新成功");
        }

        public async Task<IResponseEntity> DeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysResourceRepository.DeleteAsync(id);
            //清除缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.PermissionResourceList);
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> DeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysResourceRepository.Where(m => ids.Contains(m.Id)).ToDelete().ExecuteAffrowsAsync();
            //清除缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.PermissionResourceList);
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> SoftDeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysResourceRepository.SoftDeleteAsync(id);
            //清除缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.PermissionResourceList);

            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> SoftDeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysResourceRepository.SoftDeleteAsync(ids);

            //清除缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.PermissionResourceList);


            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> GetItemAsync(string id)
        {

            var result = await _sysResourceRepository.GetAsync(id);
            var data = _mapper.Map<ResourceForItemResponse>(result);
            return ResponseEntity.Ok(data);

        }

        public async Task<IResponseEntity> GetListAsync(RequestEntity<ResourceFilterRequest> req)
        {
            var key = req.Filter?.Key;
            var withDisable = req.Filter != null ? req.Filter.WithDisable : false;
            var list = await _sysResourceRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .Count(out var total)               
                .OrderBy(true, a => a.OrderIndex)
                .Page(req.CurrentPage, req.PageSize)
                .ToListAsync();

            var data = new ResponsePageEntity<ResourceForListResponse>()
            {
                List = _mapper.Map<List<ResourceForListResponse>>(list),
                Total = total
            };

            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetAllAsync(ResourceFilterRequest req)
        {
            var key = req?.Key;
            var withDisable = req != null ? req.WithDisable : false;
            var list = await _sysResourceRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .OrderBy(true, a => a.OrderIndex)
                .ToListAsync();

            var result = _mapper.Map<List<ResourceForListResponse>>(list);
            return ResponseEntity.Ok(result);
        }
        #endregion

        #region 特殊接口服务实现

        public async Task<IResponseEntity> GetResourceApisByIdAsync(string resourceId) {
            var apis = await _sysRResourceApiRepository.Select
                .Where(a => a.ResourceId == resourceId)
                .Include(a=>a.Api)
                .ToListAsync(a=>a.Api);    

            return ResponseEntity.Ok(apis);

        }

        /// <summary>
        /// 更新资源API集合
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<IResponseEntity> UpdateResourceApisByIdAsync(UpdateResourceApiRequest req)
        {
            var result = await _resourceDomain.UpdateResourceApisByIdAsync(req.resourceId, req.ApiIds);

            //清除权限缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.PermissionApiList);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> GetResourcesAsync()
        {
            var resources = await _sysResourceRepository.Select
                .Where(a => a.IsDisabled == false)
                //.OrderBy(a => a.ParentId)
                .OrderBy(a => a.OrderIndex)
                .ToListAsync(a => new { a.Id, a.ParentId, a.Title, a.ResourceType, a.Icon, a.OrderIndex });

            var funcs = resources
                .Where(a => a.ResourceType == ResourceType.Func)
                .OrderBy(a => a.OrderIndex)
                .Select(a => new { a.Id, a.ParentId, a.Title });

            var result = resources
                .Where(a => (new[] { ResourceType.Group, ResourceType.Menu }).Contains(a.ResourceType))
                .OrderBy(a => a.OrderIndex)
                .Select(a => new
                {
                    a.Id,                    
                    a.ParentId,
                    a.Icon,
                    a.Title,
                    funcs = funcs.Where(b => b.ParentId == a.Id).Select(b => new { b.Id, b.Title })
                });

            return ResponseEntity.Ok(result);
        }        

        #endregion

    }
}
