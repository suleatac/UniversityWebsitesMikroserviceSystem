using Hangfire;
using Mikroservice.Personel.Application.Services;

namespace Mikroservice.Personel.Api.Jobs
{
    public class PersonelHangfireJob
    {
        public static void ScheduleDailySync()
        {
         
            var yesterday = DateTime.Now.Date.AddDays(-1);
            RecurringJob.AddOrUpdate<PersonelSyncService>(
                "personel-daily-sync",
                x => x.SyncPersonelsAsync(yesterday),
                Cron.Daily(4, 0),
                new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });

            
        }

     
    }
}
