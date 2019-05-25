using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;


namespace DataImporting.Classes.Task
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler defaultScheduler = StdSchedulerFactory.GetDefaultScheduler();
            defaultScheduler.Start();
            IJobDetail jobDetail = JobBuilder.Create<DataJob>().WithIdentity("job").WithDescription("job")
                .Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("triggerdata", "groupdata").StartNow()
                .WithSimpleSchedule(delegate (SimpleScheduleBuilder x)
                {
                    x.WithIntervalInHours(24).RepeatForever();
                })
                .Build();
            defaultScheduler.ScheduleJob(jobDetail, trigger);
        }
    }

}