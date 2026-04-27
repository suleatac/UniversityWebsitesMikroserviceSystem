using Microservice.Admin.Services.Interfaces;
using System.Security.Claims;

namespace Microservice.Admin.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal User =>
            _httpContextAccessor.HttpContext!.User;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated == true;

        public Guid UserId
        {
            get
            {
                if (!IsAuthenticated)
                    throw new Exception("User is not authenticated");

                var sub = User?.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(sub))
                    throw new Exception("UserId (sub claim) not found");

                return Guid.Parse(sub);
            }
        }

        public string Username
        {
            get
            {
                if (!IsAuthenticated)
                    throw new Exception("User is not authenticated");

                return User?.Identity?.Name ?? throw new Exception("Username not found");
            }
        }

        public List<string> Roles
        {
            //http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name
            get
            {
                if (!IsAuthenticated)
                    throw new Exception("User is not authenticated");

                return User?
                    .FindAll(ClaimTypes.Role)
                    .Select(x => x.Value)
                    .ToList()
                    ?? new List<string>();
            }
        }
    }
}
