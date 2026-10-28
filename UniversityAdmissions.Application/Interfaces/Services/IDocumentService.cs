using UniversityAdmissions.Application.DTOs.Documents;
using UniversityAdmissions.Application.Responses;
namespace UniversityAdmissions.Application.Interfaces.Services;
public interface IDocumentService
{
    Task<Response<AdmissionDocumentDto>> UploadAsync(UploadDocumentDto dto);
    Task<Response<AdmissionDocumentDto>> GetByAdmissionIdAsync(int admissionId);
    Task<Response<DocumentDownloadDto>> DownloadAsync(int documentId);
}