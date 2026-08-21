using RUYA_API.Application.Services.Admin.Interfaces;
using RUYA_API.Application.Services.Admin.Service;
using RUYA_API.Application.Services.Auth.Interfaces;
using RUYA_API.Application.Services.Auth.Service;
using RUYA_API.Application.Services.Chat.Interfaces;
using RUYA_API.Application.Services.Chat.Service;
using RUYA_API.Application.Services.Moments.Interfaces;
using RUYA_API.Application.Services.Moments.Service;

namespace RUYA_API.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the Use Case / Service
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IArtifactService, ArtifactService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IMomentsService, MomentsService>();

            // (No MediatR, No FluentValidation, No Scanners needed for now!)
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IConversationService, ConversationService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddMemoryCache();

            return services;
        }
    }
}
