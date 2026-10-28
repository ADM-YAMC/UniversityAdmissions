namespace UniversityAdmissions.Application.DTOs.Admissions;
public class ChangeAdmissionStatusDto
{
    public int AdmissionId { get; set; }
    public int NewStatusId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? Comments { get; set; }
}
