using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversityAdmissions.Application.Security;
namespace UniversityAdmissions.Infrastructure.Identity;
public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var roles = new[]
        {
            new ApplicationRole { Name = Roles.Administrator, Description = "Administrador general del sistema" },
            new ApplicationRole { Name = Roles.AdmissionsOfficer, Description = "Personal encargado de revisar solicitudes" },
            new ApplicationRole { Name = Roles.Approver, Description = "Personal autorizado para aprobar o rechazar solicitudes" }
        };
        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role.Name!)) continue;
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
        }
        var email = configuration["AdminUser:Email"];
        var password = configuration["AdminUser:Password"];
        var firstName = configuration["AdminUser:FirstName"];
        var lastName = configuration["AdminUser:LastName"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return;
        var administrator = await userManager.FindByEmailAsync(email);
        if (administrator == null)
        {
            administrator = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName ?? "Administrador",
                LastName = lastName ?? "Sistema",
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            var result = await userManager.CreateAsync(administrator, password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
        }
        if (!await userManager.IsInRoleAsync(administrator, Roles.Administrator))
        {
            var result = await userManager.AddToRoleAsync(administrator, Roles.Administrator);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
        }
    }
}