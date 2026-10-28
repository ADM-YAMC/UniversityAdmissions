namespace UniversityAdmissions.Domain.Entities;
public class AdmissionStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
}