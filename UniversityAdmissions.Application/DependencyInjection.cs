using Microsoft.Extensions.DependencyInjection;
using UniversityAdmissions.Application.Interfaces.Services;
using UniversityAdmissions.Application.Services;
namespace UniversityAdmissions.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IApplicantService, ApplicantService>();
        services.AddScoped<IAdmissionService, AdmissionService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<ICatalogService, CatalogService>();
        return services;
    }
}