using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.HttpApi.Init.Entities;
using AlonsoAdmin.HttpApi.Init.Services;
using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlonsoAdmin.HttpApi.Init.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToolController : ControllerBase
    {
        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseEntity TestDbConnect(DbConnectItem req)
        {
            try
            {
                var dbType = (FreeSql.DataType)Enum.Parse(typeof(FreeSql.DataType), req.DbType);
                var connStr = req.ConnectionString;
                IFreeSql fsql = new FreeSql.FreeSqlBuilder()
                            .UseConnectionString(dbType, connStr)
                            .Build();

                // 这儿验证 连接是否成功
                //var a = fsql.DbFirst.GetDatabases();
                //Fsql.Ado.MasterPool.Get().Value
                DbConnection dbConnection = fsql.Ado.MasterPool.Get().Value;

                fsql.Dispose();

                return ResponseEntity.Ok("连接成功");
            }
            catch (Exception ex)
            {
                return ResponseEntity.Error(ex.Message);
            }

        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> CreateDb(DbCreateItem req)
        {
            try
            {
                var dbType = (FreeSql.DataType)Enum.Parse(typeof(FreeSql.DataType), req.DbType);
                var connStr = req.ConnectionString;
                IFreeSql fsql = new FreeSql.FreeSqlBuilder()
                            .UseConnectionString(dbType, connStr)
                            .Build();

                Console.WriteLine("\r\n开始建库");
                await fsql.Ado.ExecuteNonQueryAsync(req.CreateDbCommand);
                fsql.Dispose();
                Console.WriteLine("建库完成\r\n");
                return ResponseEntity.Ok("创建成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"建库失败.\n{ex.Message}\r\n");
                return ResponseEntity.Error(ex.Message);
            }

        }

        /// <summary>
        /// 生成种子数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseEntity GenerateSeedData(DbConnectItem req)
        {
            var dbType = (FreeSql.DataType)Enum.Parse(typeof(FreeSql.DataType), req.DbType);
            var connStr = req.ConnectionString;
            IFreeSql fsql = new FreeSqlBuilder()
                        .UseConnectionString(dbType, connStr)
                        .Build();
            var seedData = new SeedData();
            seedData.GenerateSeedData(fsql);
            return ResponseEntity.Ok("种子数据生成成功");
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> InitDb(DbConnectItem req)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");
            sb.Append("<li>创建数据连接对象 开始</li>");

            var dbType = (FreeSql.DataType)Enum.Parse(typeof(FreeSql.DataType), req.DbType);
            var connStr = req.ConnectionString;
            IFreeSql fsql = new FreeSqlBuilder()
                        .UseConnectionString(dbType, connStr)
                        .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
                        .Build();
            DbConnection dbConnection = fsql.Ado.MasterPool.Get().Value; // 这儿验证 连接是否成功，这句代码可以不要，如果连接字符不变正确，为了提早发现（报异常）
            fsql.Aop.AuditValue += SyncDataAuditValue;

            sb.Append("<li>创建数据连接对象 结束</li>");

            sb.Append("<li>创建数据库结构及初始化数据 开始</li>");
            using (var uow = fsql.CreateUnitOfWork())
            using (var tran = uow.GetOrBeginTransaction())
            {
                SeedDataEntity data = (new SeedData()).GetSeedData();
                sb.Append("<ul>");
                await InitDtData(fsql, data.SysApiEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysConditionEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysDictionaryDetailEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysDictionaryEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysGroupEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysPermissionEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysResourceEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysRoleEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysRPermissionConditionEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysRPermissionGroupEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysRPermissionRoleEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysRResourceApiEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysRRoleResourceEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysSettingEntities.ToArray(), tran, sb);
                await InitDtData(fsql, data.SysUserEntities.ToArray(), tran, sb);
                sb.Append("</ul>");
                uow.Commit();
            }

            sb.Append("<li>创建数据库结构及初始化数据 结束</li>");
            sb.Append("</ul>");

            fsql.Dispose();

            return ResponseEntity.Ok("初始化成功", msg: sb.ToString());


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

        private async Task<StringBuilder> InitDtData<T>(
            IFreeSql db,
            T[] data,
            DbTransaction tran,
            StringBuilder sb
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
                            //var insrtSql = insert.AppendData(data).InsertIdentity().ToSql();
                            //var sql = $"SET IDENTITY_INSERT {tableName} ON\n {insrtSql} \nSET IDENTITY_INSERT {tableName} OFF";
                            //await db.Ado.ExecuteNonQueryAsync(sql);
                            await insert.AppendData(data).InsertIdentity().ExecuteAffrowsAsync();
                        }
                        else
                        {
                            await insert.AppendData(data).InsertIdentity().ExecuteAffrowsAsync();
                        }
                        sb.Append($"<li>table: {tableName} 生成种子数据 成功</li>");
                        Console.WriteLine($"table: {tableName} 生成种子数据 succeed");
                    }
                    else
                    {
                        sb.Append($"<li>table: {tableName} 无种子数据</li>");
                        Console.WriteLine($"table: {tableName} 无种子数据");
                    }
                }
                else
                {
                    sb.Append($"<li>table: {tableName} 数据已存在 如果需重新初始化，先清空表</li>");
                    Console.WriteLine($"table: {tableName} 数据已存在 如果需重新初始化，先清空表");
                }
                return sb;
            }
            catch (Exception ex)
            {
                sb.Append($"<li>table: {tableName} 同步数据失败 \n{ex.Message}</li>");
                Console.WriteLine($"table: {tableName} 同步数据失败 \n{ex.Message}");
                return sb;
            }

        }
    }
}