using UniversityAdmissions.Application.DTOs.Admissions;
using UniversityAdmissions.Application.Responses;
namespace UniversityAdmissions.Application.Interfaces.Services;
public interface IAdmissionService
{
    Task<Response<AdmissionDto>> CreateAsync(CreateAdmissionDto dto);
    Task<Response<AdmissionDto>> GetByIdAsync(int id);
    Task<Response<AdmissionDto>> GetAllAsync();
    Task<Response<AdmissionDto>> ChangeStatusAsync(ChangeAdmissionStatusDto dto);
    Task<Response<PublicAdmissionStatusDto>> GetPublicStatusAsync(int admissionId, string identificationNumber);

}
