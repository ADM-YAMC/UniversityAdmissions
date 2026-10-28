namespace UniversityAdmissions.Application.DTOs.Admissions;
public class PublicAdmissionStatusDto
{
    public int AdmissionId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
