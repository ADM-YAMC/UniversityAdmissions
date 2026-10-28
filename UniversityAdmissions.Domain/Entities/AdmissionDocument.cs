namespace UniversityAdmissions.Domain.Entities;
public class AdmissionDocument
{
    public int Id { get; set; }
    public int AdmissionId { get; set; }
    public int DocumentTypeId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StoredFileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public Admission Admission { get; set; } = null!;
    public DocumentType DocumentType { get; set; } = null!;
}