using RUYA_API.Application.Services.Auth.Interfaces;
using RUYA_API.Application.Services.Auth.Service;

namespace RUYA_API.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the Use Case / Service
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IAuthService, AuthService>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddMemoryCache();

            return services;
        }
    }
}
