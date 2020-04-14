using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Request;
using AlonsoAdmin.Services.System.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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


            ////查询
            //CreateMap<SysUserEntity, UserGetResponse>().ForMember(
            //    d => d.RoleIds,
            //    m => m.MapFrom(s => s.Roles.Select(a => a.Id))
            //);


            //查询列表
            CreateMap<SysUserEntity, UserListResponse>()
                .ForMember(
                d => d.PermissionName, 
                m => m.MapFrom(s => s.Permission.Title)
                );


        }
    }
}
