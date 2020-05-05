using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Common.Utils;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Repository.System;
using AlonsoAdmin.Repository.System.Interface;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using AlonsoAdmin.Services.System.Response;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Implement
{
    public class SysOprationLogService : ISysOprationLogService
    {
        private readonly IAuthUser _user;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly ISysOprationLogRepository _oprationLogRepository;
        public SysOprationLogService(
            IAuthUser user,
            IMapper mapper,
            IHttpContextAccessor accessor,
            ISysOprationLogRepository oprationLogRepository
            )
        {
            _user = user;
            _mapper = mapper;
            _accessor = accessor;
            _oprationLogRepository = oprationLogRepository;
        }
        public async Task<IResponseEntity> CreateAsync(OprationLogAddRequest req)
        {

            var entity = _mapper.Map<SysOperationLogEntity>(req);
            var item = await _oprationLogRepository.InsertAsync(entity);
            return ResponseEntity.Result(item?.Id != "");
        }

        public async Task<IResponseEntity> GetListAsync(RequestEntity<OprationLogFilterRequest> req)
        {
            var key = req.Filter?.Key;

            var list = await _oprationLogRepository.Select
            .WhereIf(key.IsNotNull(), a => a.CreatedByName.Contains(key) || a.RealName.Contains(key) || a.ApiTitle.Contains(key) || a.Ip.Contains(key) || a.ApiPath.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(req.CurrentPage, req.PageSize)
            .ToListAsync<OperationLogForListResponse>();

            var data = new PageEntity<OperationLogForListResponse>()
            {
                List = list,
                Total = total
            };

            return ResponseEntity.Ok(data);
        }
    }
}
