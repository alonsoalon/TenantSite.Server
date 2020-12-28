﻿using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Common.RequestEntity;
using AlonsoAdmin.Common.ResponseEntity;
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
    public class SysGroupService : ISysGroupService
    {
        private readonly IMapper _mapper;
        private readonly ICache _cache;
        private readonly IAuthUser _authUser;        
        private readonly ISysGroupRepository _sysGroupRepository;
        public SysGroupService(
            IMapper mapper,
            ICache cache,
            IAuthUser authUser,
            ISysGroupRepository sysGroupRepository
            )
        {
            _mapper = mapper;
            _cache = cache;
            _authUser= authUser;
            _sysGroupRepository = sysGroupRepository;
        }


        #region 通用接口服务实现 对应通用接口
        public async Task<IResponseEntity> CreateAsync(GroupAddRequest req)
        {
            var item = _mapper.Map<SysGroupEntity>(req);
            var result = await _sysGroupRepository.InsertAsync(item);

            //清除缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.GroupList);

            return ResponseEntity.Result(result != null && result?.Id != "");
        }

        public async Task<IResponseEntity> UpdateAsync(GroupEditRequest req)
        {

            if (req == null || req?.Id == "")
            {
                return ResponseEntity.Error("更新的实体主键丢失");
            }

            //var entity = await _sysGroupRepository.GetAsync(req.Id);
            //if (entity == null || entity?.Id == "")
            //{
            //    return ResponseEntity.Error("找不到更新的实体！");
            //}
            //_mapper.Map(req, entity);

            var entity = _mapper.Map<SysGroupEntity>(req);
            await _sysGroupRepository.UpdateAsync(entity);

            //清除缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.GroupList);

            return ResponseEntity.Ok("更新成功");
        }

        public async Task<IResponseEntity> DeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysGroupRepository.DeleteAsync(id);

            //清除缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.GroupList);

            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> DeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysGroupRepository.Where(m => ids.Contains(m.Id)).ToDelete().ExecuteAffrowsAsync();

            //清除缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.GroupList);

            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> SoftDeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysGroupRepository.SoftDeleteAsync(id);

            //清除缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.GroupList);

            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> SoftDeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysGroupRepository.SoftDeleteAsync(ids);

            //清除缓存
            await _cache.RemoveByPatternAsync(CacheKeyTemplate.GroupList);

            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> GetItemAsync(string id)
        {

            var result = await _sysGroupRepository.GetAsync(id);
            var data = _mapper.Map<GroupForItemResponse>(result);
            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetListAsync(RequestEntity<GroupFilterRequest> req)
        {

            var key = req.Filter?.Key;
            var withDisable = req.Filter!=null ? req.Filter.WithDisable : false;

            var list = await _sysGroupRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .Count(out var total)
                .OrderBy(true, a => a.OrderIndex)
                .Page(req.CurrentPage, req.PageSize)
                .ToListAsync();

            var data = new ResponsePageEntity<GroupForListResponse>()
            {
                List = _mapper.Map<List<GroupForListResponse>>(list),
                Total = total
            };

            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetAllAsync(GroupFilterRequest req)
        {
            var withDisable = req != null ? req.WithDisable : false;

            var key = req?.Key;
            var data = await _sysGroupRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .OrderBy(true, a => a.OrderIndex)
                .ToListAsync();
            var result = _mapper.Map<List<GroupForListResponse>>(data);
            return ResponseEntity.Ok(result);


            //var cacheKey = string.Format(CacheKeyTemplate.PermissionGroupList, _authUser.PermissionId, withDisable ? "withDisable" : "onlyEnable");
            //List<SysGroupEntity> data = new List<SysGroupEntity>();
            //if (await _cache.ExistsAsync(cacheKey))
            //{
            //    data = await _cache.GetAsync<List<SysGroupEntity>>(cacheKey);
               
            //}
            //else
            //{


            //    var key = req?.Key;
            //    data = await _sysGroupRepository.Select
            //        .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key)))
            //        .WhereIf(!withDisable, a => a.IsDisabled == false)
            //        .OrderBy(true, a => a.OrderIndex)
            //        .ToListAsync();
            //    await _cache.SetAsync(cacheKey, data);

              

            //}
            //var result = _mapper.Map<List<GroupForListResponse>>(data);
            //return ResponseEntity.Ok(result);
        }
        #endregion

        #region 特殊接口服务实现




        #endregion

    }
}
