using LetWeCook.Common.Enums;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.IngredientRepositories;
using LetWeCook.Data.Repositories.IngredientSectionRepositories;
using LetWeCook.Data.Repositories.MediaUrlRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text.Json;

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

        public static async Task ExportIngredientsToFile(IServiceProvider serviceProvider, string filePath, CancellationToken cancellationToken = default)
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "ingredients.json");

            try
            {
                using var scope = serviceProvider.CreateScope();
                var ingredientRepository = scope.ServiceProvider.GetRequiredService<IIngredientRepository>();

                var getIngredientsResult = await ingredientRepository.GetIngredientsForDataExporter(cancellationToken);

                if (!getIngredientsResult.IsSuccess)
                {
                    Console.WriteLine("Failed to retrieve ingredients.");
                    return;
                }

                var ingredients = getIngredientsResult.Data;

                if (ingredients!.Count == 0)
                {
                    Console.WriteLine("There are no ingredients to retrieve.");
                    return;
                }

                // Use JsonSerializerOptions for concise JSON
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull // Ignore nulls
                };

                // Transform data for clean output
                var transformedIngredients = ingredients.Select(ingredient => new
                {
                    id = ingredient.Id,
                    name = ingredient.Name,
                    description = ingredient.Description,
                    coverImageUrl = ingredient.CoverImageUrl is not null
                        ? new { id = ingredient.CoverImageUrl.Id, url = ingredient.CoverImageUrl.Url }
                        : null,
                    ingredientSections = ingredient.IngredientSections.OrderBy(section => section.Order).Select(section => new
                    {
                        id = section.Id,
                        textContent = section.TextContent,
                        mediaUrl = section.MediaUrl is not null
                            ? new { id = section.MediaUrl.Url, url = section.MediaUrl.Url }
                            : null,
                        order = section.Order
                    })
                });

                var jsonData = System.Text.Json.JsonSerializer.Serialize(transformedIngredients, options);

                await File.WriteAllTextAsync(filePath, jsonData, cancellationToken);

                Console.WriteLine($"Ingredients written to JSON file at: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing ingredients to JSON: {ex.Message}");
            }
        }

        public static async Task ParseJsonAndLogAsync(string filePath, CancellationToken cancellationToken)
        {
            // Read the JSON file into a string
            var jsonData = await File.ReadAllTextAsync(filePath, cancellationToken);

            // Deserialize the JSON into a list of Ingredient objects
            var ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(jsonData);

            foreach (var ingredient in ingredients)
            {
                Console.WriteLine(ingredient.Id);
                Console.WriteLine(ingredient.Name);
                Console.WriteLine(ingredient.Description);
                Console.WriteLine(ingredient.CoverImageUrl?.Id);
                Console.WriteLine(ingredient.CoverImageUrl?.Url);
                for (int i = 0; i < ingredient.IngredientSections.Count; i++)
                {
                    var ingredientSection = ingredient.IngredientSections[i];
                    Console.WriteLine($"\t{ingredientSection.Id}");
                    Console.WriteLine($"\t{ingredientSection.Ingredient?.Name}");
                    Console.WriteLine($"\t{ingredientSection.MediaUrl?.Url}");
                    Console.WriteLine($"\t{ingredientSection.TextContent}");
                    Console.WriteLine($"\t{ingredientSection.Order}");
                }

            }
        }

        public static async Task ParseJsonAndSeedDatabase(IServiceProvider serviceProvider, string filePath, CancellationToken cancellationToken)
        {
            // Read the JSON file into a string
            var jsonData = await File.ReadAllTextAsync(filePath, cancellationToken);

            // Deserialize the JSON into a list of Ingredient objects
            var ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(jsonData);

            try
            {
                using var scope = serviceProvider.CreateScope();
                var ingredientRepository = scope.ServiceProvider.GetRequiredService<IIngredientRepository>();
                var ingredientSectionRepository = scope.ServiceProvider.GetRequiredService<IIngredientSectionRepository>();

                var mediaUrlRepository = scope.ServiceProvider.GetRequiredService<IMediaUrlRepository>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                foreach (var ingredient in ingredients)
                {
                    //Console.WriteLine(ingredient.Id);
                    var getExistingIngredientResult = await ingredientRepository.GetIngredientByIdAsync(ingredient.Id, cancellationToken);

                    if (getExistingIngredientResult.IsSuccess)
                    {
                        continue;
                    }

                    var getExistingOverImageResult = await mediaUrlRepository.GetMediaUrlByIdAsync(ingredient.CoverImageUrl.Id, cancellationToken);

                    if (!getExistingIngredientResult.IsSuccess)
                    {
                        MediaUrl coverImageMediaUrl = new MediaUrl
                        {
                            Id = ingredient.CoverImageUrl.Id,
                            Url = ingredient.CoverImageUrl.Url,
                            Alt = ingredient.CoverImageUrl.Url
                        };
                        await mediaUrlRepository.CreateMediaUrlAsync(coverImageMediaUrl, cancellationToken);
                    }

                    Console.WriteLine(ingredient.Name);
                    Console.WriteLine(ingredient.Description);
                    //Console.WriteLine(ingredient.CoverImageUrl?.Id);
                    //Console.WriteLine(ingredient.CoverImageUrl?.Url);
                    for (int i = 0; i < ingredient.IngredientSections.Count; i++)
                    {
                        var ingredientSection = ingredient.IngredientSections[i];

                        var getIngredientSectionResult = await ingredientSectionRepository.GetIngredientSectionByIdAsync(ingredientSection.Id, cancellationToken);

                        if (!getIngredientSectionResult.IsSuccess)
                        {

                        }
                        //Console.WriteLine($"\t{ingredientSection.Id}");
                        Console.WriteLine($"\t{ingredientSection.Ingredient?.Name}");
                        Console.WriteLine($"\t{ingredientSection.MediaUrl?.Url}");
                        Console.WriteLine($"\t{ingredientSection.TextContent}");
                        Console.WriteLine($"\t{ingredientSection.Order}");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding ingredients to db: {ex.Message}");
            }
        }


        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            // You can add other seeding logic here if necessary
        }
    }
}
