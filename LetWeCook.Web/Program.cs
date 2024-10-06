using LetWeCook.Services;
using LetWeCook.Services.AuthenticationServices;
using LetWeCook.Services.EmailSenders;
using LetWeCook.Web.Models.Configs;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddTransient<IEmailSender, EmailSender>();

// Bind app configs to some strongly typed models
SmtpSettings smtpSettings = new SmtpSettings();
builder.Configuration.GetSection("SmtpSettings").Bind(smtpSettings);
builder.Services.AddSingleton(smtpSettings);

AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
builder.Configuration.GetSection("Authentications").Bind(authenticationConfiguration);
builder.Services.AddSingleton(authenticationConfiguration);

// Configure application cookies with security best practices
builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.HttpOnly = true; // Prevent access to cookies via JavaScript
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure cookies are only sent over HTTPS
	options.Cookie.SameSite = SameSiteMode.Lax; // Mitigate CSRF attacks
	options.LoginPath = "/Account/Auth/Login";
	options.AccessDeniedPath = "/Account/Auth/AccessDenied";
	options.SlidingExpiration = true; // Refresh expiration time on each request
	options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Set the cookie expiration time
});

/*
For the typical scenario with ASP.NET Core Identity, even if you don t specify the DefaultSignInScheme, 
it defaults to the external scheme used by Identity. This separation helps keep the internal login processes 
(your traditional form login) and external login processes distinct, ensuring smooth functionality. 
 */
builder.Services
	.AddAuthentication()
	// And then google external login
	.AddGoogle(googleOptions =>
	{
		googleOptions.ClientId = authenticationConfiguration.Google.ClientId;
		googleOptions.ClientSecret = authenticationConfiguration.Google.ClientSecret;
		googleOptions.Scope.Add("profile");
		googleOptions.Events.OnCreatingTicket = (context) =>
		{
			var picture = context.User.GetProperty("picture").GetString();
			if (picture != null)
			{
				context.Identity?.AddClaim(new Claim("picture", picture));
			}
			return Task.CompletedTask;
		};
		googleOptions.Events.OnRedirectToAuthorizationEndpoint = context =>
		{
			// Always prompt the user to choose their account
			context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
			return Task.CompletedTask;
		};
	})
	.AddFacebook(facebookOptions =>
	{
		facebookOptions.AppId = authenticationConfiguration.Facebook.ClientId;
		facebookOptions.AppSecret = authenticationConfiguration.Facebook.ClientSecret;
		facebookOptions.Fields.Add("picture");
		facebookOptions.Events.OnCreatingTicket = (context) =>
		{
			var picture = context.User.GetProperty("picture").GetProperty("data").GetProperty("url").ToString();
			if (picture != null)
			{
				context.Identity?.AddClaim(new Claim("picture", picture));
			}

			return Task.CompletedTask;
		};
		facebookOptions.Events.OnRedirectToAuthorizationEndpoint = context =>
		{
			// Always prompt the user to choose their Facebook account
			context.Response.Redirect(context.RedirectUri + "&auth_type=reauthenticate");
			return Task.CompletedTask;
		};
	});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "MyArea",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
