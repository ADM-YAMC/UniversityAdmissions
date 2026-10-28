using System.ComponentModel.DataAnnotations;
namespace UniversityAdmissions.Application.DTOs.Admissions;
public class CreateAdmissionDto
{
    [Range(1, int.MaxValue)]
    public int ApplicantId { get; set; }
    [Range(typeof(bool), "true", "true", ErrorMessage = "Debes aceptar los términos y condiciones")]
    public bool AcceptTerms { get; set; }
}