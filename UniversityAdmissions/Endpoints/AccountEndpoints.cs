using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using UniversityAdmissions.Infrastructure.Identity;
namespace UniversityAdmissions.Endpoints;
public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/account/logout", async (HttpContext context, SignInManager<ApplicationUser> signInManager, IAntiforgery antiforgery) =>
        {
            await antiforgery.ValidateRequestAsync(context);
            await signInManager.SignOutAsync();
            return Results.LocalRedirect("/account/login");
        }).RequireAuthorization();
        return app;
    }
}