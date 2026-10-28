using UniversityAdmissions.Application.DTOs.Catalogs;
namespace UniversityAdmissions.Application.Interfaces.Repositories;
public interface ICatalogRepository
{
    Task<List<CatalogItemDto>> GetGendersAsync();
    Task<List<CatalogItemDto>> GetIdentificationTypesAsync();
    Task<List<CatalogItemDto>> GetAdmissionStatusesAsync();
    Task<List<CatalogItemDto>> GetDocumentTypesAsync();
}