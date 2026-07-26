using RUYA_API.Application.Services.Auth.Interfaces;
using RUYA_API.Application.Services.Auth.Service;

namespace RUYA_API.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register the Use Case / Service
            services.AddScoped<IAuthService, AuthService>();

            // (No MediatR, No FluentValidation, No Scanners needed for now!)

            return services;
        }
    }
}
