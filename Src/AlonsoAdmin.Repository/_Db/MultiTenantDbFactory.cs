using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.MultiTenant;
using AlonsoAdmin.MultiTenant.Extensions;
using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AlonsoAdmin.Repository
{
    public class MultiTenantDbFactory : IMultiTenantDbFactory
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IdleBus<IFreeSql> _ib;
        private readonly IAuthUser _authUser;
        private readonly IOptionsMonitor<SystemConfig> _systemConfig;
        private readonly IWebHostEnvironment _env;

        private readonly ICache _cache;
        public MultiTenantDbFactory(
            IHttpContextAccessor accessor,
            IdleBus<IFreeSql> ib,
            IAuthUser authUser,
            IOptionsMonitor<SystemConfig> systemConfig,
            IWebHostEnvironment env,
            ICache cache
            )
        {
            _accessor = accessor;
            _ib = ib;
            _authUser = authUser;
            _systemConfig = systemConfig;
            _env = env;
            _cache = cache;
        }

        public TenantInfo Tenant
        {
            get
            {
                var tenantInfo = _accessor.HttpContext.GetMultiTenantContext()?.TenantInfo;
                //if (tenantInfo == null)
                //{
                //    throw new Exception("没有找到租户信息");
                //}
                return tenantInfo;
            }
        }


        public IFreeSql Db(string dbKey = "default")
        {
            if(Tenant == null){
                return null;
            }
           
            var currentDbOption = Tenant.DbOptions.Where(e => e.Key.ToLower() == dbKey.ToLower()).FirstOrDefault();
            if (currentDbOption == null)
            {
                throw new Exception("没有找到对应数据库信息,请检查key是否配置正确");
            }

            var dbType = (FreeSql.DataType)Enum.Parse(typeof(FreeSql.DataType), currentDbOption.DbType);
            var fristName = $"{Tenant.Id}#{dbKey}#";
            var dbCacheKey = fristName + currentDbOption.SerializeToString().Md5();
            // TryRegister 内部做了如果存在KEY，就什么不做，不存在就创建
            _ib.TryRegister(dbCacheKey, () => CreateDb(dbType, currentDbOption));
            return _ib.Get(dbCacheKey);
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

            if (_env.IsDevelopment())
            {
                freeSqlBuilder = freeSqlBuilder.UseAutoSyncStructure(true); //自动同步实体结构【开发环境必备】
            }

            var fsql = freeSqlBuilder.Build();


            fsql.Aop.ConfigEntityProperty += ConfigEntityProperty;
            fsql.Aop.CurdBefore += CurdBefore;
            fsql.Aop.AuditValue += AuditValue;
            //fsql.Aop.SyncStructureAfter += SyncStructureAfter;

            DataFilterAsync(fsql);

            return fsql;
        }

        private void DataFilterAsync(IFreeSql fsql) {

            fsql.GlobalFilter.Apply<IBaseSoftDelete>("SoftDelete", a => a.IsDeleted == false);

            //var cacheKey = string.Format(CacheKeyTemplate.PermissionGroupList, _authUser.PermissionId);
            //List<string> groupIds = new List<string>();
            //if (await _cache.ExistsAsync(cacheKey))
            //{
            //    var data = await _cache.GetAsync<List<SysGroupEntity>>(cacheKey);

            //    foreach (var item in data) {
            //        groupIds.Add(item.Id);
            //    }
            //}
            //fsql.GlobalFilter.Apply<IBaseGroup>("Group", a => groupIds.Contains(a.GroupId));
        }

        private void ConfigEntityProperty(object s, ConfigEntityPropertyEventArgs e) {

            
    

            // 处理排序字段自动取最大值插入
            if (e.Property.GetCustomAttributes(typeof(MaxValueAttribute)).Any()) {

                string tableName=e.EntityType.Name;
                var entityTypeAttr = (e.EntityType.GetCustomAttribute(typeof(TableAttribute)) as FreeSql.DataAnnotations.TableAttribute);
                if (entityTypeAttr?.Name != "") tableName = entityTypeAttr.Name;

                string fieldName= e.Property.Name;
                var PropertyAttr = (e.Property.GetCustomAttribute(typeof(ColumnAttribute)) as FreeSql.DataAnnotations.ColumnAttribute);
                if (PropertyAttr?.Name != "") fieldName = PropertyAttr.Name;

                IFreeSql fsql = s as IFreeSql;
                string insertValueSql = "";

                switch (fsql.Ado.DataType) {
                    case DataType.MySql:
                    case DataType.OdbcMySql:
                        insertValueSql = $"(SELECT a.max_v FROM (SELECT (IFNULL(max({fieldName}),0) + 1) max_v from {tableName}) a)";
                        break;
                    case DataType.SqlServer:
                    case DataType.OdbcSqlServer:
                        insertValueSql = $"(SELECT a.max_v FROM (SELECT (isnull(max({fieldName}),0) + 1) max_v from {tableName}) a)";
                        break;
                    case DataType.Oracle:
                    case DataType.OdbcOracle:
                        insertValueSql = $"(SELECT a.max_v FROM (SELECT (nvl(max({fieldName}),0) + 1) max_v from {tableName}) a)";
                        break;
                    default:
                        insertValueSql = $"(SELECT a.max_v FROM (SELECT (max({fieldName}) + 1) max_v from {tableName}) a)";
                        break;
                }

                e.ModifyResult.InsertValueSql = insertValueSql;

            }
        }

        private void AuditValue(object s, AuditValueEventArgs e) {

            if (e.AuditValueType == AuditValueType.Insert
                && e.Property.Name == "Id"
                && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) != null
                && (e.Value == null || e.Value?.ToString() == "")
                )
            {
                var dataCenterId = _systemConfig.CurrentValue?.DataCenterId ?? 5;
                var workId = _systemConfig.CurrentValue?.WorkId ?? 20;
                //var sf = new Common.IdGenerator.SnowflakeId(dataCenterId, workId);
                //var id = sf.NextId();

                var idWorker = Common.IdGenerator.Snowflake.Instance();
                idWorker.Init(dataCenterId, workId);
                var id = idWorker.NextId();

                e.Value = id.ToString();
            }

            if (_authUser == null || _authUser.Id == "")
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

        /// <summary>
        /// Curd前事件
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void CurdBefore(object s, CurdBeforeEventArgs e) {
            if (_systemConfig.CurrentValue != null && _systemConfig.CurrentValue.WatchCurd)
            {
                Console.WriteLine($"{e.Sql}\r\n");
            }
        }

        private void SyncStructureAfter(object s, SyncStructureAfterEventArgs e) { 


        }





    }
}
