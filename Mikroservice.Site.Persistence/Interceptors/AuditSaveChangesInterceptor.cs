using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Mikroservice.Site.Persistence.Interceptors
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            var context = eventData.Context;

            var userId = AuditLogContext.UserId;
            var username = AuditLogContext.Username;

            // audit logic burada
            return base.SavingChanges(eventData, result);
        }
    }
}
