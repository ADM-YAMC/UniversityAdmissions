using UniversityAdmissions.Application.DTOs.Catalogs;
using UniversityAdmissions.Application.Responses;
namespace UniversityAdmissions.Application.Interfaces.Services;
public interface ICatalogService
{
    Task<Response<CatalogItemDto>> GetGendersAsync();
    Task<Response<CatalogItemDto>> GetIdentificationTypesAsync();
    Task<Response<CatalogItemDto>> GetAdmissionStatusesAsync();
    Task<Response<CatalogItemDto>> GetDocumentTypesAsync();
}