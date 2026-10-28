using UniversityAdmissions.Domain.Entities;
namespace UniversityAdmissions.Application.Interfaces.Repositories;
public interface IAdmissionDocumentRepository
{
    Task<AdmissionDocument> AddAsync(AdmissionDocument document);
    Task<AdmissionDocument?> GetByIdAsync(int id);
    Task<List<AdmissionDocument>> GetByAdmissionIdAsync(int admissionId);
}