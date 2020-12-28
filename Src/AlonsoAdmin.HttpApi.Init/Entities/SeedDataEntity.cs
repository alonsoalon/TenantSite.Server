﻿using AlonsoAdmin.Entities.Dictionary;
using AlonsoAdmin.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Init.Entities
{
    /// <summary>
    /// 种子数据实体类
    /// </summary>
    public class SeedDataEntity
    {
        public List<SysApiEntity> SysApiEntities { get; set; }
        public List<SysConditionEntity> SysConditionEntities { get; set; }

        public List<SysGroupEntity> SysGroupEntities { get; set; }
        public List<SysPermissionEntity> SysPermissionEntities { get; set; }
        public List<SysResourceEntity> SysResourceEntities { get; set; }
        public List<SysRoleEntity> SysRoleEntities { get; set; }
        public List<SysRPermissionConditionEntity> SysRPermissionConditionEntities { get; set; }
        public List<SysRPermissionRoleEntity> SysRPermissionRoleEntities { get; set; }
        public List<SysRResourceApiEntity> SysRResourceApiEntities { get; set; }
        public List<SysRRoleResourceEntity> SysRRoleResourceEntities { get; set; }
        public List<SysConfigEntity> SysSettingEntities { get; set; }
        public List<SysUserEntity> SysUserEntities { get; set; }

        public List<SysLoginLogEntity> SysLoginLogEntities { get; set; }
        public List<SysOperationLogEntity> SysOperationLogEntities { get; set; }


        public List<DictionaryEntryEntity> SysDictionaryEntryEntities { get; set; }
        public List<DictionaryHeaderEntity> SysDictionaryHeaderEntities { get; set; }

        public List<SysConfigEntity> SysConfigEntities { get; set; }
    }
}
