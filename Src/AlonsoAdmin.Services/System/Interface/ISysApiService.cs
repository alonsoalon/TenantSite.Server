using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using System.Collections.Generic;


namespace AlonsoAdmin.Services.System.Interface
{
    public interface ISysApiService
    {
   
        List<SysApiEntity> Query1();
        List<SysApiEntity> Query2();

        List<SysApiEntity> GetList();

        SysApiEntity FindByUrl(string url);
    }
}
