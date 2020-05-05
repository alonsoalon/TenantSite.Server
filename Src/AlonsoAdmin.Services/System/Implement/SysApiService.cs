using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Extensions;
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
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Implement
{
    public class SysApiService : ISysApiService
    {
        private readonly IAuthUser _authUser;
        private readonly ICache _cache;
        private readonly IMapper _mapper;
        private readonly ISysApiRepository _sysApiRepository;
        private readonly IApiDomain _apiDomain;
        public SysApiService(
            IAuthUser authUser,
            ICache cache,
            IMapper mapper,
            ISysApiRepository sysApiRepository,
            IApiDomain apiDomain
            )
        {
            _authUser = authUser;
            _cache = cache;
            _mapper = mapper;
            _sysApiRepository = sysApiRepository;
            _apiDomain = apiDomain;

        }

        #region 通用接口服务实现 对应通用接口
        public async Task<IResponseEntity> CreateAsync(ApiAddRequest req)
        {
            var item = _mapper.Map<SysApiEntity>(req);
            var result = await _sysApiRepository.InsertAsync(item);
            return ResponseEntity.Result(result != null && result?.Id != "");
        }

        public async Task<IResponseEntity> UpdateAsync(ApiEditRequest req)
        {

            if (req == null || req?.Id == "")
            {
                return ResponseEntity.Error("更新的实体主键丢失");
            }

            var entity = _mapper.Map<SysApiEntity>(req);
            await _sysApiRepository.UpdateAsync(entity);
            return ResponseEntity.Ok("更新成功");
        }

        public async Task<IResponseEntity> DeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysApiRepository.DeleteAsync(id);
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> DeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }
            var result = await _sysApiRepository.Where(m => ids.Contains(m.Id)).ToDelete().ExecuteAffrowsAsync();
            return ResponseEntity.Result(result > 0);
        }

        public async Task<IResponseEntity> SoftDeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysApiRepository.SoftDeleteAsync(id);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> SoftDeleteBatchAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysApiRepository.SoftDeleteAsync(ids);
            return ResponseEntity.Result(result);
        }

        public async Task<IResponseEntity> GetItemAsync(string id)
        {

            var result = await _sysApiRepository.GetAsync(id);
            var data = _mapper.Map<ApiForItemResponse>(result);
            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetListAsync(RequestEntity<ApiFilterRequest> req)
        {
            var key = req.Filter?.Key;
            var withDisable = req.Filter != null ? req.Filter.WithDisable : false;
            var list = await _sysApiRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Category.Contains(key) || a.Url.Contains(key) || a.HttpMethod.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .Count(out var total)
                .OrderBy(true, a => a.Category)
                .Page(req.CurrentPage, req.PageSize)
                .ToListAsync();

            var data = new PageEntity<ApiForListResponse>()
            {
                List = _mapper.Map<List<ApiForListResponse>>(list),
                Total = total
            };

            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> GetAllAsync(ApiFilterRequest req)
        {
            var key = req?.Key;
            var withDisable = req != null ? req.WithDisable : false;
            var list = await _sysApiRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Category.Contains(key) || a.Url.Contains(key) || a.HttpMethod.Contains(key)))
                .WhereIf(!withDisable, a => a.IsDisabled == false)
                .ToListAsync();

            var result = _mapper.Map<List<ApiForListResponse>>(list);
            return ResponseEntity.Ok(result);
        }
        #endregion

        #region 特殊接口服务实现

        public async Task<IResponseEntity> GenerateApisAsync(List<SysApiEntity> list) {
            var result= await _apiDomain.GenerateApisAsync(list);
            return ResponseEntity.Result(result);
        }

        #endregion
    }
}
