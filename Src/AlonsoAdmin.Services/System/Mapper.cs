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
            // 登录
            CreateMap<SysUserEntity, AuthLoginResponse>();

            // 登录日志
            CreateMap<LoginLogAddRequest, SysLoginLogEntity>();        

            // 操作日志
            CreateMap<OprationLogAddRequest, SysOprationLogEntity>();


            // Group 创建、更新 用到的映射
            CreateMap<GroupAddRequest, SysGroupEntity>();


            CreateMap<GroupEditRequest, SysGroupEntity>();

            // Group 查询实体  用到的映射
            CreateMap<SysGroupEntity, GroupListResponse>();




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
