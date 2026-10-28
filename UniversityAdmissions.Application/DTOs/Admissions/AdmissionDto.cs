using UniversityAdmissions.Application.DTOs.Applicants;
using UniversityAdmissions.Application.DTOs.Documents;
namespace UniversityAdmissions.Application.DTOs.Admissions;
public class AdmissionDto
{
    public int Id { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool AcceptTerms { get; set; }
    public ApplicantDto Applicant { get; set; } = null!;
    public List<AdmissionDocumentDto> Documents { get; set; } = new();
}