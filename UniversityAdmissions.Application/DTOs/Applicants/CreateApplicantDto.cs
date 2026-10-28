using System.ComponentModel.DataAnnotations;
namespace UniversityAdmissions.Application.DTOs.Applicants;
public class CreateApplicantDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateTime? DateOfBirth { get; set; }
    [Required(ErrorMessage = "El DNI es obligatorio")]
    [StringLength(30)]
    public string IdentificationNumber { get; set; } = string.Empty;
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione el tipo de identificació")]
    public int IdentificationTypeId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione su genero")]
    public int GenderId { get; set; }
    [Required(ErrorMessage = "El número de teléfono es obligatorio")]
    public string PhoneNumber { get; set; } = string.Empty;
}