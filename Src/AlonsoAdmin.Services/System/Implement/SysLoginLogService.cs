using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Common.Utils;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Repository.System;
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
        public async Task<IResponseEntity> AddAsync(LoginLogAddRequest req)
        {
            var entity = _mapper.Map<SysLoginLogEntity>(req);

            var item = await _loginLogRepository.InsertAsync(entity);
            return ResponseEntity.Result(item?.Id != "");
        }

        public async Task<IResponseEntity> PageAsync(RequestEntity<SysLoginLogEntity> req)
        {
            var userName = req.Filter?.CreatedByName;

            var list = await _loginLogRepository.Select
            .WhereIf(userName.IsNotNull(), a => a.CreatedByName.Contains(userName))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(req.CurrentPage, req.PageSize)
            .ToListAsync<LoginLogResponse>();

            var data = new PageEntity<LoginLogResponse>()
            {
                List = list,
                Total = total
            };

            return ResponseEntity.Ok(data);
        }
    }
}
