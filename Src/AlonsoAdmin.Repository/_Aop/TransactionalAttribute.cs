using AspectCore.DynamicProxy;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlonsoAdmin.Repository
{
    /// <summary>
    /// 事务处理，目前这个事务处理没调试成功，弃用状态，
    /// </summary>
    [Obsolete]
    public class TransactionalAttribute : AbstractInterceptorAttribute
    {
        public Propagation Propagation { get; set; } = Propagation.Requierd;
        public IsolationLevel? IsolationLevel { get; set; }

        public string DbKey { get; set; }

        private IUnitOfWork _uow;

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
               // Func<string, UowManager>

                //var declaringType = context.ServiceMethod.DeclaringType;
                //var uowManager = context.ServiceProvider.GetService(typeof(Func<string, UowManager>)) as Func<string, UowManager>;              
                //var uow = uowManager(DbKey);


                var dbFactory = context.ServiceProvider.GetService(typeof(IMultiTenantDbFactory)) as IMultiTenantDbFactory;

                //_uow = dbFactory.Db(DbKey).CreateUnitOfWork();
                var uow = new UnitOfWorkManager(dbFactory.Db(DbKey));

                await OnBefore(uow);
                await next(context);
                await OnAfter(null);
            }
            catch (Exception ex)
            {
                await OnAfter(ex);
                throw ex;
            }
            finally
            {
                //Console.WriteLine("After service call");
            }
        }

        Task OnBefore(UnitOfWorkManager uowm)
        {
            _uow = uowm.Begin(this.Propagation, this.IsolationLevel);
            return Task.FromResult(false);
        }
        Task OnAfter(Exception ex)
        {
            try
            {
                if (ex == null) _uow.Commit();
                else _uow.Rollback();
            }
            finally
            {
                _uow.Dispose();
            }
            return Task.FromResult(false);
        }
    }
}
