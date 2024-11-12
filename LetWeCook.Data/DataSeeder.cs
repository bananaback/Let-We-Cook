using LetWeCook.Common.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LetWeCook.Data
{
    public class DataSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                string roleName = role.ToString(); // Convert enum to string

                // Check if the role already exists
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                    // Optionally handle the result of role creation (e.g., logging)
                }
            }
        }

        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            // You can add other seeding logic here if necessary
        }
    }
}
