namespace Microservice.Admin.Settings
{
    /// <summary>
    /// Kimlik doğrulama boyunca access/refresh token'ların taşındığı anahtarlar.
    /// <see cref="Microsoft.AspNetCore.Authentication.AuthenticationProperties.Items"/>
    /// içinde bu anahtarlarla saklanır ve <c>GetTokenAsync</c> ile okunur.
    /// </summary>
    public static class AuthTokenKeys
    {
        public const string AccessToken = "access_token";
        public const string RefreshToken = "refresh_token";
    }
}
