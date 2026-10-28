namespace UniversityAdmissions.Application.DTOs.Applicants;
public class ApplicantDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; }
    public string IdentificationNumber { get; set; } = string.Empty;
    public string IdentificationType { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}