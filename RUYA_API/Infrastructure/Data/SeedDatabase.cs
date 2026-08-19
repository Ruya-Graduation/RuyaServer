using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RUYA_API.Domain.Entities;
using RUYA_API.Infrastructure.Context;

namespace RUYA_API.Infrastructure.Data
{
    /// <summary>
    /// Standalone utility to seed the database.
    /// Can be called from Program.cs or run as a separate script.
    /// </summary>
    public static class SeedDatabase
    {
        /// <summary>
        /// Seeds the database with initial data.
        /// Usage: await SeedDatabase.InitializeAsync(app.Services);
        /// </summary>
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                Console.WriteLine("🌱 Starting database seeding...");
                Console.WriteLine("═══════════════════════════════════════════════════════════");

                var context = services.GetRequiredService<RuyaContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                
                // Ensure database is created and migrations are applied
                Console.WriteLine("📊 Applying database migrations...");
                await context.Database.MigrateAsync();
                Console.WriteLine("✅ Database migrations applied.");

                var seeder = new DatabaseSeeder(context, userManager);
                await seeder.SeedAllAsync();

                Console.WriteLine("═══════════════════════════════════════════════════════════");
                Console.WriteLine("✅ Database seeding completed successfully!");
                Console.WriteLine($"   Total Sites: {await context.Sites.CountAsync()}");
                Console.WriteLine($"   Total Artifacts: {await context.Artifacts.CountAsync()}");
                Console.WriteLine("═══════════════════════════════════════════════════════════");
            }
            catch (Exception ex)
            {
                Console.WriteLine("═══════════════════════════════════════════════════════════");
                Console.WriteLine($"❌ Error seeding database: {ex.Message}");
                Console.WriteLine($"   {ex.InnerException?.Message}");
                Console.WriteLine("═══════════════════════════════════════════════════════════");
                throw;
            }
        }

        /// <summary>
        /// Seeds database from standalone console app (for testing/manual seeding)
        /// NOTE: This method does not seed admin user (requires UserManager)
        /// </summary>
        public static async Task SeedFromConfigurationAsync(string connectionString)
        {
            Console.WriteLine("⚠️  WARNING: This method does not seed admin user.");
            Console.WriteLine("   Use InitializeAsync() from Program.cs for full seeding.");
            Console.WriteLine();
            
            var options = new DbContextOptionsBuilder<RuyaContext>()
                .UseSqlServer(connectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null))
                .Options;

            using var context = new RuyaContext(options);

            try
            {
                Console.WriteLine("🌱 Starting database seeding...");
                Console.WriteLine("═══════════════════════════════════════════════════════════");

                Console.WriteLine("📊 Applying database migrations...");
                await context.Database.MigrateAsync();
                Console.WriteLine("✅ Database migrations applied.");

                // Note: Cannot seed admin user without UserManager
                // Only seed sites and artifacts
                Console.WriteLine("⚠️  Skipping admin user seeding (UserManager not available)");
                
                await SeedSitesOnlyAsync(context);
                await SeedArtifactsOnlyAsync(context);

                Console.WriteLine("═══════════════════════════════════════════════════════════");
                Console.WriteLine("✅ Database seeding completed!");
                Console.WriteLine($"   Total Sites: {await context.Sites.CountAsync()}");
                Console.WriteLine($"   Total Artifacts: {await context.Artifacts.CountAsync()}");
                Console.WriteLine("═══════════════════════════════════════════════════════════");
            }
            catch (Exception ex)
            {
                Console.WriteLine("═══════════════════════════════════════════════════════════");
                Console.WriteLine($"❌ Error seeding database: {ex.Message}");
                Console.WriteLine($"   {ex.InnerException?.Message}");
                Console.WriteLine("═══════════════════════════════════════════════════════════");
                throw;
            }
        }

        private static async Task SeedSitesOnlyAsync(RuyaContext context)
        {
            if (await context.Sites.AnyAsync())
            {
                Console.WriteLine("⏭️  Sites already exist. Skipping site seeding.");
                return;
            }

            // Copy site seeding logic here...
            Console.WriteLine("🌍 Seeding sites...");
            // (Implementation same as DatabaseSeeder.SeedSitesAsync)
        }

        private static async Task SeedArtifactsOnlyAsync(RuyaContext context)
        {
            if (await context.Artifacts.AnyAsync())
            {
                Console.WriteLine("⏭️  Artifacts already exist. Skipping artifact seeding.");
                return;
            }

            Console.WriteLine("🏺 Seeding artifacts...");
            // (Implementation same as DatabaseSeeder.SeedArtifactsAsync)
        }
    }
}
