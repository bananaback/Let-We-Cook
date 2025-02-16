using LetWeCook.Common.Enums;
using LetWeCook.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public static async Task SeedDietaryPreferencesAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LetWeCookDbContext>();

            var dietaryOptions = new[]
            {
                new { Name = "Vegan", Description = "No animal products, plant-based diet.", Color = "bg-green-600", Icon = "🥦" },
                new { Name = "Vegetarian", Description = "No meat, but allows dairy and eggs.", Color = "bg-yellow-500", Icon = "🥕" },
                new { Name = "Keto", Description = "Low-carb, high-fat diet.", Color = "bg-blue-600", Icon = "🥩" },
                new { Name = "Paleo", Description = "Whole foods, avoids processed items.", Color = "bg-orange-500", Icon = "🍗" },
                new { Name = "Gluten-Free", Description = "Avoids wheat, barley, and rye.", Color = "bg-purple-600", Icon = "🌾" },
                new { Name = "Dairy-Free", Description = "No milk, cheese, or dairy products.", Color = "bg-red-500", Icon = "🥛" },
                new { Name = "Halal", Description = "Follows Islamic dietary laws.", Color = "bg-teal-600", Icon = "🕌" },
                new { Name = "Kosher", Description = "Follows Jewish dietary laws.", Color = "bg-indigo-600", Icon = "✡️" }
            };

            foreach (var option in dietaryOptions)
            {
                bool exists = await context.DietaryPreferences
                    .AnyAsync(dp => dp.Value == option.Name);

                if (!exists)
                {
                    context.DietaryPreferences.Add(new DietaryPreference
                    {
                        Id = Guid.NewGuid(),
                        Value = option.Name,
                        Description = option.Description,
                        Color = option.Color,
                        Icon = option.Icon
                    });
                }
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            // You can add other seeding logic here if necessary
        }
    }
}
