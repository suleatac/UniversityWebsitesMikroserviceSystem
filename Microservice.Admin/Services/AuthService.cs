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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;
        private readonly LdapSetting _ldapSettings;

        public AuthService(
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService,
            ILogger<AuthService> logger,
            IOptions<LdapSetting> ldapSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _logger = logger;
            _ldapSettings = ldapSettings.Value;
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

                var username = signInViewModel.Username.Trim().ToLower();
                var password = "eJU3e_1TdwkTi@c";

                // LDAP doğrulama
                var ldapResult = await LdapAuthenticationAsync(username, password);

                if (!ldapResult)
                {
                    _logger.LogWarning(
                        "LDAP authentication başarısız. Username: {Username}",
                        username);

                    return ServiceResult.Error(
                        "Authentication Error",
                        "Kullanıcı adı veya şifre hatalı."
                    );
                }

                _logger.LogInformation(
                    "LDAP authentication başarılı. Username: {Username}",
                    username);

                // Keycloak token al
                var tokenResponse =
                    await _tokenService.GetPasswordAccessToken(signInViewModel);

                if (tokenResponse?.Data == null ||
                    string.IsNullOrEmpty(tokenResponse.Data.AccessToken))
                {
                    _logger.LogWarning(
                        "Keycloak token alınamadı. Username: {Username}",
                        username);

                    return ServiceResult.Error(
                        tokenResponse?.Data?.Error ?? "Auth Error",
                        tokenResponse?.Data?.ErrorDescription ?? "Login failed"
                    );
                }

                // Claimleri çıkar
                var userClaims = _tokenService
                    .ExtractClaims(tokenResponse.Data.AccessToken);

                var filteredClaims = userClaims
                    .Where(c =>
                        c.Type == "sub" ||
                        c.Type == ClaimTypes.Name ||
                        c.Type == ClaimTypes.NameIdentifier ||
                        c.Type == ClaimTypes.Role)
                    .ToList();

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
                    username);

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