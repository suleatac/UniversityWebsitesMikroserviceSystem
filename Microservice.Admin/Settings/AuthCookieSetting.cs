namespace Microservice.Admin.Settings
{
    /// <summary>
    /// Kimlik doğrulama cookie'si ve ticket (session) saklama ayarları.
    /// Bu ayarlar ile cookie içeriği (header boyutu) kontrol altına alınır ve
    /// access/refresh token'ları Redis üzerinde saklanır.
    /// </summary>
    public class AuthCookieSetting
    {
        public const string Key = "AuthCookie";

        /// <summary>Kimlik doğrulama cookie'sinin adı.</summary>
        public string CookieName { get; set; } = "MikroserviceAuthWebCookie";

        /// <summary>Cookie/ticket geçerlilik süresi (dakika).</summary>
        public int ExpireMinutes { get; set; } = 60;

        /// <summary>Cookie path değeri.</summary>
        public string CookiePath { get; set; } = "/";

        /// <summary>Cookie SameSite değeri (Lax, Strict, None).</summary>
        public string SameSite { get; set; } = "Lax";

        /// <summary>Cookie SecurePolicy değeri (SameAsRequest, Always, None).</summary>
        public string SecurePolicy { get; set; } = "SameAsRequest";

        /// <summary>Sliding expiration aktif mi? Ticket Redis'te olduğu için önerilir.</summary>
        public bool SlidingExpiration { get; set; } = true;

        /// <summary>Cookie içinde header şişmesini önlemek için parçalama boyutu (byte).</summary>
        public int ChunkSize { get; set; } = 4096;

        /// <summary>ITicketStore (Redis) aktif mi? Kapatılırsa token'lar cookie içine yazılır.</summary>
        public bool EnableTicketStore { get; set; } = true;

        /// <summary>Redis ticket anahtar ön eki.</summary>
        public string TicketKeyPrefix { get; set; } = "auth:ticket:";

        /// <summary>Redis ticket kayıtlarının ekstra güvenlik süresi (dakika).</summary>
        public int TicketBufferMinutes { get; set; } = 10;

        /// <summary>Keycloak istemci kimlik doğrulaması. Kimlik doğrulamayı LDAP yaptığı için yalnızca offline_access gerekir.</summary>
        public string KeycloakScope { get; set; } = "openid offline_access";

        /// <summary>
        /// Rol claim'i olarak okunacak claim tipleri. Keycloak token'ları rolü
        /// varsayılan olarak "realm_access.roles" içinde tutar.
        /// </summary>
        public string[] RoleClaimTypes { get; set; } = new[]
        {
            "realm_access.roles",
            "resource_access.roles",
            "roles",
            "role",
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };

        /// <summary>
        /// Kullanıcı adı olarak kabul edilecek claim tipleri (öncelik sırası ile).
        /// </summary>
        public string[] NameClaimTypes { get; set; } = new[]
        {
            "preferred_username",
            "name",
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
        };
    }
}
