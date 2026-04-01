using Hangfire.Dashboard;

namespace Mikroservice.Ogrenci.Api.Jobs
{
    public class AllowAll : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
