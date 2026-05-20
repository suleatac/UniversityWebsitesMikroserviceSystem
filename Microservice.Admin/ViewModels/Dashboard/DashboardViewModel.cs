using Microservice.Admin.ViewModels.AuditLog;
using Microservice.Admin.ViewModels.YonetimDuyuru;

namespace Microservice.Admin.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int HaberCount { get; set; }
        public int DuyuruCount { get; set; }
        public int EtkinlikCount { get; set; }
        public List<YonetimDuyuruVm> RecentYonetimDuyurular { get; set; } = new();
        public int UnreadYonetimDuyuruCount { get; set; }

        // AuditLog fields
        public int TotalAuditLogCount { get; set; }
        public List<AuditLogDailyStatVm> DailyAuditLogStats { get; set; } = new();
        public List<AuditLogVm> RecentAuditLogs { get; set; } = new();
    }
}
