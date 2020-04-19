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
            CreateMap<LoginLogRequest, SysLoginLogEntity>();
            // 操作日志
            CreateMap<OprationLogRequest, SysOprationLogEntity>();

            
            #region Group
            // 创建 用到的映射
            CreateMap<GroupAddRequest, SysGroupEntity>();
            // Group 更新 用到的映射
            CreateMap<GroupEditRequest, SysGroupEntity>();
            // Group 查询 用到的映射
            CreateMap<SysGroupEntity, GroupListResponse>();
            #endregion


            #region Resource
            // 创建 用到的映射
            CreateMap<ResourceAddRequest, SysResourceEntity>();
            // 更新 用到的映射
            CreateMap<ResourceEditRequest, SysResourceEntity>();
            // 查询 用到的映射
            CreateMap<SysResourceEntity, ResourceListResponse>();
            #endregion



        }
    }
}
