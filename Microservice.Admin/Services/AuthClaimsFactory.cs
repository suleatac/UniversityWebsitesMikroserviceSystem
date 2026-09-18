using Microservice.Admin.Settings;
using System.Security.Claims;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    /// <summary>
    /// Token'dan gelen ham claim listesini, uygulamanın gerçekten ihtiyaç duyduğu
    /// minimum claim setine indirger.
    ///
    /// Neden gerekli?
    /// - Keycloak access token'ları içinde jwt'nin kendisi hariç <c>jti</c>, <c>iat</c>,
    ///   <c>exp</c>, <c>nbf</c>, <c>iss</c>, <c>aud</c>, <c>azp</c>, <c>sid</c>, <c>scope</c>,
    ///   <c>typ</c>, <c>session_state</c> gibi cookie ihtiyacı olmayan onlarca claim bulunur.
    /// - <c>realm_access.roles</c> ve <c>resource_access.*.roles</c> tek bir string claim
    ///   içinde JSON olarak geldiği için hem büyük hem de ClaimTypes.Role ile eşleşmez.
    ///   Bu yüzden JSON parse edilip düz role claim'lerine dönüştürülür.
    /// </summary>
    public static class AuthClaimsFactory
    {
        /// <summary>Kimlik sağlayıcı yalnızca "sub" claim'i içermediğinde kullanılan varsayılan claim tipi.</summary>
        private const string FallbackSubjectClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        /// <summary>
        /// Token claim'lerini azaltır, rol ve isim claim'lerini normalize eder.
        /// Yalnızca <c>sub</c>, isim ve roller cookie'ye yazılır; token'lar cookie'ye
        /// yazılmaz (RedisTicketStore kullanılır).
        /// </summary>
        public static List<Claim> Build(
            IEnumerable<Claim>? tokenClaims,
            AuthCookieSetting settings,
            string? fallbackSubject = null)
        {
            ArgumentNullException.ThrowIfNull(settings);

            var source = tokenClaims?.ToList() ?? new List<Claim>();
            var result = new List<Claim>();
            var added = new HashSet<string>(StringComparer.Ordinal);

            // 1) sub -> ClaimTypes.NameIdentifier (+ UserId/Username okuyan kodu bozmamak için düz "sub")
            var subject = FirstNonEmpty(source, "sub", "user_id", "userId", FallbackSubjectClaimType) ?? fallbackSubject;

            if (!string.IsNullOrWhiteSpace(subject))
            {
                Add(result, added, ClaimTypes.NameIdentifier, subject);
                Add(result, added, "sub", subject);
            }

            // 2) Kullanıcı adı -> ClaimTypes.Name
            var userName = FirstNonEmpty(source, settings.NameClaimTypes)
                           ?? FirstNonEmpty(source, "preferred_username", "name", "given_name");

            if (!string.IsNullOrWhiteSpace(userName))
            {
                Add(result, added, ClaimTypes.Name, userName);
            }

            // 3) Roller -> ClaimTypes.Role (realm_access / resource_access JSON'ları parse edilir)
            foreach (var role in ExtractRoles(source, settings))
            {
                Add(result, added, ClaimTypes.Role, role);
            }

            return result;
        }

        /// <summary>
        /// Token claim'lerinden rol listesini çıkarır. <c>realm_access.roles</c> gibi
        /// JSON içeren claim'ler parse edilir, <c>roles</c>/<c>role</c> gibi koleksiyonlar açılır.
        /// </summary>
        public static IReadOnlyCollection<string> ExtractRoles(
            IEnumerable<Claim>? tokenClaims,
            AuthCookieSetting settings)
        {
            var roles = new List<string>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (tokenClaims == null)
            {
                return roles;
            }

            var claims = tokenClaims.ToList();
            var roleClaimTypes = settings.RoleClaimTypes ?? Array.Empty<string>();

            foreach (var claim in claims)
            {
                if (!IsRoleClaim(claim, roleClaimTypes))
                {
                    continue;
                }

                AddRoleValues(claim.Value, roles, seen);
            }

            // Rol claim'i hiç yoksa ve token realm_access nesnesi içeriyorsa doğrudan oradan dene
            if (roles.Count == 0)
            {
                var realmAccess = claims.FirstOrDefault(c =>
                    string.Equals(c.Type, "realm_access", StringComparison.Ordinal))?.Value;

                if (!string.IsNullOrWhiteSpace(realmAccess))
                {
                    AddRoleValues(realmAccess, roles, seen);
                }
            }

            return roles;
        }

        /// <summary>
        /// Verilen claim tipi rol claim'i mi? (hem yapılandırılmış tipler hem JSON içerikli kaynaklar)
        /// </summary>
        public static bool IsRoleClaim(Claim claim, IReadOnlyCollection<string> roleClaimTypes)
        {
            if (roleClaimTypes.Contains(claim.Type, StringComparer.Ordinal))
            {
                return true;
            }

            return claim.Type.EndsWith(".roles", StringComparison.Ordinal)
                   || string.Equals(claim.Type, "realm_access", StringComparison.Ordinal)
                   || string.Equals(claim.Type, "resource_access", StringComparison.Ordinal);
        }

        /// <summary>
        /// Bir claim değerini rol listesine ekler. Değer JSON nesnesi/dizisi ise açılır.
        /// </summary>
        private static void AddRoleValues(string? rawValue, List<string> roles, HashSet<string> seen)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return;
            }

            var value = rawValue.Trim();

            if (value.StartsWith("{", StringComparison.Ordinal) || value.StartsWith("[", StringComparison.Ordinal))
            {
                foreach (var nested in FlattenJsonValues(value))
                {
                    AddUnique(nested, roles, seen);
                }

                return;
            }

            AddUnique(value, roles, seen);
        }

        /// <summary>
        /// JSON içeriğindeki tüm string değerleri (özellikle "roles" dizileri) düzleştirir.
        /// </summary>
        private static IEnumerable<string> FlattenJsonValues(string json)
        {
            JsonDocument? document = null;
            JsonElement root = default;

            try
            {
                document = JsonDocument.Parse(json);
                root = document.RootElement;
            }
            catch (JsonException)
            {
                // JSON değilse olduğu gibi bırak
            }

            if (document == null)
            {
                yield return json.Trim('"');
                yield break;
            }

            try
            {
                foreach (var value in EnumerateStrings(root))
                {
                    yield return value;
                }
            }
            finally
            {
                document.Dispose();
            }
        }

        private static IEnumerable<string> EnumerateStrings(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.String:
                    var stringValue = element.GetString();

                    if (!string.IsNullOrWhiteSpace(stringValue) && !IsMetadataKey(stringValue))
                    {
                        yield return stringValue;
                    }

                    break;

                case JsonValueKind.Array:
                    foreach (var item in element.EnumerateArray())
                    {
                        foreach (var value in EnumerateStrings(item))
                        {
                            yield return value;
                        }
                    }

                    break;

                case JsonValueKind.Object:
                    foreach (var property in element.EnumerateObject())
                    {
                        // "roles": ["Admin"] gibi diziler içeriğiyle, "Admin": {...} gibi
                        // anahtarla rol verilen yapıların her ikisini de destekle.
                        if (LooksLikeRoleName(property.Name))
                        {
                            yield return property.Name;
                        }

                        foreach (var value in EnumerateStrings(property.Value))
                        {
                            yield return value;
                        }
                    }

                    break;
            }
        }

        private static bool LooksLikeRoleName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            return !string.Equals(name, "roles", StringComparison.OrdinalIgnoreCase)
                   && !string.Equals(name, "role", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsMetadataKey(string value) =>
            string.Equals(value, "roles", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "role", StringComparison.OrdinalIgnoreCase);

        private static string? FirstNonEmpty(IEnumerable<Claim> claims, params string[] claimTypes)
        {
            foreach (var claimType in claimTypes)
            {
                var value = claims
                    .FirstOrDefault(c => string.Equals(c.Type, claimType, StringComparison.Ordinal))
                    ?.Value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }

            return null;
        }

        private static void Add(List<Claim> claims, HashSet<string> added, string type, string value)
        {
            if (!added.Add($"{type}|{value}"))
            {
                return;
            }

            claims.Add(new Claim(type, value));
        }

        private static void AddUnique(string value, List<string> roles, HashSet<string> seen)
        {
            var role = value.Trim();

            if (role.Length == 0 || !seen.Add(role))
            {
                return;
            }

            roles.Add(role);
        }
    }
}
