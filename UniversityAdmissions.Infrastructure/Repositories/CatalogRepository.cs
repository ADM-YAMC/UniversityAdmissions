using Microsoft.EntityFrameworkCore;
using UniversityAdmissions.Application.DTOs.Catalogs;
using UniversityAdmissions.Application.Interfaces.Repositories;
using UniversityAdmissions.Infrastructure.Persistence;
namespace UniversityAdmissions.Infrastructure.Repositories;
public class CatalogRepository : ICatalogRepository
{
    private readonly ApplicationDbContext _context;
    public CatalogRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public Task<List<CatalogItemDto>> GetGendersAsync()
        =>_context.Genders.AsNoTracking().Select(x => new CatalogItemDto { Id = x.Id, Name = x.Name }).ToListAsync();

    public Task<List<CatalogItemDto>> GetIdentificationTypesAsync()
        => _context.IdentificationTypes.AsNoTracking().Select(x => new CatalogItemDto { Id = x.Id, Name = x.Name }).ToListAsync();
    
    public Task<List<CatalogItemDto>> GetAdmissionStatusesAsync()
        => _context.AdmissionStatuses.AsNoTracking().Select(x => new CatalogItemDto { Id = x.Id, Name = x.Name }).ToListAsync();
    public Task<List<CatalogItemDto>> GetDocumentTypesAsync()
        => _context.DocumentTypes.AsNoTracking().Select(x => new CatalogItemDto { Id = x.Id, Name = x.Name }).ToListAsync();
}