﻿using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Repository.System.Interface;

namespace AlonsoAdmin.Repository.System.Implement
{
    public class SysConfigRepository : RepositoryBase<SysConfigEntity>, ISysConfigRepository
    {
        public SysConfigRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.SystemDbKey), user)
        {

        }

    }
}
