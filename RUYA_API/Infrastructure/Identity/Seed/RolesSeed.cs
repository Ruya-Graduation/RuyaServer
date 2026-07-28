using Microsoft.AspNetCore.Identity;
using RUYA_API.Application.Common;

namespace RUYA_API.Infrastructure.Identity.Seed
{
    public class RolesSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in Roles.All)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
