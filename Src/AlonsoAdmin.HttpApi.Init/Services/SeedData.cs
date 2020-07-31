using AlonsoAdmin.Entities.Dictionary;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.HttpApi.Init.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Init.Services
{
    public class SeedData
    {
        private readonly string _seedDataFileName = "SeedData.json";
        private readonly string _path = Directory.GetCurrentDirectory();

        public SeedData()
        { }

        /// <summary>
        /// 得到种子数据
        /// </summary>
        /// <returns></returns>
        public SeedDataEntity GetSeedData()
        {
            var filePath = Path.Combine(_path, _seedDataFileName);
            var jsonData = ReadFile(filePath);
            var data = JsonConvert.DeserializeObject<SeedDataEntity>(jsonData);
            return data;
        }

        /// <summary>
        /// 生成种子数据
        /// </summary>
        public void GenerateSeedData(IFreeSql fsql)
        {

            var sysApiEntities = fsql.GetRepository<SysApiEntity>().Select.Where(x => x.IsDeleted == false).ToList();
            var SysConditionEntities = fsql.GetRepository<SysConditionEntity>().Select.Where(x => x.IsDeleted == false).ToList();
            var SysDictionaryEntryEntities = fsql.GetRepository<DictionaryEntryEntity>().Select.Where(x => x.IsDeleted == false).ToList();
            var SysDictionaryHeaderEntities = fsql.GetRepository<DictionaryHeaderEntity>().Select.Where(x => x.IsDeleted == false).ToList();
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
                SysDictionaryEntryEntities = SysDictionaryEntryEntities,
                SysDictionaryHeaderEntities = SysDictionaryHeaderEntities,
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

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), _seedDataFileName);

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
}
