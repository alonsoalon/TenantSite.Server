using AlonsoAdmin.Repository;
using Microsoft.AspNetCore.Hosting;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Tasks.Jobs
{
   public  class Job_Test: JobBase, IJob
    {
        //注入服务
        public Job_Test()
        {
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobKey = context.JobDetail.Key;
            var jobId = jobKey.Name;
            var executeLog = await ExecuteJob(context, async () => await Run(context));
        }


        /// <summary>
        /// 执行具体的工作
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jobid"></param>
        /// <returns></returns>
        public async Task Run(IJobExecutionContext context)
        {
            JobDataMap data = context.JobDetail.JobDataMap;
            string connParam = data.GetString("ConnectionParam");
            //通过数据库中配置的数据库信息获取
            IFreeSql freeSql = GetDb(connParam);

        }
    }
}
