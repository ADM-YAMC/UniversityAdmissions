using Microsoft.EntityFrameworkCore;
using UniversityAdmissions.Application.Interfaces.Repositories;
using UniversityAdmissions.Domain.Entities;
using UniversityAdmissions.Infrastructure.Persistence;
namespace UniversityAdmissions.Infrastructure.Repositories;
public class AdmissionRepository : IAdmissionRepository
{
    private readonly ApplicationDbContext _context;
    public AdmissionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public Task<Admission?> GetByIdAsync(int id)
        => _context.Admissions.AsNoTracking()
        .Include(x => x.AdmissionStatus)
        .Include(x => x.Documents)
        .ThenInclude(x => x.DocumentType)
        .FirstOrDefaultAsync(x => x.Id == id);
    public Task<Admission?> GetPublicStatusAsync(int admissionId, string identificationNumber)
        => _context.Admissions.AsNoTracking()
        .Include(x => x.AdmissionStatus)
        .FirstOrDefaultAsync(x => x.Id == admissionId && x.Applicant.IdentificationNumber == identificationNumber);
    public Task<List<Admission>> GetAllAsync()
        => _context.Admissions.AsNoTracking().Include(x => x.AdmissionStatus)
        .Include(x => x.Documents)
        .ThenInclude(x => x.DocumentType)
        .OrderByDescending(x => x.ApplicationDate)
        .ToListAsync();

    public async Task<Admission> AddAsync(Admission admission)
    {
        _context.Admissions.Add(admission);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(admission.Id))!;
    }
    public async Task<Admission?> ChangeStatusAsync(int admissionId, int newStatusId, string userId, string? comments)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var admission = await _context.Admissions.FirstOrDefaultAsync(x => x.Id == admissionId);
        if (admission == null || admission.AdmissionStatusId == newStatusId) return null;
        if (admission.AdmissionStatusId is 3 or 4) return null;
        if (newStatusId == 2 && admission.AdmissionStatusId != 1) return null;
        if (newStatusId is 3 or 4 && admission.AdmissionStatusId != 2) return null;
        if (!await _context.AdmissionStatuses.AnyAsync(x => x.Id == newStatusId)) return null;
        var previousStatusId = admission.AdmissionStatusId;
        admission.AdmissionStatusId = newStatusId;
        _context.AdmissionDecisions.Add(new AdmissionDecision
        {
            AdmissionId = admissionId,
            UserId = userId,
            PreviousStatusId = previousStatusId,
            NewStatusId = newStatusId,
            Comments = comments,
            DecisionDate = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return await GetByIdAsync(admissionId);
    }
}
