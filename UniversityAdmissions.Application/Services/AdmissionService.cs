using System.ComponentModel.DataAnnotations;
using UniversityAdmissions.Application.DTOs.Admissions;
using UniversityAdmissions.Application.DTOs.Applicants;
using UniversityAdmissions.Application.DTOs.Documents;
using UniversityAdmissions.Application.Interfaces.Repositories;
using UniversityAdmissions.Application.Interfaces.Services;
using UniversityAdmissions.Application.Responses;
using UniversityAdmissions.Domain.Entities;
namespace UniversityAdmissions.Application.Services;
public class AdmissionService : IAdmissionService
{
    private readonly IAdmissionRepository _repository;
    private readonly IApplicantService _applicantService;
    public AdmissionService(IAdmissionRepository repository, IApplicantService applicantService)
    {
        _repository = repository;
        _applicantService = applicantService;
    }
    public async Task<Response<AdmissionDto>> CreateAsync(CreateAdmissionDto dto)
    {
        var response = new Response<AdmissionDto>();
        try
        {
            Validator.ValidateObject(dto, new ValidationContext(dto), true);
            var applicantResponse = await _applicantService.GetByIdAsync(dto.ApplicantId);
            if (!applicantResponse.Success)
            {
                response.Errors!.AddRange(applicantResponse.Errors ?? new List<string>());
                return response;
            }
            var admission = new Admission
            {
                ApplicantId = dto.ApplicantId,
                AdmissionStatusId = 1,
                AcceptTerms = true,
                TermsAcceptedAt = DateTime.UtcNow,
                ApplicationDate = DateTime.UtcNow
            };
            admission = await _repository.AddAsync(admission);
            response.Success = true;
            response.Identity = admission.Id;
            response.SingleData = Map(admission, applicantResponse.SingleData!);
            response.Messages!.Add("Solicitud de admisión registrada exitosamente...");
        }
        catch (ValidationException ex)
        {
            response.Errors!.Add(ex.Message);
        }
        catch (Exception)
        {
            response.Errors!.Add("Ocurrió un error al registrar la solicitud...");
        }
        return response;
    }
    public async Task<Response<AdmissionDto>> GetByIdAsync(int id)
    {
        var response = new Response<AdmissionDto>();
        try
        {
            var admission = await _repository.GetByIdAsync(id);
            if (admission == null)
            {
                response.Errors!.Add("No se encontró la solicitud de admisión...");
                return response;
            }
            var applicantResponse = await _applicantService.GetByIdAsync(admission.ApplicantId);
            if (!applicantResponse.Success)
            {
                response.Errors!.AddRange(applicantResponse.Errors ?? new List<string>());
                return response;
            }
            response.Success = true;
            response.Identity = admission.Id;
            response.SingleData = Map(admission, applicantResponse.SingleData!);
        }
        catch (Exception)
        {
            response.Errors!.Add("Ocurrió un error al consultar la solicitud...");
        }
        return response;
    }
    public async Task<Response<AdmissionDto>> GetAllAsync()
    {
        var response = new Response<AdmissionDto>();
        try
        {
            var admissions = await _repository.GetAllAsync();
            var result = new List<AdmissionDto>();
            foreach (var admission in admissions)
            {
                var applicantResponse = await _applicantService.GetByIdAsync(admission.ApplicantId);
                if (!applicantResponse.Success || applicantResponse.SingleData == null)
                {
                    response.Errors!.Add($"No fue posible consultar el solicitante de la admisión {admission.Id}...");
                    return response;
                }
                result.Add(Map(admission, applicantResponse.SingleData));
            }
            response.Success = true;
            response.DataList = result;
            response.Messages!.Add("Solicitudes consultadas exitosamente...");
        }
        catch (Exception)
        {
            response.Errors!.Add("Ocurrió un error al consultar las solicitudes...");
        }
        return response;
    }
    public async Task<Response<AdmissionDto>> ChangeStatusAsync(ChangeAdmissionStatusDto dto)
    {
        var response = new Response<AdmissionDto>();
        if (dto.AdmissionId <= 0 || dto.NewStatusId is < 2 or > 4 || string.IsNullOrWhiteSpace(dto.UserId))
        {
            response.Errors!.Add("Los datos del cambio de estado no son válidos.");
            return response;
        }
        if (dto.Comments?.Length > 1000 || (dto.NewStatusId == 4 && string.IsNullOrWhiteSpace(dto.Comments)))
        {
            response.Errors!.Add("El rechazo requiere una observación de hasta 1000 caracteres.");
            return response;
        }
        try
        {
            var admission = await _repository.ChangeStatusAsync(dto.AdmissionId, dto.NewStatusId, dto.UserId, dto.Comments?.Trim());
            if (admission == null)
            {
                response.Errors!.Add("No existe la solicitud, el estado no cambió o la transición no está permitida.");
                return response;
            }
            var applicantResponse = await _applicantService.GetByIdAsync(admission.ApplicantId);
            response.Success = true;
            response.Identity = admission.Id;
            if (applicantResponse.Success && applicantResponse.SingleData != null) response.SingleData = Map(admission, applicantResponse.SingleData);
            response.Messages!.Add("Estado actualizado correctamente.");
        }
        catch (Exception)
        {
            response.Errors!.Add("No fue posible actualizar el estado de la solicitud.");
        }
        return response;
    }
    public async Task<Response<PublicAdmissionStatusDto>> GetPublicStatusAsync(int admissionId, string identificationNumber)
    {
        var response = new Response<PublicAdmissionStatusDto>();
        if (admissionId <= 0 || string.IsNullOrWhiteSpace(identificationNumber) || identificationNumber.Length > 50)
        {
            response.Errors!.Add("Introduce un número de solicitud y una identificación válidos.");
            return response;
        }
        try
        {
            var admission = await _repository.GetPublicStatusAsync(admissionId, identificationNumber.Trim());
            if (admission == null)
            {
                response.Errors!.Add("No encontramos una solicitud que coincida con los datos introducidos.");
                return response;
            }
            response.Success = true;
            response.SingleData = new PublicAdmissionStatusDto
            {
                AdmissionId = admission.Id,
                ApplicationDate = admission.ApplicationDate,
                Status = admission.AdmissionStatus?.Name ?? "Pendiente"
            };
        }
        catch (Exception)
        {
            response.Errors!.Add("No fue posible consultar el estado. Inténtalo más tarde.");
        }
        return response;
    }
    private static AdmissionDto Map(Admission admission, ApplicantDto applicant)
    {
        return new AdmissionDto
        {
            Id = admission.Id,
            ApplicationDate = admission.ApplicationDate,
            Status = admission.AdmissionStatus?.Name ?? "",
            AcceptTerms = admission.AcceptTerms,
            Applicant = applicant,
            Documents = admission.Documents.Select(d => new AdmissionDocumentDto
            {
                Id = d.Id,
                DocumentTypeId = d.DocumentTypeId,
                DocumentType = d.DocumentType?.Name ?? "",
                FileName = d.FileName,
                ContentType = d.ContentType,
                FileSize = d.FileSize,
                UploadedAt = d.UploadedAt
            }).ToList()
        };
    }
}
