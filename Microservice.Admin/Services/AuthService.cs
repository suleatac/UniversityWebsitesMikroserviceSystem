using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.Settings;
using Microservice.Admin.ViewModels.SignIn;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System.Security.Claims;

namespace Microservice.Admin.Services
{
    public class AuthService : IAuthService
    {
        // LDAP kullanıcıları için Keycloak sub claim'i her zaman GUID olmayabilir.
        // Bu durumda deterministik bir GUID üretilir (v5 benzeri).
        private static readonly Guid UserIdNamespace = new("6ba7b812-9dad-11d1-80b4-00c04fd430c8");

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;
        private readonly LdapSetting _ldapSettings;
        private readonly AuthCookieSetting _authCookieSettings;

        public AuthService(
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService,
            ILogger<AuthService> logger,
            IOptions<LdapSetting> ldapSettings,
            IOptions<AuthCookieSetting> authCookieSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _logger = logger;
            _ldapSettings = ldapSettings.Value;
            _authCookieSettings = authCookieSettings.Value;
        }

        public async Task<ServiceResult> AuthenticateAsync(SignInVm signInViewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(signInViewModel.Username) ||
                    string.IsNullOrWhiteSpace(signInViewModel.Password))
                {
                    _logger.LogWarning("Username veya password boş geldi.");

                    return ServiceResult.Error(
                        "Validation Error",
                        "Kullanıcı adı ve şifre zorunludur."
                    );
                }



                // LDAP doğrulama
                //var ldapResult = true;
                var ldapResult = await LdapAuthenticationAsync(signInViewModel.Username, signInViewModel.Password);

                if (!ldapResult)
                {
                    _logger.LogWarning(
                        "LDAP authentication başarısız. Username: {Username}",
                        signInViewModel.Username);

                    return ServiceResult.Error(
                        "LDAP Authentication Error",
                        "Kullanıcı adı veya şifre hatalı."
                    );
                }

                _logger.LogInformation(
                    "LDAP authentication başarılı. Username: {Username}",
                    signInViewModel.Username);

                // Keycloak token al
                var tokenResponse =
                    await _tokenService.GetPasswordAccessToken(signInViewModel);

                if (tokenResponse?.Data == null ||
                    string.IsNullOrEmpty(tokenResponse.Data.AccessToken))
                {
                    _logger.LogWarning(
                        "Keycloak token alınamadı. Username: {Username}",
                        signInViewModel.Username);

                    return ServiceResult.Error(
                        tokenResponse?.Data?.Error ?? "Auth Error",
                        tokenResponse?.Data?.ErrorDescription ?? "Login failed"
                    );
                }

                // Claimleri çıkar ve minimum sete indir.
                // Access token içindeki jti/iat/nbf/iss/aud/sid gibi kullanılmayan
                // claim'ler cookie'ye taşınmaz. Token'ın kendisi de cookie'ye yazılmaz.
                var userClaims = _tokenService
                    .ExtractClaims(tokenResponse.Data.AccessToken);

                var filteredClaims = AuthClaimsFactory.Build(userClaims, _authCookieSettings);

                // sub claim'i yoksa veya GUID değilse UserId doğrulaması kırılmasın diye tamamla.
                EnsureSubjectClaim(filteredClaims, signInViewModel.Username);

                var claimsIdentity = new ClaimsIdentity(
                    filteredClaims,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name,
                    ClaimTypes.Role);

                var claimsPrincipal =
                    new ClaimsPrincipal(claimsIdentity);

                var authenticationProperties =
                    _tokenService.CreateAuthenticationProperties(
                        tokenResponse.Data);

                // Cookie login
                await _httpContextAccessor.HttpContext!
                    .SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        claimsPrincipal,
                        authenticationProperties);

                _logger.LogInformation(
                    "Kullanıcı başarıyla login oldu. Username: {Username}",
                    signInViewModel.Username);

                return ServiceResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "AuthenticateAsync sırasında hata oluştu.");

                return ServiceResult.Error(
                    "System Error",
                    "Beklenmeyen bir hata oluştu.");
            }
        }

        /// <summary>
        /// sub claim'i yoksa veya Guid'e çevrilemiyorsa, kullanıcı adından deterministik bir
        /// Guid üretip <c>sub</c> ve <see cref="ClaimTypes.NameIdentifier"/> claim'lerine ekler.
        /// </summary>
        private static void EnsureSubjectClaim(List<Claim> claims, string? username)
        {
            var subject = claims
                .FirstOrDefault(c => string.Equals(c.Type, ClaimTypes.NameIdentifier, StringComparison.Ordinal))
                ?.Value
                ?? claims.FirstOrDefault(c => string.Equals(c.Type, "sub", StringComparison.Ordinal))?.Value;

            if (Guid.TryParse(subject, out _))
            {
                return;
            }

            var normalizedUserName = string.IsNullOrWhiteSpace(username) ? "unknown" : username.Trim();
            var generated = CreateDeterministicGuid(UserIdNamespace, normalizedUserName.ToLowerInvariant());

            claims.RemoveAll(c =>
                string.Equals(c.Type, ClaimTypes.NameIdentifier, StringComparison.Ordinal) ||
                string.Equals(c.Type, "sub", StringComparison.Ordinal));

            claims.Add(new Claim(ClaimTypes.NameIdentifier, generated.ToString()));
            claims.Add(new Claim("sub", generated.ToString()));
        }

        /// <summary>
        /// RFC 4122 v5 benzeri (isim tabanlı) deterministik GUID üretir.
        /// </summary>
        private static Guid CreateDeterministicGuid(Guid namespaceId, string name)
        {
            var namespaceBytes = namespaceId.ToByteArray();
            SwapByteOrder(namespaceBytes);

            var nameBytes = System.Text.Encoding.UTF8.GetBytes(name);
            var data = new byte[namespaceBytes.Length + nameBytes.Length];

            Buffer.BlockCopy(namespaceBytes, 0, data, 0, namespaceBytes.Length);
            Buffer.BlockCopy(nameBytes, 0, data, namespaceBytes.Length, nameBytes.Length);

            var hash = System.Security.Cryptography.SHA1.HashData(data);
            var newGuid = new byte[16];
            Array.Copy(hash, newGuid, 16);

            newGuid[6] = (byte)((newGuid[6] & 0x0F) | 0x50); // version 5
            newGuid[8] = (byte)((newGuid[8] & 0x3F) | 0x80); // variant

            SwapByteOrder(newGuid);
            return new Guid(newGuid);
        }

        private static void SwapByteOrder(byte[] guid)
        {
            // Guid.ToByteArray(): [0-3] int (little endian), [4-5] short (little endian),
            // [6-7] short (little endian), [8-15] big endian
            (guid[0], guid[3]) = (guid[3], guid[0]);
            (guid[1], guid[2]) = (guid[2], guid[1]);
            (guid[4], guid[5]) = (guid[5], guid[4]);
            (guid[6], guid[7]) = (guid[7], guid[6]);
        }

        private async Task<bool> LdapAuthenticationAsync(string username, string password)
        {
            try
            {
                using var connection = new LdapConnection();

                await connection.ConnectAsync(_ldapSettings.Server, 389);

                string userDn =
                    $"uid={username},{_ldapSettings.PeopleOu},{_ldapSettings.Tree}";

                await connection.BindAsync(userDn, password);

                return connection.Bound;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "LDAP authentication başarısız. Username: {Username}",
                    username);

                return false;
            }
        }
    }
}