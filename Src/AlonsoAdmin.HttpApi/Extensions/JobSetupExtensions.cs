using AlonsoAdmin.Tasks;
using AlonsoAdmin.Tasks.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Extensions
{
    /// <summary>
    /// 任务调度 启动服务
    /// </summary>
    public static class JobSetupExtensions
    {
        public static void AddJobSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddSingleton<IJobFactory, JobFactory>();
            //Job使用瞬时依赖注入
            services.AddTransient<Job_Test>();

            //统一调度任务注入
            services.AddSingleton<ISchedulerCenter, SchedulerCenterServer>();
        }
    }

}
