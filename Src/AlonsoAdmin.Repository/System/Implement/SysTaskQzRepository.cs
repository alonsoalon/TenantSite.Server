using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Repository.System.Interface;
using FreeSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace AlonsoAdmin.Repository.System.Implement
{
    public class SysTaskQzRepository : RepositoryBase<SysTaskQzEntity>, ISysTaskQzRepository
    {

        private readonly IAuthUser _user;
        private readonly ILogger<SysTaskQzRepository> _logger;

        public SysTaskQzRepository(IMultiTenantDbFactory dbFactory, IAuthUser user, ILogger<SysTaskQzRepository> logger)
            : base(dbFactory.Db(Constants.SystemDbKey), user)
        {
            _user = user;
            _logger = logger;
        }

    }

}
