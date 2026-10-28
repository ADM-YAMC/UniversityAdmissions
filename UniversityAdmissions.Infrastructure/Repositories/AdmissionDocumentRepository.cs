using Microsoft.EntityFrameworkCore;
using UniversityAdmissions.Application.Interfaces.Repositories;
using UniversityAdmissions.Domain.Entities;
using UniversityAdmissions.Infrastructure.Persistence;
namespace UniversityAdmissions.Infrastructure.Repositories;
public class AdmissionDocumentRepository : IAdmissionDocumentRepository
{
    private readonly ApplicationDbContext _context;
    public AdmissionDocumentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<AdmissionDocument> AddAsync(AdmissionDocument document)
    {
        _context.AdmissionDocuments.Add(document);
        await _context.SaveChangesAsync();
        return document;
    }
    public Task<AdmissionDocument?> GetByIdAsync(int id)
    =>_context.AdmissionDocuments.AsNoTracking().Include(x => x.DocumentType).FirstOrDefaultAsync(x => x.Id == id);
    public Task<List<AdmissionDocument>> GetByAdmissionIdAsync(int admissionId)
        => _context.AdmissionDocuments.AsNoTracking().Include(x => x.DocumentType).Where(x => x.AdmissionId == admissionId).ToListAsync();
}