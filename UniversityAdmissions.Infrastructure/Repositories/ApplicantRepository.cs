using Microsoft.EntityFrameworkCore;
using UniversityAdmissions.Application.Interfaces.Repositories;
using UniversityAdmissions.Domain.Entities;
using UniversityAdmissions.Infrastructure.Persistence;
namespace UniversityAdmissions.Infrastructure.Repositories;
public class ApplicantRepository : IApplicantRepository
{
    private readonly ApplicationDbContext _context;
    public ApplicantRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public Task<Applicant?> GetByIdAsync(int id)
        => _context.Applicants.Include(x => x.Gender).Include(x => x.IdentificationType).FirstOrDefaultAsync(x => x.Id == id);
    public Task<Applicant?> GetByIdentificationAsync(int typeId, string number)
        => _context.Applicants.FirstOrDefaultAsync(x => x.IdentificationTypeId == typeId && x.IdentificationNumber == number);
    public Task<List<Applicant>> GetAllAsync()
        => _context.Applicants.AsNoTracking().Include(x => x.Gender).Include(x => x.IdentificationType).OrderBy(x => x.FirstName).ToListAsync();
    public async Task<Applicant> AddAsync(Applicant applicant)
    {
        _context.Applicants.Add(applicant);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(applicant.Id))!;
    }
}