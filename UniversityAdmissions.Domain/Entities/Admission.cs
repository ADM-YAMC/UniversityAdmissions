namespace UniversityAdmissions.Domain.Entities;
public class Admission
{
    public int Id { get; set; }
    public int ApplicantId { get; set; }
    public int AdmissionStatusId { get; set; }
    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
    public bool AcceptTerms { get; set; }
    public DateTime? TermsAcceptedAt { get; set; }
    public Applicant Applicant { get; set; } = null!;
    public AdmissionStatus AdmissionStatus { get; set; } = null!;
    public ICollection<AdmissionDocument> Documents { get; set; } = new List<AdmissionDocument>();
}