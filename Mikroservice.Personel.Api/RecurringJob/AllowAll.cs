using Hangfire.Dashboard;

namespace Mikroservice.Personel.Api.RecurringJob
{
    public class AllowAll : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
