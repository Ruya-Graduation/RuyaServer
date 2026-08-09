using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RUYA_API.Application.Common.Interfaces;
using RUYA_API.Domain.Entities;
using RUYA_API.Infrastructure.Common;
using RUYA_API.Infrastructure.Context;
using RUYA_API.Infrastructure.Identity;
using RUYA_API.Infrastructure.Services;

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
                options.UseSqlServer(configuration.GetConnectionString("deployed")));

            // 2. Identity Registration (UserManager uses AppDbContext)
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<RuyaContext>()
                .AddDefaultTokenProviders();

            // 3. DI for our wrappers
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJWTGenerator, JWTGenerator>();

            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            services.AddScoped<IImageService, CloudinaryService>();

            services.AddScoped<IAIService, FakeAIService>();

            return services;
        }
    }
}
