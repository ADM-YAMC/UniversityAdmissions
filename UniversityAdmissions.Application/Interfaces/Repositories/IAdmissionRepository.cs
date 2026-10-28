using UniversityAdmissions.Domain.Entities;
namespace UniversityAdmissions.Application.Interfaces.Repositories;
public interface IAdmissionRepository
{
    Task<Admission?> GetByIdAsync(int id);
    Task<Admission?> GetPublicStatusAsync(int admissionId, string identificationNumber);
    Task<List<Admission>> GetAllAsync();
    Task<Admission> AddAsync(Admission admission);
    Task<Admission?> ChangeStatusAsync(int admissionId, int newStatusId, string userId, string? comments);
}
