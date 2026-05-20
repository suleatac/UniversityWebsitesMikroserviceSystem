using Microservice.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Microservice.Shared.Extentions
{
    public static class AuthenticationExt
    {
        public static IServiceCollection AddAuthenticationAndAuthorizationExt(this IServiceCollection services, IConfiguration configuration)
        {
            /* 
             
             4 parametreye bakacağız bunlar;
             Sign, 
             Aud => personel.api, 
             Iss => http://localhost:8080/realms/ExampleMikroservice, 
             Token lifetime 

            */

            var identityOption = configuration.GetSection(nameof(IdentityOption)).Get<IdentityOption>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
                    options.Authority = identityOption!.Adress;
                    options.RequireHttpsMetadata = false;
                    options.Audience = identityOption.Audience;

                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateAudience = true,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.Name
                    };


                }).AddJwtBearer("ClientCredentialSchema", options => {
                    options.Authority = identityOption!.Adress;
                    options.RequireHttpsMetadata = false;
                    options.Audience = identityOption.Audience;

                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateAudience = true,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };



                });


            services.AddAuthorization(

                Options =>
                {
                    Options.AddPolicy("ClientCredential", policy =>
                    {
                        policy.AuthenticationSchemes.Add("ClientCredentialSchema");
                        policy.RequireAuthenticatedUser();

                    });
                    Options.AddPolicy("Instructor", policy => 
                    {
                        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim(ClaimTypes.Email);
                        policy.RequireRole(ClaimTypes.Role, "Admin");
                    });
                    Options.AddPolicy("Password", policy =>
                    {
                        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim(ClaimTypes.Email);
                    });
                });

            return services;
        }
    }
}
