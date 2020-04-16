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
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Implement
{
    public class SysGroupService : ISysGroupService
    {
        private readonly IMapper _mapper;
        private readonly ISysGroupRepository _sysGroupRepository;
        public SysGroupService(
            IMapper mapper,
            ISysGroupRepository sysGroupRepository
            )
        {
            _mapper = mapper;
            _sysGroupRepository = sysGroupRepository;
        }
        public async Task<IResponseEntity> CreateAsync(GroupAddRequest req)
        {
            var item = _mapper.Map<SysGroupEntity>(req);
            var result = await _sysGroupRepository.InsertAsync(item);
            return ResponseEntity.Result(result != null && result?.Id != "");
        }

        public async Task<IResponseEntity> DeleteAsync(string id)
        {
            var result = await _sysGroupRepository.DeleteAsync(id);
            return ResponseEntity.Result(result > 0);
        }

        public Task<IResponseEntity> DeleteBatchAsync(string[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponseEntity> GetAllAsync(GroupFilterRequest req)
        {
            var key = req?.Key;

            var list = await _sysGroupRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key)))
                .Count(out var total)
                .OrderBy(true, a => a.OrderIndex)
                .ToListAsync();

            var result = _mapper.Map<List<GroupListResponse>>(list);
            return ResponseEntity.Ok(result);
        }

        public async Task<IResponseEntity> GetListAsync(RequestEntity<GroupFilterRequest> req)
        {
            var key = req.Filter?.Key;
                
            var list = await _sysGroupRepository.Select
                .WhereIf(key.IsNotNull(), a => (a.Title.Contains(key) || a.Code.Contains(key)))
                .Count(out var total)               
                .OrderBy(true, a => a.OrderIndex)
                .Page(req.CurrentPage, req.PageSize)
                .ToListAsync();

            var data = new PageEntity<GroupListResponse>()
            {
                List = _mapper.Map<List<GroupListResponse>>(list),
                Total = total
            };

            return ResponseEntity.Ok(data);
        }

        public async Task<IResponseEntity> SoftDeleteAsync(string id)
        {
            if (id == null || id == "")
            {
                return ResponseEntity.Error("删除对象的主键获取失败");
            }

            var result = await _sysGroupRepository.SoftDeleteAsync(id);
            return ResponseEntity.Result(result );
        }

        public Task<IResponseEntity> SoftDeleteBatchAsync(string[] ids)
        {
            throw new NotImplementedException();
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
            return ResponseEntity.Ok("更新成功");
        }
    }
}
