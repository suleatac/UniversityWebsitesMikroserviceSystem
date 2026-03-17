using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Shared.Extentions
{
    public static class CommonServicesExt
    {
        public static IServiceCollection AddCommonServiceExt(this IServiceCollection services, Type assembly)
        {
        
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(assembly));
            services.AddAutoMapper(cfg => { }, assembly );
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining(assembly);  
            return services;
        }
    }
}
