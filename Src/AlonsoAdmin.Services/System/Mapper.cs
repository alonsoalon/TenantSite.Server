using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Request;
using AlonsoAdmin.Services.System.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Services.System
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<SysUserEntity, AuthLoginResponse>();

            CreateMap<LoginLogAddRequest, SysLoginLogEntity>();             

            CreateMap<OprationLogAddRequest, SysOprationLogEntity>();

            
        }
    }
}
