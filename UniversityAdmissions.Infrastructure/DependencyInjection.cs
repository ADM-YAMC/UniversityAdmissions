using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversityAdmissions.Application.Interfaces.Repositories;
using UniversityAdmissions.Application.Interfaces.Storage;
using UniversityAdmissions.Infrastructure.Identity;
using UniversityAdmissions.Infrastructure.Persistence;
using UniversityAdmissions.Infrastructure.Repositories;
using UniversityAdmissions.Infrastructure.Storage;
namespace UniversityAdmissions.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequiredLength = 10;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/account/login";
            options.AccessDeniedPath = "/account/access-denied";
            options.ExpireTimeSpan = TimeSpan.FromHours(2);
            options.SlidingExpiration = true;
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Lax;
        });
        services.AddScoped<IApplicantRepository, ApplicantRepository>();
        services.AddScoped<IAdmissionRepository, AdmissionRepository>();
        services.AddScoped<IAdmissionDocumentRepository, AdmissionDocumentRepository>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddSingleton<IFileStorage>(_ =>
        {
            var root = configuration["FileStorage:RootPath"];
            if (string.IsNullOrWhiteSpace(root))
                throw new InvalidOperationException("Configura FileStorage:RootPath");
            return new LocalFileStorage(root);
        });
        return services;
    }
}