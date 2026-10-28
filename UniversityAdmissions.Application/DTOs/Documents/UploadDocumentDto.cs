namespace UniversityAdmissions.Application.DTOs.Documents;
public class UploadDocumentDto
{
    public int AdmissionId { get; set; }
    public int DocumentTypeId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public Stream Content { get; set; } = Stream.Null;
}