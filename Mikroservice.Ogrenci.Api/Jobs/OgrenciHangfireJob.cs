using Hangfire;
using Mikroservice.Ogrenci.Application.Services;

namespace Mikroservice.Ogrenci.Api.Jobs
{
    public class OgrenciHangfireJob
    {
        public static void ScheduleDailySync()
        {
         
            var yesterday = DateTime.Now.Date.AddDays(-1);
            RecurringJob.AddOrUpdate<OgrenciSyncService>(
                "student-daily-sync",
                x => x.SyncOgrencisAsync(yesterday),
                Cron.Daily(4, 0),
                new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });

            
        }

     
    }
}
