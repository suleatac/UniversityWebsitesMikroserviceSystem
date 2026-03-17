using Hangfire.Dashboard;

namespace Mikroservice.Ogrenci.Api.RecurringJob
{
    public class AllowAll : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
