using UniversityAdmissions.Application;
using UniversityAdmissions.Infrastructure;
using UniversityAdmissions.Infrastructure.Identity;
using UniversityAdmissions.Application.Security;
using UniversityAdmissions;
using UniversityAdmissions.Endpoints;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Permissions.ViewAdmissions, p => p.RequireRole(Roles.Administrator, Roles.AdmissionsOfficer, Roles.Approver));
    options.AddPolicy(Permissions.ReviewAdmissions, p => p.RequireRole(Roles.Administrator, Roles.AdmissionsOfficer));
    options.AddPolicy(Permissions.ApproveAdmissions, p => p.RequireRole(Roles.Administrator, Roles.Approver));
    options.AddPolicy(Permissions.RejectAdmissions, p => p.RequireRole(Roles.Administrator, Roles.Approver));
    options.AddPolicy(Permissions.ViewDocuments, p => p.RequireRole(Roles.Administrator, Roles.AdmissionsOfficer, Roles.Approver));
    options.AddPolicy(Permissions.ManageUsers, p => p.RequireRole(Roles.Administrator));
});
builder.Services.AddCascadingAuthenticationState();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/admin/login";
    options.AccessDeniedPath = "/admin/access-denied";
});
var app = builder.Build();
app.MapAdminAuthEndpoints();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
await IdentitySeeder.SeedAsync(app.Services);
app.Run();