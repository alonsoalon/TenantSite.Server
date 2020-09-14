using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.Entities.Dictionary;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.MultiTenant;
using Blazui.Component;
using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.Install.Services.Tenant
{
    public class FormBase: ComponentBase
    {
      
        [Parameter]
        public string TenantId { get; set; }

        [Inject]
        ISettingService SettingService { get; set; }


        [Inject]
        MessageBox MessageBox { get; set; }


        internal List<string> Logs { get; set; } = new List<string>();

        protected TenantInfo TenantItem { get; set; }

        protected TenantOtherOptions TenantOtherItem { get; set; }

        protected BForm configForm;

        private bool WriteTenantSettings()
        {

            TenantItem.Items.Clear();
            TenantItem.Items.Add("Audience", TenantOtherItem.Audience);
            TenantItem.Items.Add("Issuer", TenantOtherItem.Issuer);
            TenantItem.Items.Add("ExpirationMinutes", TenantOtherItem.ExpirationMinutes);
            TenantItem.Items.Add("Secret", TenantOtherItem.Secret);

            var tenants = SettingService.GetTenantListAsync().Result;
            var item = tenants.Where(x => x.Id == TenantId).FirstOrDefault();
            tenants.Remove(item);
            tenants.Add(TenantItem);

            var result = SettingService.WriteTenantsConfig(tenants);

            return result;
        }


            protected void Save() {
            if (!configForm.IsValid())
            {
                return;
            }
            try
            {


                if (WriteTenantSettings())
                {
                    _ = MessageBox.AlertAsync("更新成功");
                }
                else {
                    _ = MessageBox.AlertAsync("更新失败");
                }

                
            }
            catch (Exception ex)
            {
                _ = MessageBox.AlertAsync(ex.Message);
            }

        }

        
        protected void Submit()
        {
            if (!configForm.IsValid())
            {
                return;
            }

            var systemDb = TenantItem.DbOptions.Where(x => x.Key.ToLower() == "system").FirstOrDefault();
            if (systemDb == null)
            {
                _ = MessageBox.AlertAsync("找不到System数据库配置");
                return;
            }
            try
            {

                Logs.Clear();
                Logs.Add("保存配置 开始");
                if (!WriteTenantSettings())
                { 
                    _ = MessageBox.AlertAsync("配置保存失败");
                    return;
                }
                Logs.Add("保存配置 结束");


                Logs.Add("创建数据连接对象 开始");
                var dbType = (FreeSql.DataType)Enum.Parse(typeof(FreeSql.DataType), systemDb.DbType);
                    var connStr = systemDb.ConnectionStrings.Where(x => x.UseType == DbUseType.Master).First().ConnectionString;
                    IFreeSql fsql = new FreeSql.FreeSqlBuilder()
                                .UseConnectionString(dbType, connStr)
                                .Build();
                
                DbConnection dbConnection = fsql.Ado.MasterPool.Get().Value; // 这儿验证 连接是否成功，这句代码可以不要，如果连接字符不变正确，为了提早发现（报异常）
                fsql.Aop.AuditValue += SyncDataAuditValue;
                Logs.Add("创建数据连接对象 结束");


                Logs.Add("同步数据库结构 开始");
                if (dbType == DataType.Oracle)
                {
                    fsql.CodeFirst.IsSyncStructureToUpper = true;
                }
                fsql.CodeFirst.SyncStructure(new Type[]
                {
                    typeof(SysApiEntity), // API 
                    typeof(SysConditionEntity),// 数据条件 相关功能暂未实现,表结构已设计
                    typeof(DictionaryEntryEntity),// 数据字典明细，相关功能暂未实现,表结构已设计
                    typeof(DictionaryHeaderEntity),// 数据字典主表，相关功能暂未实现,表结构已设计
                    typeof(SysGroupEntity), // 数据归属组
                    typeof(SysLoginLogEntity),// 登录日志
                    typeof(SysOperationLogEntity),// API访问日志
                    typeof(SysPermissionEntity),// 权限岗
                    typeof(SysResourceEntity),// 资源
                    typeof(SysRoleEntity),// 角色
                    typeof(SysRPermissionConditionEntity),// 权限岗 与 数据条件关系表，功能未实现
                    typeof(SysRPermissionRoleEntity),// 权限岗 与 角色关系表
                    typeof(SysRResourceApiEntity),// 资源 与 API关系表
                    typeof(SysRRoleResourceEntity),// 角色 与 资源关系表
                    typeof(SysConfigEntity),// 系统设置表，相关功能暂未实现,表结构已设计
                    typeof(SysUserEntity) // 用户表
                });
                Logs.Add("同步数据库结构 结束");


                Logs.Add("种子数据初始化 开始");
                using (var uow = fsql.CreateUnitOfWork())
                using (var tran = uow.GetOrBeginTransaction())
                {
                    SeedDataEntity data = (new SeedData()).GetSeedData();

                    InitDtData(fsql, data.SysApiEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysConditionEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysDictionaryEntryEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysDictionaryHeaderEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysGroupEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysPermissionEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysResourceEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysRoleEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysRPermissionConditionEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysRPermissionRoleEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysRResourceApiEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysRRoleResourceEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysSettingEntities.ToArray(), tran).Wait();
                    InitDtData(fsql, data.SysUserEntities.ToArray(), tran).Wait();

                    uow.Commit();
                }
                Logs.Add("种子数据初始化 结束");

                fsql.Dispose();                
                // _ = MessageBox.AlertAsync("初始化成功");
            }
            catch (Exception ex)
            {
                _ = MessageBox.AlertAsync(ex.Message);
            }
        }

        private static void SyncDataAuditValue(object s, AuditValueEventArgs e)
        {
            if (e.AuditValueType == AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case "CreatedBy":
                        e.Value = "INSTALL";
                        break;
                    case "CreatedByName":
                        e.Value = "INSTALL";
                        break;
                }
            }
            else if (e.AuditValueType == AuditValueType.Update)
            {
                switch (e.Property.Name)
                {
                    case "UpdatedBy":
                        e.Value = "INSTALL";
                        break;
                    case "UpdatedByName":
                        e.Value = "INSTALL";
                        break;
                }
            }
        }

        private async Task InitDtData<T>(
            IFreeSql db,
            T[] data,
            DbTransaction tran
        ) where T : class
        {
            var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
            var tableName = table.Name;

            try
            {
                if (!await db.Queryable<T>().AnyAsync())
                {
                    if (data?.Length > 0)
                    {
                        var insert = db.Insert<T>();

                        if (tran != null)
                        {
                            insert = insert.WithTransaction(tran);
                        }

                        if (db.Ado.DataType == DataType.SqlServer)
                        {
                            var insrtSql = insert.AppendData(data).InsertIdentity().ToSql();
                            await db.Ado.ExecuteNonQueryAsync($"SET IDENTITY_INSERT {tableName} ON\n {insrtSql} \nSET IDENTITY_INSERT {tableName} OFF");
                        }
                        else
                        {
                            await insert.AppendData(data).InsertIdentity().ExecuteAffrowsAsync();
                        }
                        Logs.Add($"table: {tableName} 生成种子数据 成功");
                        Console.WriteLine($"table: {tableName} 生成种子数据 succeed");
                    }
                    else
                    {
                        Logs.Add($"table: {tableName} 无种子数据");
                        Console.WriteLine($"table: {tableName} 无种子数据");
                    }
                }
                else
                {
                    Logs.Add($"table: {tableName} 数据已存在 如果需重新初始化，先清空表");
                    Console.WriteLine($"table: {tableName} 数据已存在 如果需重新初始化，先清空表");
                }
            }
            catch (Exception ex)
            {
                Logs.Add($"table: {tableName} 同步数据失败 \n{ex.Message}");
                Console.WriteLine($"table: {tableName} 同步数据失败 \n{ex.Message}");
            }
        }

        protected void Reset()
        {
            configForm.Reset();
        }

        protected void TestDb() {
            var systemDb = TenantItem.DbOptions.Where(x => x.Key.ToLower() == "system").FirstOrDefault();
            if (systemDb == null) {
                _ = MessageBox.AlertAsync("找不到System数据库配置");
                return;
            }
           
            try
            {
                var dbType = (FreeSql.DataType)Enum.Parse(typeof(FreeSql.DataType), systemDb.DbType);
                var connStr = systemDb.ConnectionStrings.Where(x => x.UseType == DbUseType.Master).First().ConnectionString;
                IFreeSql fsql = new FreeSql.FreeSqlBuilder()
                            .UseConnectionString(dbType, connStr)
                            .Build();

                // 这儿验证 连接是否成功
                //var a = fsql.DbFirst.GetDatabases();
                //Fsql.Ado.MasterPool.Get().Value
                DbConnection dbConnection = fsql.Ado.MasterPool.Get().Value;


                fsql.Dispose();
                _ = MessageBox.AlertAsync("连接成功");
            }
            catch (Exception ex) {
                _ = MessageBox.AlertAsync(ex.Message);
            }
            finally{
               
            }

        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            InitTenant();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (!firstRender)
            {
                StateHasChanged();

                configForm.Refresh();
            }

        }

        protected override bool ShouldRender()
        {
            return true;
        }

        private void InitTenant() {
            
            var tenants = SettingService.GetTenantListAsync().Result;
            var item = tenants.Where(x => x.Id == TenantId).FirstOrDefault();
            TenantItem = item;

            TenantOtherOptions jwtOptions = new TenantOtherOptions();
            if (TenantItem.Items.TryGetValue("Audience",out object audience)) {
                jwtOptions.Audience = audience.ToString();
            }
            if (TenantItem.Items.TryGetValue("ExpirationMinutes", out object expirationMinutes))
            {
                jwtOptions.ExpirationMinutes = expirationMinutes.ToString();
            }
            if (TenantItem.Items.TryGetValue("Issuer", out object issuer))
            {
                jwtOptions.Issuer = issuer.ToString();
            }
            if (TenantItem.Items.TryGetValue("Secret", out object secret))
            {
                jwtOptions.Secret = secret.ToString();
            }

            TenantOtherItem = jwtOptions;

        }
    }
}
