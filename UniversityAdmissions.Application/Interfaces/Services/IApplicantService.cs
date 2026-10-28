using UniversityAdmissions.Application.DTOs.Applicants;
using UniversityAdmissions.Application.Responses;
namespace UniversityAdmissions.Application.Interfaces.Services;
public interface IApplicantService
{
    Task<Response<ApplicantDto>> CreateAsync(CreateApplicantDto dto);
    Task<Response<ApplicantDto>> GetByIdAsync(int id);
    Task<Response<ApplicantDto>> GetAllAsync();
}