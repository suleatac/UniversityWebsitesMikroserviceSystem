using Hangfire.Dashboard;

namespace Mikroservice.Personel.Api.Jobs
{
    public class AllowAll : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
