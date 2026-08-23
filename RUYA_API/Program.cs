using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RUYA_API.Application.DependencyInjection;
using RUYA_API.Infrastructure.Common;
using RUYA_API.Infrastructure.Data;
using RUYA_API.Infrastructure.DependencyInjection;
using RUYA_API.Infrastructure.Identity.Seed;
using RUYA_API.Middleware;
using RUYA_API.Responses;
using System.Security.Claims;
using System.Text;

namespace RUYA_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            
            builder.Services.AddMemoryCache();

            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
                 ?? throw new InvalidOperationException("JwtSettings section is missing.");

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Add this
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // 1. Validate the Issuer
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer, // <-- REQUIRED: Must match the issuer in your generator

                    // 2. Validate the Signature
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey!)), // <-- REQUIRED: The key to verify the token

                    // 3. Validate Lifetime (Expiration)
                    ValidateLifetime = true,

                    // 4. Audience (Set to false since you don't generate an audience in your generator)
                    ValidateAudience = false,

                    ClockSkew = TimeSpan.FromSeconds(30)
                };
                // ADD THIS ENTIRE BLOCK TO SEE THE ERROR IN YOUR CONSOLE
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("❌ JWT AUTH FAILED: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        Console.WriteLine("📩 JWT RECEIVED HEADER: " + context.Request.Headers.Authorization);
                        return Task.CompletedTask;
                    }
                };

            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddControllers();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new BadRequestObjectResult(
                        ResponseFactory.Failure("Validation failed.", errors));
                };
            });
            builder.Services.AddOpenApi();

            var app = builder.Build();
            
            // Enable CORS
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMiddleware<ExceptionMiddleware>();

            //if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.MapGet("/api", () =>
            {
                return Results.Ok(new
                {
                    name = "RUYA API",
                    description = "AI-powered historical companion Platform",
                    version = "v1",
                    status = "Running"
                });
            });
            app.MapControllers();
            
            // Seed roles and initial data
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await RolesSeed.SeedRolesAsync(roleManager);
                
                // Seed database with sites, artifacts, and admin user
                await SeedDatabase.InitializeAsync(app.Services);
            }
            
            app.Run();
        }
    }
}
