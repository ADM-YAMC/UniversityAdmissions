
using Microsoft.AspNetCore.Identity;
using UniversityAdmissions.Domain.Entities;
using UniversityAdmissions.Infrastructure.Identity;
namespace UniversityAdmissions.Endpoints;
public static class AdminAuthEndpoints
{
    public static IEndpointRouteBuilder MapAdminAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/admin/auth/login", async (HttpContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) =>
        {
            var form = await context.Request.ReadFormAsync();
            var email = form["Email"].ToString().Trim();
            var password = form["Password"].ToString();
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return Results.LocalRedirect("/admin/login?error=invalid");
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return Results.LocalRedirect("/admin/login?error=invalid");
            if (!user.IsActive)
                return Results.LocalRedirect("/admin/login?error=invalid");
            var authorized = await userManager.IsInRoleAsync(user, "Administrador") || await userManager.IsInRoleAsync(user, "OficialAdmisiones") || await userManager.IsInRoleAsync(user, "Aprobador");
            if (!authorized)
                return Results.LocalRedirect("/admin/login?error=access");
            var result = await signInManager.PasswordSignInAsync(user, password, false, lockoutOnFailure: true);
            if (result.IsLockedOut)
                return Results.LocalRedirect("/admin/login?error=locked");
            if (!result.Succeeded)
                return Results.LocalRedirect("/admin/login?error=invalid");
            return Results.LocalRedirect("/admin/admissions");
        }).AllowAnonymous();
        app.MapPost("/admin/auth/logout", async (SignInManager<ApplicationUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.LocalRedirect("/admin/login");
        }).RequireAuthorization();
        return app;
    }
}
