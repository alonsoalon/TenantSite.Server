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
            CreateMap<OprationLogAddRequest, SysOperationLogEntity>();

            #region User
            // 创建 用到的映射
            CreateMap<UserAddRequest, SysUserEntity>();
            // 更新 用到的映射
            CreateMap<UserEditRequest, SysUserEntity>();
            // 查询 用到的映射
            CreateMap<SysUserEntity, UserForListResponse>().ForMember(
                d => d.PermissionName,
                m => m.MapFrom(s =>s.Permission.Title)
            ).ForMember(
                d => d.GroupName,
                m => m.MapFrom(s => s.Group.Title)
            );


            // 查询单条明细 用到的映射
            CreateMap<SysUserEntity, UserForItemResponse>().ForMember(
                d => d.PermissionName,
                m => m.MapFrom(s => s.Permission.Title)
            ).ForMember(
                d => d.GroupName,
                m => m.MapFrom(s => s.Group.Title)
            );
            #endregion

            #region Group
            // 创建 用到的映射
            CreateMap<GroupAddRequest, SysGroupEntity>();
            // 更新 用到的映射
            CreateMap<GroupEditRequest, SysGroupEntity>();
            // 查询 用到的映射
            CreateMap<SysGroupEntity, GroupForListResponse>();
            // 查询单条明细 用到的映射
            CreateMap<SysGroupEntity, GroupForItemResponse>();
            #endregion

            #region Resource
            // 创建 用到的映射
            CreateMap<ResourceAddRequest, SysResourceEntity>();
            // 更新 用到的映射
            CreateMap<ResourceEditRequest, SysResourceEntity>();
            // 查询列表 用到的映射
            CreateMap<SysResourceEntity, ResourceForListResponse>();
            // 查询单条明细 用到的映射
            CreateMap<SysResourceEntity, ResourceForItemResponse>();

            // 查询菜单 用到的映射
            CreateMap<SysResourceEntity, ResourceForMenuResponse>();
            
            #endregion

            #region Permission
            // 创建 用到的映射
            CreateMap<PermissionAddRequest, SysPermissionEntity>();
            // 更新 用到的映射
            CreateMap<PermissionEditRequest, SysPermissionEntity>();
            // 查询列表 用到的映射
            CreateMap<SysPermissionEntity, PermissionForListResponse>();
            // 查询单条明细 用到的映射
            CreateMap<SysPermissionEntity, PermissionForItemResponse>();
            #endregion

            #region Role
            // 创建 用到的映射
            CreateMap<RoleAddRequest, SysRoleEntity>();
            // 更新 用到的映射
            CreateMap<RoleEditRequest, SysRoleEntity>();
            // 查询列表 用到的映射
            CreateMap<SysRoleEntity, RoleForListResponse>();
            // 查询单条明细 用到的映射
            CreateMap<SysRoleEntity, RoleForItemResponse>();
            #endregion

            #region Api
            // 创建 用到的映射
            CreateMap<ApiAddRequest, SysApiEntity>();
            // 更新 用到的映射
            CreateMap<ApiEditRequest, SysApiEntity>();
            // 查询列表 用到的映射
            CreateMap<SysApiEntity, ApiForListResponse>();
            // 查询单条明细 用到的映射
            CreateMap<SysApiEntity, ApiForItemResponse>();
            #endregion

            #region Condition 数据条件 映射
            // 创建 用到的映射 (DTO -> DO)
            CreateMap<ConditionAddRequest, SysConditionEntity>();
            // 更新 用到的映射 (DTO -> DO)
            CreateMap<ConditionEditRequest, SysConditionEntity>();
            // 查询列表 用到的映射 (DO -> DTO)
            CreateMap<SysConditionEntity, ConditionForListResponse>();
            // 查询单条明细 用到的映射 (DO -> DTO)
            CreateMap<SysConditionEntity, ConditionForItemResponse>();
            #endregion

            #region Config 配置管理 映射
            // 创建 用到的映射 (DTO -> DO)
            CreateMap<ConfigAddRequest, SysConfigEntity>();
            // 更新 用到的映射 (DTO -> DO)
            CreateMap<ConfigEditRequest, SysConfigEntity>();
            // 查询列表 用到的映射 (DO -> DTO)
            CreateMap<SysConfigEntity, ConfigForListResponse>();
            // 查询单条明细 用到的映射 (DO -> DTO)
            CreateMap<SysConfigEntity, ConfigForItemResponse>();
            #endregion

            #region TaskQz

            //新增
            CreateMap<TaskQzAddRequest, SysTaskQzEntity>();
            //编辑
            CreateMap<TaskQzEditRequest, SysTaskQzEntity>();
            //查询列表
            CreateMap<SysTaskQzEntity, TaskQzForListResponse>();
            //查询详情
            CreateMap<SysTaskQzEntity, TaskQzForItemResponse>();

            #endregion
        }
    }
}
