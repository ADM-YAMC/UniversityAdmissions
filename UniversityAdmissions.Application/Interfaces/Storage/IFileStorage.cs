namespace UniversityAdmissions.Application.Interfaces.Storage;
public interface IFileStorage
{
    Task<string> SaveAsync(Stream content, string fileName);
    Task<Stream> OpenReadAsync(string storedFileName);
    Task DeleteAsync(string storedFileName);
}