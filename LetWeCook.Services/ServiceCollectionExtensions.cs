using LetWeCook.Data;
using LetWeCook.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetWeCook.Services;

public static class ServiceCollectionExtensions
{
	public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<LetWeCookDbContext>(options =>
		{
			options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
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
	}
}
