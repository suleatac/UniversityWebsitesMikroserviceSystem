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

        /// <summary>
        /// Kimlik sağlayıcıdan gelen subject claim'i. Token farklı kaynaklardan
        /// üretilebildiği için hem "sub" hem de <see cref="ClaimTypes.NameIdentifier"/>
        /// kontrol edilir.
        /// </summary>
        private string? Subject =>
            User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User?.FindFirst("sub")?.Value;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated == true;

        public Guid UserId
        {
            get
            {
                if (!IsAuthenticated)
                    throw new Exception("User is not authenticated");

                var subject = Subject;

                if (string.IsNullOrEmpty(subject))
                    throw new Exception("UserId (sub claim) not found");

                if (!Guid.TryParse(subject, out var userId))
                    throw new Exception($"UserId (sub claim) is not a valid Guid: {subject}");

                return userId;
            }
        }

        public string KeycloakUserId
        {
            get
            {
                if (!IsAuthenticated)
                    throw new Exception("User is not authenticated");

                var subject = Subject;

                if (string.IsNullOrEmpty(subject))
                    throw new Exception("KeycloakUserId (sub claim) not found");

                return subject;
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

        public bool IsAdmin
        {
            get
            {
                if (!IsAuthenticated)
                    return false;

                return Roles.Contains("Admin", StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}
