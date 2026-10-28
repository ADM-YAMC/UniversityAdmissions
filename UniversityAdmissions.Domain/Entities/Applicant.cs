namespace UniversityAdmissions.Domain.Entities;
public class Applicant
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string IdentificationNumber { get; set; } = string.Empty;
    public int IdentificationTypeId { get; set; }
    public int GenderId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public IdentificationType IdentificationType { get; set; } = null!;
    public Gender Gender { get; set; } = null!;
    public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
}