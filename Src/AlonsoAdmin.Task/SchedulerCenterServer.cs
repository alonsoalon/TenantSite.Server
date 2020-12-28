using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Entities.System;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Tasks
{
    public class SchedulerCenterServer : ISchedulerCenter
    {
        private Task<IScheduler> _scheduler;
        private readonly IJobFactory _iocjobFactory;

        public SchedulerCenterServer(IJobFactory jobFactory)
        {
            _iocjobFactory = jobFactory;
            _scheduler = GetSchedulerAsync();
        }

        private Task<IScheduler> GetSchedulerAsync()
        {
            if (_scheduler != null)
                return this._scheduler;
            else
            {
                // 从Factory中获取Scheduler实例
                NameValueCollection collection = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" },
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(collection);
                return _scheduler = factory.GetScheduler();
            }
        }

        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<IResponseEntity> StartScheduleAsync()
        {
            try
            {
                this._scheduler.Result.JobFactory = this._iocjobFactory;
                if (!this._scheduler.Result.IsStarted)
                {
                    //等待任务运行完成
                    await this._scheduler.Result.Start();
                    await Console.Out.WriteLineAsync("任务调度开启！");

                    return ResponseEntity.Ok("任务调度开启成功");
                }
                else
                {
                    return ResponseEntity.Error("任务调度已经开启");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<IResponseEntity> StopScheduleAsync()
        {
            try
            {
                if (!this._scheduler.Result.IsShutdown)
                {
                    //等待任务运行完成
                    await this._scheduler.Result.Shutdown();
                    await Console.Out.WriteLineAsync("任务调度停止！");
                    return ResponseEntity.Ok($"任务调度停止成功");
                }
                else
                {
                    return ResponseEntity.Error($"任务调度已经停止");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// 添加一个计划任务（映射程序集指定IJob实现类）
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        public async Task<IResponseEntity> AddScheduleJobAsync(SysTaskQzEntity tasksQz)
        {
            if (tasksQz == null)
            {
                await Console.Out.WriteLineAsync("计划任务不存在！");
                return ResponseEntity.Error("计划任务不存在！");
            }

            try
            {
                JobKey jobKey = new JobKey(tasksQz.Id.ToString(), tasksQz.JobGroup);
                if (await _scheduler.Result.CheckExists(jobKey))
                {
                    await Console.Out.WriteLineAsync($"该任务计划已经在执行:【{tasksQz.Name}】,请勿重复启动！");
                    return ResponseEntity.Error($"该任务计划已经在执行:【{tasksQz.Name}】,请勿重复启动！");
                }
                #region 设置开始时间和结束时间

                if (tasksQz.BeginTime == null)
                {
                    tasksQz.BeginTime = DateTime.Now;
                }
                DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(tasksQz.BeginTime, 1);//设置开始时间
                if (tasksQz.EndTime == null)
                {
                    tasksQz.EndTime = DateTime.MaxValue.AddDays(-1);
                }
                DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(tasksQz.EndTime, 1);//设置暂停时间

                #endregion

                #region 通过反射获取程序集类型和类   

                Assembly assembly = Assembly.Load(new AssemblyName(tasksQz.AssemblyName));
                Type jobType = assembly.GetType(tasksQz.AssemblyName + ".Jobs." + tasksQz.ClassName);

                #endregion

                //传入反射出来的执行程序集
                IJobDetail job = new JobDetailImpl(tasksQz.Id.ToString(), tasksQz.JobGroup, jobType);
                job.JobDataMap.Add("JobParams", tasksQz.JobParams);
                if (tasksQz.ConnectionParam.IsNull())
                {
                    await Console.Out.WriteLineAsync($"任务依赖数据库为空！");
                    return ResponseEntity.Error($"任务依赖数据库为空！");
                }
                job.JobDataMap.Add("ConnectionParam", tasksQz.ConnectionParam);

                ITrigger trigger;

                #region 泛型传递
                //IJobDetail job1 = JobBuilder.Create<T>()
                //    .WithIdentity(sysSchedule.Name, sysSchedule.JobGroup)
                //    .Build();
                #endregion


                if (tasksQz.Cron != null && CronExpression.IsValidExpression(tasksQz.Cron) && tasksQz.TriggerType > 0)
                {
                    trigger = CreateCronTrigger(tasksQz);
                }
                else
                {
                    trigger = CreateSimpleTrigger(tasksQz);
                }

                //判断任务调度是否开启
                if (!_scheduler.Result.IsStarted)
                {
                    await StartScheduleAsync();
                }

                // 告诉Quartz使用我们的触发器来安排作业
                await _scheduler.Result.ScheduleJob(job, trigger);
                await Console.Out.WriteLineAsync($"启动任务:【{tasksQz.Name}】成功");
                return ResponseEntity.Ok($"启动任务:【{tasksQz.Name}】成功");
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        /// <summary>
        /// 暂停一个指定的计划任务
        /// </summary>
        /// <returns></returns>
        public async Task<IResponseEntity> PauseScheduleJobAsync(SysTaskQzEntity sysSchedule)
        {
            try
            {
                JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                if (!await _scheduler.Result.CheckExists(jobKey))
                {
                    await Console.Out.WriteLineAsync($"未找到要暂停的任务:【{sysSchedule.Name}】");
                    return ResponseEntity.Error($"未找到要暂停的任务:【{sysSchedule.Name}】");
                }
                else
                {
                    await _scheduler.Result.PauseJob(jobKey);
                    await Console.Out.WriteLineAsync($"暂停任务:【{sysSchedule.Name}】成功");
                    return ResponseEntity.Ok($"暂停任务:【{sysSchedule.Name}】成功");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        public async Task<IResponseEntity> StopScheduleJobAsync(SysTaskQzEntity sysSchedule)
        {
            try
            {
                JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                if (!await _scheduler.Result.CheckExists(jobKey))
                {
                    await Console.Out.WriteLineAsync($"未找到要停止的任务:【{sysSchedule.Name}】");
                    return ResponseEntity.Error($"未找到要停止的任务:【{sysSchedule.Name}】");
                }
                else
                {
                    await _scheduler.Result.DeleteJob(jobKey);
                    await Console.Out.WriteLineAsync($"停止任务:【{sysSchedule.Name}】成功");
                    return ResponseEntity.Ok($"停止任务:【{sysSchedule.Name}】成功");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 恢复指定的计划任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        public async Task<IResponseEntity> ResumeJob(SysTaskQzEntity sysSchedule)
        {
            try
            {
                JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                if (!await _scheduler.Result.CheckExists(jobKey))
                {
                    return ResponseEntity.Error($"未找到要重新的任务:【{sysSchedule.Name}】,请先选择添加计划！");
                }
                await this._scheduler.Result.ResumeJob(jobKey);
                return ResponseEntity.Ok($"恢复计划任务:【{sysSchedule.Name}】成功");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取计划任务运行状态
        /// </summary>
        /// <param name="key"></param>
        /// <param name="jobGroup"></param>
        /// <returns></returns>
        public string GetJobRunTriggerState(string key, string jobGroup)
        {
            TriggerKey triggerKey = new TriggerKey(key, jobGroup);
            string resStr = string.Empty;
            if (_scheduler.Result.CheckExists(triggerKey).Result)
            {
                switch (_scheduler.Result.GetTriggerState(triggerKey).Result)
                {
                    case TriggerState.Normal:
                        resStr = "正常";
                        break;
                    case TriggerState.Paused:
                        resStr = "暂停";
                        break;
                    case TriggerState.Complete:
                        resStr = "完成";
                        break;
                    case TriggerState.Error:
                        resStr = "错误";
                        break;
                    case TriggerState.Blocked:
                        resStr = "阻塞";
                        break;
                    case TriggerState.None:
                        resStr = "不存在";
                        break;
                }
            }
            else
            {
                resStr = "不存在";
            }
            return resStr;
        }

        /// <summary>
        /// 获取任务下一次执行时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="jobGroup"></param>
        /// <returns></returns>
        public DateTime? GetJobNextFireTime(string key, string jobGroup)
        {
            JobKey jobKey = new JobKey(key, jobGroup);
            TriggerKey triggerKey = new TriggerKey(key, jobGroup);
            DateTime? _dateTime = null;
            if (_scheduler.Result.CheckExists(triggerKey).Result)
            {
                var time = _scheduler.Result.GetTrigger(triggerKey).Result.GetNextFireTimeUtc();
                if (time.HasValue)
                {
                    _dateTime = time.Value.UtcDateTime.ToLocalTime();
                }
            }
            return _dateTime;
        }

        #region 创建触发器帮助方法

        /// <summary>
        /// 创建SimpleTrigger触发器（简单触发器）
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <param name="starRunTime"></param>
        /// <param name="endRunTime"></param>
        /// <returns></returns>
        private ITrigger CreateSimpleTrigger(SysTaskQzEntity sysSchedule)
        {
            if (sysSchedule.RunTimes > 0)
            {
                ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(sysSchedule.Id.ToString(), sysSchedule.JobGroup)
                .StartAt(sysSchedule.BeginTime.Value)
                .EndAt(sysSchedule.EndTime.Value)
                .WithSimpleSchedule(x =>
                x.WithIntervalInSeconds(sysSchedule.IntervalSecond)
                .WithRepeatCount(sysSchedule.RunTimes)).ForJob(sysSchedule.Id.ToString(), sysSchedule.JobGroup).Build();
                return trigger;
            }
            else
            {
                ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(sysSchedule.Id.ToString(), sysSchedule.JobGroup)
                .StartAt(sysSchedule.BeginTime.Value)
                .EndAt(sysSchedule.EndTime.Value)
                .WithSimpleSchedule(x =>
                x.WithIntervalInSeconds(sysSchedule.IntervalSecond)
                .RepeatForever()).ForJob(sysSchedule.Id.ToString(), sysSchedule.JobGroup).Build();
                return trigger;
            }

        }
        /// <summary>
        /// 创建类型Cron的触发器
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private ITrigger CreateCronTrigger(SysTaskQzEntity sysSchedule)
        {
            // 作业触发器
            return TriggerBuilder.Create()
                   .WithIdentity(sysSchedule.Id.ToString(), sysSchedule.JobGroup)
                   .StartAt(sysSchedule.BeginTime.Value)//开始时间
                   .EndAt(sysSchedule.EndTime.Value)//结束数据
                   .WithCronSchedule(sysSchedule.Cron)//指定cron表达式
                   .ForJob(sysSchedule.Id.ToString(), sysSchedule.JobGroup)//作业名称
                   .Build();
        }
        #endregion


    }
}
