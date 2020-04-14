using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.MultiTenant;
using AlonsoAdmin.Repository;
using AlonsoAdmin.Repository.System;
using AlonsoAdmin.Services.System.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Services.System.Implement
{
    public class SysApiService : ISysApiService
    {
        private ISysApiRepository _sysApiRepository;
        public SysApiService(ISysApiRepository sysApiRepository)
        {
            this._sysApiRepository = sysApiRepository;
        }
 

        public List<SysApiEntity> Query1()
        {
            return _sysApiRepository.Select.ToList();
        }

        public List<SysApiEntity> Query2()
        {
            return _sysApiRepository.Select.ToList();
        }
        public List<SysApiEntity> GetList()
        {
            return _sysApiRepository.Select.ToList();
        }

        public SysApiEntity FindByUrl(string url)
        {
            return _sysApiRepository.Select.First();
        }
    }
}
