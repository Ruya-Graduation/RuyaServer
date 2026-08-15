using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RUYA_API.Application.Common.Interfaces;
using RUYA_API.Application.Services.Chat.DTOs;
using RUYA_API.Application.Services.Chat.Interfaces;
using RUYA_API.Domain.Entities;
using RUYA_API.Infrastructure.Common;
using RUYA_API.Infrastructure.Context;
using RUYA_API.Infrastructure.Identity;
using RUYA_API.Infrastructure.Persistence.Repositories;
using RUYA_API.Infrastructure.Services;
using RUYA_API.Infrastructure.Services.AI;

namespace RUYA_API.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            // 1. Register the settings so you can inject IOptions<JwtSettings>
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            // 1. DbContext
            services.AddDbContext<RuyaContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("main")));

            // 2. Identity Registration (UserManager uses AppDbContext)
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<RuyaContext>()
                .AddDefaultTokenProviders();

            // 3. DI for our wrappers
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJWTGenerator, JWTGenerator>();

            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.Configure<AIServiceSettings>(configuration.GetSection("AIServiceSettings"));

            services.AddScoped<IImageService, CloudinaryService>();
            services.AddScoped<IAIService, FakeAIService>();

            // Repositories
            services.AddScoped<IChatSessionRepository, ChatSessionRepository>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();

            // HTTP AI Clients
            var aiSettings = configuration.GetSection("AIServiceSettings").Get<AIServiceSettings>() ?? new AIServiceSettings();

            services.AddHttpClient<IVisionAiClient, VisionAiClient>(client =>
            {
                client.BaseAddress = new Uri(aiSettings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<IChatAiClient, ChatAiClient>(client =>
            {
                client.BaseAddress = new Uri(aiSettings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(60);
            });

            return services;
        }
    }
}
