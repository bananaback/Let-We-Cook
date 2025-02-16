using CloudinaryDotNet;
using LetWeCook.Data;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.CollectionRecipeRepositories;
using LetWeCook.Data.Repositories.DietaryPreferenceRepositories;
using LetWeCook.Data.Repositories.DishCollectionRepositories;
using LetWeCook.Data.Repositories.IngredientRepositories;
using LetWeCook.Data.Repositories.IngredientSectionRepositories;
using LetWeCook.Data.Repositories.MediaUrlRepositories;
using LetWeCook.Data.Repositories.ProfileRepositories;
using LetWeCook.Data.Repositories.RecipeRepositories;
using LetWeCook.Data.Repositories.RecipeReviewRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Data.Repositories.UserDietaryPreferenceRepositories;
using LetWeCook.Services.DishCollectionServices;
using LetWeCook.Services.FileStorageServices;
using LetWeCook.Services.IngredientServices;
using LetWeCook.Services.MediaUrlServices;
using LetWeCook.Services.ProfileServices;
using LetWeCook.Services.RecipeReviewServices;
using LetWeCook.Services.RecipeServices;
using LetWeCook.Services.UserDietaryPreferenceServices;
using LetWeCook.Web.Models.Configs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LetWeCook.Services;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContext<LetWeCookDbContext>(options =>
        {
            options.UseSqlServer(appSettings.ConnectionString);
        });

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
            options.SignIn.RequireConfirmedAccount = true;
        })
       .AddEntityFrameworkStores<LetWeCookDbContext>()
       .AddDefaultTokenProviders();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IIngredientRepository, IngredientRepository>();
        services.AddScoped<IIngredientSectionRepository, IngredientSectionRepository>();

        services.AddScoped<IMediaUrlRepository, MediaUrlRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IDishCollectionRepository, DishCollectionRepository>();
        services.AddScoped<ICollectionRecipeRepository, CollectionRecipeRepository>();
        services.AddScoped<IRecipeReviewRepository, RecipeReviewRepository>();
        services.AddScoped<IDietaryPreferenceRepository, DietaryPreferenceRepository>();
        services.AddScoped<IUserDietaryPreferenceRepository, UserDietaryPreferenceRepository>();


        Cloudinary cloudinary = new Cloudinary(appSettings.CloudinaryUrl);
        cloudinary.Api.Secure = true;
        services.AddSingleton(cloudinary);

        services.AddScoped<IFileStorageService, CloudinaryFileStorageService>();
        services.AddScoped<IIngredientService, IngredientService>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IMediaUrlService, MediaUrlService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IDishCollectionService, DishCollectionService>();
        services.AddScoped<IRecipeReviewService, RecipeReviewService>();
        services.AddScoped<IUserDietaryPreferenceService, UserDietaryPreferenceService>();

    }
}
