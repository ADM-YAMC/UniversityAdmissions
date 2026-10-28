namespace UniversityAdmissions.Domain.Entities;
public class AdmissionDecision
{
    public int Id { get; set; }
    public int AdmissionId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int PreviousStatusId { get; set; }
    public int NewStatusId { get; set; }
    public string? Comments { get; set; }
    public DateTime DecisionDate { get; set; } = DateTime.UtcNow;
    public Admission Admission { get; set; } = null!;
}