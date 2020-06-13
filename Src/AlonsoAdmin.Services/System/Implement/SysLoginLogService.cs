using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Common.RequestEntity;
using AlonsoAdmin.Common.ResponseEntity;
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
    public class SysLoginLogService : ISysLoginLogService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly ISysLoginLogRepository _loginLogRepository;
        public SysLoginLogService(
           
            IMapper mapper,
            IHttpContextAccessor accessor,
            ISysLoginLogRepository oprationLogRepository
            )
        {
            _mapper = mapper;
            _accessor = accessor;
            _loginLogRepository = oprationLogRepository;
        }
        public async Task<IResponseEntity> CreateAsync(LoginLogAddRequest req)
        {
            var entity = _mapper.Map<SysLoginLogEntity>(req);

            var item = await _loginLogRepository.InsertAsync(entity);
            return ResponseEntity.Result(item?.Id != "");
        }

        public async Task<IResponseEntity> GetListAsync(RequestEntity<LoginLogFilterRequest> req)
        {
            var key = req.Filter?.Key;
            var list = await _loginLogRepository.Select
            .WhereIf(key.IsNotNull(), a => a.CreatedByName.Contains(key) || a.RealName.Contains(key) || a.Ip.Contains(key) )
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(req.CurrentPage, req.PageSize)
            .ToListAsync<LoginLogForListResponse>();

            var data = new ResponsePageEntity<LoginLogForListResponse>()
            {
                List = list,
                Total = total
            };

            return ResponseEntity.Ok(data);
        }
    }
}
