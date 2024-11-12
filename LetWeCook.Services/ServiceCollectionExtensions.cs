using CloudinaryDotNet;
using LetWeCook.Data;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.IngredientRepositories;
using LetWeCook.Data.Repositories.MediaUrlRepositories;
using LetWeCook.Data.Repositories.RecipeRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.FileStorageServices;
using LetWeCook.Services.IngredientServices;
using LetWeCook.Services.MediaUrlServices;
using LetWeCook.Services.RecipeServices;
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
        services.AddScoped<IMediaUrlRepository, MediaUrlRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();

        Cloudinary cloudinary = new Cloudinary(appSettings.CloudinaryUrl);
        cloudinary.Api.Secure = true;
        services.AddSingleton(cloudinary);

        services.AddScoped<IFileStorageService, CloudinaryFileStorageService>();
        services.AddScoped<IIngredientService, IngredientService>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IMediaUrlService, MediaUrlService>();

    }
}
