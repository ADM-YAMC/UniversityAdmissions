using System.ComponentModel.DataAnnotations;
using UniversityAdmissions.Application.DTOs.Applicants;
using UniversityAdmissions.Application.Interfaces.Repositories;
using UniversityAdmissions.Application.Interfaces.Services;
using UniversityAdmissions.Application.Responses;
using UniversityAdmissions.Domain.Entities;
namespace UniversityAdmissions.Application.Services;
public class ApplicantService : IApplicantService
{
    private readonly IApplicantRepository _repository;
    public ApplicantService(IApplicantRepository repository)
    {
        _repository = repository;
    }
    public async Task<Response<ApplicantDto>> CreateAsync(CreateApplicantDto dto)
    {
        var response = new Response<ApplicantDto>();
        try
        {
            Validator.ValidateObject(dto, new ValidationContext(dto), true);
            var birthDate = dto.DateOfBirth!.Value.Date;
            if (birthDate > DateTime.UtcNow.Date || birthDate.Year < 1900)
            {
                response.Errors!.Add("La fecha de nacimiento no es válida...");
                return response;
            }
            var identification = dto.IdentificationNumber.Trim().ToUpperInvariant();
            var existing = await _repository.GetByIdentificationAsync(dto.IdentificationTypeId, identification);
            if (existing != null)
            {
                response.Errors!.Add("Ya existe un solicitante con esta identificación...");
                return response;
            }
            var applicant = new Applicant
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                DateOfBirth = birthDate,
                IdentificationNumber = identification,
                IdentificationTypeId = dto.IdentificationTypeId,
                GenderId = dto.GenderId,
                PhoneNumber = dto.PhoneNumber.Trim()
            };
            applicant = await _repository.AddAsync(applicant);
            response.Success = true;
            response.Identity = applicant.Id;
            response.SingleData = Map(applicant);
            response.Messages!.Add("Solicitante registrado exitosamente...");
        }
        catch (ValidationException ex)
        {
            response.Errors!.Add(ex.Message);
        }
        catch (Exception)
        {
            response.Errors!.Add("Ocurrió un error al registrar el solicitante...");
        }
        return response;
    }
    public async Task<Response<ApplicantDto>> GetByIdAsync(int id)
    {
        var response = new Response<ApplicantDto>();
        try
        {
            var applicant = await _repository.GetByIdAsync(id);
            if (applicant == null)
            {
                response.Errors!.Add("No se encontró el solicitante");
                return response;
            }
            response.Success = true;
            response.SingleData = Map(applicant);
            response.Identity = applicant.Id;
        }
        catch (Exception)
        {
            response.Errors!.Add("Ocurrió un error al consultar el solicitante");
        }
        return response;
    }
    public async Task<Response<ApplicantDto>> GetAllAsync()
    {
        var response = new Response<ApplicantDto>();
        try
        {
            var applicants = await _repository.GetAllAsync();
            response.Success = true;
            response.DataList = applicants.Select(Map).ToList();
            response.Messages!.Add("Consulta realizada exitosamente");
        }
        catch (Exception)
        {
            response.Errors!.Add("Ocurrió un error al consultar los solicitantes");
        }
        return response;
    }
    private static ApplicantDto Map(Applicant applicant)
    {
        var today = DateTime.UtcNow.Date;
        var age = today.Year - applicant.DateOfBirth.Year;
        if (applicant.DateOfBirth.Date > today.AddYears(-age)) age--;
        return new ApplicantDto
        {
            Id = applicant.Id,
            FirstName = applicant.FirstName,
            LastName = applicant.LastName,
            DateOfBirth = applicant.DateOfBirth,
            Age = age,
            IdentificationNumber = applicant.IdentificationNumber,
            IdentificationType = applicant.IdentificationType?.Name ?? "",
            Gender = applicant.Gender?.Name ?? "",
            PhoneNumber = applicant.PhoneNumber
        };
    }
}