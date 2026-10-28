namespace UniversityAdmissions.Domain.Entities;
public class Gender
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}