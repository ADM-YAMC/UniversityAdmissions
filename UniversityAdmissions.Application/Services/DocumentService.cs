using UniversityAdmissions.Application.DTOs.Documents;
using UniversityAdmissions.Application.Interfaces.Repositories;
using UniversityAdmissions.Application.Interfaces.Services;
using UniversityAdmissions.Application.Interfaces.Storage;
using UniversityAdmissions.Application.Responses;
using UniversityAdmissions.Domain.Entities;
using UniversityAdmissions.Domain.Exceptions;
namespace UniversityAdmissions.Application.Services;
public class DocumentService : IDocumentService
{
    private readonly IAdmissionDocumentRepository _repository;
    private readonly IAdmissionRepository _admissions;
    private readonly IFileStorage _storage;
    private const int MaxFileSize = 10 * 1024 * 1024;
    public DocumentService(IAdmissionDocumentRepository repository, IAdmissionRepository admissions, IFileStorage storage)
    {
        _repository = repository;
        _admissions = admissions;
        _storage = storage;
    }
    public async Task<Response<AdmissionDocumentDto>> UploadAsync(UploadDocumentDto dto)
    {
        var response = new Response<AdmissionDocumentDto>();
        try
        {
            var document = await SaveDocumentAsync(dto);
            response.Success = true;
            response.Identity = document.Id;
            response.SingleData = Map(document);
            response.Messages!.Add("Documento cargado exitosamente");
        }
        catch (BusinessException ex)
        {
            response.Errors!.Add(ex.Message);
        }
        catch (Exception)
        {
            response.Errors!.Add("Ocurrió un error al cargar el documento");
        }
        return response;
    }
    public async Task<Response<AdmissionDocumentDto>> GetByAdmissionIdAsync(int admissionId)
    {
        var response = new Response<AdmissionDocumentDto>();
        try
        {
            var documents = await _repository.GetByAdmissionIdAsync(admissionId);
            response.Success = true;
            response.DataList = documents.Select(Map).ToList();
        }
        catch (Exception)
        {
            response.Errors!.Add("Ocurrió un error al consultar los documentos");
        }
        return response;
    }
    public async Task<Response<DocumentDownloadDto>> DownloadAsync(int documentId)
    {
        var response = new Response<DocumentDownloadDto>();
        try
        {
            var document = await _repository.GetByIdAsync(documentId);
            if (document == null)
            {
                response.Errors!.Add("El documento no existe");
                return response;
            }
            var stream = await _storage.OpenReadAsync(document.StoredFileName);
            response.Success = true;
            response.SingleData = new DocumentDownloadDto
            {
                Content = stream,
                ContentType = document.ContentType,
                FileName = document.FileName
            };
        }
        catch (Exception)
        {
            response.Errors!.Add("Ocurrió un error al descargar el documento");
        }
        return response;
    }
    private static AdmissionDocumentDto Map(AdmissionDocument document)
    {
        return new AdmissionDocumentDto
        {
            Id = document.Id,
            DocumentTypeId = document.DocumentTypeId,
            DocumentType = document.DocumentType?.Name ?? "",
            FileName = document.FileName,
            ContentType = document.ContentType,
            FileSize = document.FileSize,
            UploadedAt = document.UploadedAt
        };
    }
    private async Task<AdmissionDocument> SaveDocumentAsync(UploadDocumentDto dto)
    {
        if (await _admissions.GetByIdAsync(dto.AdmissionId) == null)
            throw new BusinessException("La solicitud no existe");
        if (dto.DocumentTypeId <= 0)
            throw new BusinessException("Selecciona un tipo de documento");
        var fileName = Path.GetFileName(dto.FileName);
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (!new[] { ".jpg", ".jpeg", ".png", ".pdf" }.Contains(extension))
            throw new BusinessException("Solo se permiten JPG, PNG y PDF");
        using var buffer = new MemoryStream();
        var chunk = new byte[81920];
        int read;
        while ((read = await dto.Content.ReadAsync(chunk)) > 0)
        {
            if (buffer.Length + read > MaxFileSize)
                throw new BusinessException("El archivo supera los 10 MB");
            await buffer.WriteAsync(chunk.AsMemory(0, read));
        }
        if (buffer.Length == 0)
            throw new BusinessException("El archivo está vacío");
        var bytes = buffer.ToArray();
        var contentType = DetectContentType(bytes);
        var expectedType = extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".pdf" => "application/pdf",
            _ => ""
        };
        if (contentType == null || contentType != expectedType)
            throw new BusinessException("El contenido del archivo no es válido");
        buffer.Position = 0;
        var storedFileName = await _storage.SaveAsync(buffer, fileName);
        try
        {
            return await _repository.AddAsync(new AdmissionDocument
            {
                AdmissionId = dto.AdmissionId,
                DocumentTypeId = dto.DocumentTypeId,
                FileName = fileName,
                StoredFileName = storedFileName,
                FilePath = storedFileName,
                ContentType = contentType,
                FileSize = buffer.Length
            });
        }
        catch
        {
            await _storage.DeleteAsync(storedFileName);
            throw;
        }
    }
    private static string? DetectContentType(byte[] bytes)
    {
        if (bytes.Length >= 8 && bytes.AsSpan(0, 8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }))
            return "image/png";
        if (bytes.Length >= 3 && bytes[0] == 255 && bytes[1] == 216 && bytes[2] == 255)
            return "image/jpeg";
        if (bytes.Length >= 5 && bytes.AsSpan(0, 5).SequenceEqual("%PDF-"u8))
            return "application/pdf";
        return null;
    }
}