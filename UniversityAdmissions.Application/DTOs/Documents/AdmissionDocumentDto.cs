namespace UniversityAdmissions.Application.DTOs.Documents;
public class AdmissionDocumentDto
{
    public int Id { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
}