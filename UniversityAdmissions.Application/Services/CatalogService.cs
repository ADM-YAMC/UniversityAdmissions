using UniversityAdmissions.Application.DTOs.Catalogs;
using UniversityAdmissions.Application.Interfaces.Repositories;
using UniversityAdmissions.Application.Interfaces.Services;
using UniversityAdmissions.Application.Responses;
namespace UniversityAdmissions.Application.Services;
public class CatalogService : ICatalogService
{
    private readonly ICatalogRepository _repository;
    public CatalogService(ICatalogRepository repository)
    {
        _repository = repository;
    }
    public Task<Response<CatalogItemDto>> GetGendersAsync() => ExecuteAsync(() => _repository.GetGendersAsync());
    public Task<Response<CatalogItemDto>> GetIdentificationTypesAsync() => ExecuteAsync(() => _repository.GetIdentificationTypesAsync());
    public Task<Response<CatalogItemDto>> GetAdmissionStatusesAsync() => ExecuteAsync(() => _repository.GetAdmissionStatusesAsync());
    public Task<Response<CatalogItemDto>> GetDocumentTypesAsync() => ExecuteAsync(() => _repository.GetDocumentTypesAsync());
    private static async Task<Response<CatalogItemDto>> ExecuteAsync(Func<Task<List<CatalogItemDto>>> action)
    {
        var response = new Response<CatalogItemDto>();
        try
        {
            response.DataList = await action();
            response.Success = true;
            response.Messages!.Add("Catálogo consultado exitosamente");
        }
        catch (Exception)
        {
            response.Errors!.Add("Ocurrió un error al consultar el catálogo");
        }
        return response;
    }
}