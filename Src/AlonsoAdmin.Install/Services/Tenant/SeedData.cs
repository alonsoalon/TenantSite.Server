using AlonsoAdmin.Entities.System;
using AlonsoAdmin.MultiTenant;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Install.Services.Tenant
{
    public class SeedData
    {

        public SeedData()
        {}

        /// <summary>
        /// 得到种子数据
        /// </summary>
        /// <returns></returns>
        public SeedDataEntity GetSeedData()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"SeedData.json");
            var jsonData = ReadFile(filePath);
            var data = JsonConvert.DeserializeObject<SeedDataEntity>(jsonData);
            return data;
        }

        /// <summary>
        /// 生成种子数据
        /// </summary>
        public void GenerateSeedData()
        {

            IFreeSql fsql = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.MySql, "Server=localhost; Port=3306; Database=Tenant1db; Uid=root; Pwd=000000; Charset=utf8mb4;")
            .Build();

            var sysApiEntities = fsql.GetRepository<SysApiEntity>().Select.Where(x => x.IsDeleted == false).ToList();
            var SysConditionEntities = fsql.GetRepository<SysConditionEntity>().Select.Where(x => x.IsDeleted == false).ToList();
            var SysDictionaryDetailEntities = fsql.GetRepository<SysDictionaryDetailEntity>().Select.Where(x => x.IsDeleted == false).ToList();
            var SysDictionaryEntities = fsql.GetRepository<SysDictionaryEntity>().Select.Where(x => x.IsDeleted == false).ToList();
            var SysGroupEntities = fsql.GetRepository<SysGroupEntity>().Select.Where(x => x.IsDeleted == false).ToList();
            var SysPermissionEntities = fsql.GetRepository<SysPermissionEntity>().Select.Where(x => x.IsDeleted == false).ToList();
            var SysResourceEntities = fsql.GetRepository<SysResourceEntity>().Select.Where(x => x.IsDeleted == false).ToList();
            var SysRoleEntities = fsql.GetRepository<SysRoleEntity>().Select.Where(x => x.IsDeleted == false).ToList();
            var SysRPermissionConditionEntities = fsql.GetRepository<SysRPermissionConditionEntity>().Select.ToList();
            var SysRPermissionGroupEntities = fsql.GetRepository<SysRPermissionGroupEntity>().Select.ToList();
            var SysRPermissionRoleEntities = fsql.GetRepository<SysRPermissionRoleEntity>().Select.ToList();
            var SysRResourceApiEntities = fsql.GetRepository<SysRResourceApiEntity>().Select.ToList();
            var SysRRoleResourceEntities = fsql.GetRepository<SysRRoleResourceEntity>().Select.ToList();
            var SysSettingEntities = fsql.GetRepository<SysSettingEntity>().Select.Where(x => x.IsDeleted == false).ToList();
            var SysUserEntities = fsql.GetRepository<SysUserEntity>().Select.Where(x => x.IsDeleted == false).ToList();

            SeedDataEntity seedDataEntity = new SeedDataEntity()
            {
                SysApiEntities = sysApiEntities,
                SysConditionEntities = SysConditionEntities,
                SysDictionaryDetailEntities = SysDictionaryDetailEntities,
                SysDictionaryEntities = SysDictionaryEntities,
                SysGroupEntities = SysGroupEntities,
                SysPermissionEntities = SysPermissionEntities,
                SysResourceEntities = SysResourceEntities,
                SysRoleEntities = SysRoleEntities,
                SysRPermissionConditionEntities = SysRPermissionConditionEntities,
                SysRPermissionGroupEntities = SysRPermissionGroupEntities,
                SysRPermissionRoleEntities = SysRPermissionRoleEntities,
                SysRResourceApiEntities = SysRResourceApiEntities,
                SysRRoleResourceEntities = SysRRoleResourceEntities,
                SysSettingEntities = SysSettingEntities,
                SysUserEntities = SysUserEntities
            };


            WriteSeedData(seedDataEntity);


            fsql.Dispose();

        }

        #region 私有方法
        private void WriteSeedData(SeedDataEntity seedDataEntity)
        {
            //var baseDirectory = AppContext.BaseDirectory;
            //var filePath = Path.Combine(baseDirectory, "SeedData.json");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"SeedData.json");

            JObject jObject = JObject.Parse(JsonConvert.SerializeObject(seedDataEntity));
            File.WriteAllText(filePath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }

        private string ReadFile(string Path)
        {
            string s;
            if (!File.Exists(Path))
                s = "不存在相应的目录";
            else
            {
                StreamReader streamReader = new StreamReader(Path);
                s = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();
            }

            return s;
        }
        #endregion
    }

    /// <summary>
    /// 种子数据实体类
    /// </summary>
    public class SeedDataEntity
    {
        public List<SysApiEntity> SysApiEntities { get; set; }
        public List<SysConditionEntity> SysConditionEntities { get; set; }
        public List<SysDictionaryDetailEntity> SysDictionaryDetailEntities { get; set; }
        public List<SysDictionaryEntity> SysDictionaryEntities { get; set; }
        public List<SysGroupEntity> SysGroupEntities { get; set; }
        public List<SysPermissionEntity> SysPermissionEntities { get; set; }
        public List<SysResourceEntity> SysResourceEntities { get; set; }
        public List<SysRoleEntity> SysRoleEntities { get; set; }        
        public List<SysRPermissionConditionEntity> SysRPermissionConditionEntities { get; set; }
        public List<SysRPermissionGroupEntity> SysRPermissionGroupEntities { get; set; }
        public List<SysRPermissionRoleEntity> SysRPermissionRoleEntities { get; set; }
        public List<SysRResourceApiEntity> SysRResourceApiEntities { get; set; }
        public List<SysRRoleResourceEntity> SysRRoleResourceEntities { get; set; }
        public List<SysSettingEntity> SysSettingEntities { get; set; }
        public List<SysUserEntity> SysUserEntities { get; set; }
    }
}