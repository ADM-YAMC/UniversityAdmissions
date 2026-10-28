using UniversityAdmissions.Domain.Entities;
namespace UniversityAdmissions.Application.Interfaces.Repositories;
public interface IApplicantRepository
{
    Task<Applicant?> GetByIdAsync(int id);
    Task<Applicant?> GetByIdentificationAsync(int typeId, string number);
    Task<List<Applicant>> GetAllAsync();
    Task<Applicant> AddAsync(Applicant applicant);
}