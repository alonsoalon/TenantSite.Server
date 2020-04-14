using AlonsoAdmin.Common;
using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Common.IdGenerator;
using AlonsoAdmin.MultiTenant;
using AlonsoAdmin.MultiTenant.Extensions;
using FreeSql.Aop;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AlonsoAdmin.Repository
{
    public class MultiTenantDbFactory : IMultiTenantDbFactory
    {
        private IHttpContextAccessor _accessor;
        private IdleBus<IFreeSql> _ib;
        private IAuthUser _authUser;
        private IOptionsMonitor<SystemConfig> _systemConfig;
        private IWebHostEnvironment _env;
        public MultiTenantDbFactory(
            IHttpContextAccessor accessor, 
            IdleBus<IFreeSql> ib,
            IAuthUser authUser,
            IOptionsMonitor<SystemConfig> systemConfig,
            IWebHostEnvironment env
            )
        {
            _accessor = accessor;
            _ib = ib;
            _authUser = authUser;
            _systemConfig = systemConfig;
            _env = env;
        }

        public TenantInfo Tenant
        {
            get
            {
                var tenantInfo = _accessor.HttpContext.GetMultiTenantContext()?.TenantInfo;
                if (tenantInfo == null)
                {
                    throw new Exception("没有找到租户信息");
                }
                return tenantInfo;
            }
        }


        public IFreeSql Db(string dbKey = "default")
        {
            var currentDbOption = Tenant.DbOptions.Where(e => e.Key.ToLower() == dbKey.ToLower()).FirstOrDefault();
            if (currentDbOption == null)
            {
                throw new Exception("没有找到对应数据库信息,请检查key是否配置正确");
            }

            var dbType = (FreeSql.DataType)Enum.Parse(typeof(FreeSql.DataType), currentDbOption.DbType);
            var fristName = $"{Tenant.Id}#{dbKey}#";
            var dbCacheKey = fristName + currentDbOption.SerializeToString().Md5();

            var oldDb = _ib.TryGet(dbCacheKey);

            if (oldDb == null)
            {
                _ib.TryRegister(dbCacheKey, () => CreateDb(dbType, currentDbOption));

                return _ib.TryGet(dbCacheKey);
            }
            else
            {
                return oldDb;
            }
        }

        private IFreeSql CreateDb(FreeSql.DataType dbType, DbInfo currentDbOption)
        {
             var master = currentDbOption.ConnectionStrings?.FirstOrDefault(e => e.UseType == DbUseType.Master);
            if (master == null)
            {
                throw new ArgumentNullException($"请设置租户 {Tenant.Code} 的主库连接字符串");
            }
            var slaveConnectionStrings = currentDbOption.ConnectionStrings?.Where(e => e.UseType == DbUseType.Slave).Select(e => e.ConnectionString).ToArray();
            var freeSqlBuilder = new FreeSql.FreeSqlBuilder()
               .UseConnectionString(dbType, master.ConnectionString);
            if (slaveConnectionStrings?.Length > 0)
            {
                freeSqlBuilder = freeSqlBuilder.UseSlave(slaveConnectionStrings);
            }

            if (_env.IsDevelopment()) {
                freeSqlBuilder = freeSqlBuilder.UseAutoSyncStructure(true); //自动同步实体结构【开发环境必备】
            }

            var fsql = freeSqlBuilder.Build();

            fsql.Aop.CurdBefore += CurdBefore;
            fsql.Aop.AuditValue += AuditValue;
            //fsql.Aop.SyncStructureAfter += SyncStructureAfter;

            return fsql;
        }

        private void AuditValue(object s, AuditValueEventArgs e) {

            if (e.AuditValueType == AuditValueType.Insert
                && e.Property.Name == "Id"
                && e.Column.CsType == typeof(long)
                && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) != null
                )
            {
                var sf = Snowflake.Instance();
                var dataCenterId = _systemConfig.CurrentValue?.DataCenterId ?? 5;
                var workId = _systemConfig.CurrentValue?.WorkId ?? 20;
                sf.Init(dataCenterId, workId);
                var id = sf.NextId();
                e.Value = id;
            }

            if (_authUser == null || !(_authUser.Id > 0))
            {
                return;
            }

            if (e.AuditValueType == FreeSql.Aop.AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case "CreatedBy":
                        e.Value = _authUser.Id;
                        break;
                    case "CreatedByName":
                        e.Value = _authUser.UserName;
                        break;
                        //case "CreatedTime":
                        //    e.Value = DateTime.Now.Subtract(timeOffset);
                        //    break;
                }
            }
            else if (e.AuditValueType == FreeSql.Aop.AuditValueType.Update)
            {
                switch (e.Property.Name)
                {
                    case "UpdatedBy":
                        e.Value = _authUser.Id;
                        break;
                    case "UpdatedByName":
                        e.Value = _authUser.UserName;
                        break;
                        //case "UpdatedTime":
                        //    e.Value = DateTime.Now.Subtract(timeOffset);
                        //    break;
                }
            }

        }

        private void CurdBefore(object s, CurdBeforeEventArgs e) {
            if (_systemConfig.CurrentValue != null && _systemConfig.CurrentValue.WatchCurd)
            {
                Console.WriteLine($"{e.Sql}\r\n");
            }
        }

        


    }
}
