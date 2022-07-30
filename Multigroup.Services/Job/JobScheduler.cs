using Multigroup.Services.Shared;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Services.Job
{
    public class JobScheduler

    {
    
        private static IScheduler scheduler;

        public static void Start()
        {
            System.Diagnostics.Debug.WriteLine("Enabling JobScheduler");
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            ConfigureTriggers();
        }

        private static void ConfigureTriggers()
        {
            System.Diagnostics.Debug.WriteLine("Adding Installment Reminder Trigger");
            IJobDetail InstallmentReminderJob = JobBuilder.Create<InstallmentReminder>().Build();
            IJobDetail AdjudicationControlJob = JobBuilder.Create<AdjudicationControl>().Build();
            IJobDetail ContractStatusJob = JobBuilder.Create<ContractStatus>().Build();
            //IJobDetail GenerateInterestsJob = JobBuilder.Create<GenerateInterests>().Build();




               //.StartNow()
               //.WithSimpleSchedule(s => s
               //.WithIntervalInSeconds(100000)
               //.RepeatForever())

               //.WithDailyTimeIntervalSchedule(s => s
               //.WithIntervalInHours(24)
               //.OnEveryDay()
               //.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(16, 37)))
               //.Build();
            //.Build();
            ITrigger InstallmentReminderTrigger = TriggerBuilder.Create()

           .WithIdentity("InstallmentReminder", "InstallmentReminder")
          .WithDailyTimeIntervalSchedule(s => s
                        .WithIntervalInHours(24)
                        .OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(2, 30)))
           .StartNow()
           .Build();



            ITrigger AdjudicationControlTrigger = TriggerBuilder.Create()

           .WithIdentity("AdjudicationControl", "AdjudicationControl")
           .WithDailyTimeIntervalSchedule(s => s
                        .WithIntervalInHours(24)
                        .OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(6, 30)))
           .StartNow()
           .Build();



            ITrigger ContractStatusTrigger = TriggerBuilder.Create()

           .WithIdentity("ContractStatus", "ContractStatus")
          .WithDailyTimeIntervalSchedule(s => s
                        .WithIntervalInHours(24)
                        .OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(4, 30)))
           .StartNow()
           .Build();




            //ITrigger GenerateInterestsTrigger = TriggerBuilder.Create()

            //.WithIdentity("GenerateInterests", "GenerateInterests")
            //  .StartNow()
            //       .WithSimpleSchedule(s => s
            //       .WithIntervalInHours(24)
            //       .RepeatForever())
            //   .Build();

            //.StartNow()
            //.WithSimpleSchedule(s => s
            //.WithIntervalInSeconds(100000)
            //.RepeatForever())

            scheduler.ScheduleJob(InstallmentReminderJob, InstallmentReminderTrigger);
            scheduler.ScheduleJob(AdjudicationControlJob, AdjudicationControlTrigger);
            scheduler.ScheduleJob(ContractStatusJob, ContractStatusTrigger);
            //scheduler.ScheduleJob(GenerateInterestsJob, GenerateInterestsTrigger);

        }
    }
}
