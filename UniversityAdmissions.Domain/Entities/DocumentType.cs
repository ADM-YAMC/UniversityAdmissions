namespace UniversityAdmissions.Domain.Entities;
public class DocumentType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<AdmissionDocument> Documents { get; set; } = new List<AdmissionDocument>();
}