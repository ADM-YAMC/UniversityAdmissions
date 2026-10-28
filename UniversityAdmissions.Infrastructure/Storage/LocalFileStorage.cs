using UniversityAdmissions.Application.Interfaces.Storage;
namespace UniversityAdmissions.Infrastructure.Storage;
public class LocalFileStorage : IFileStorage
{
    private readonly string _rootPath;
    public LocalFileStorage(string rootPath)
    {
        _rootPath = Path.GetFullPath(rootPath);
        Directory.CreateDirectory(_rootPath);
    }
    public async Task<string> SaveAsync(Stream content, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var storedFileName = $"{Guid.NewGuid():N}{extension}";
        var path = Path.Combine(_rootPath, storedFileName);
        using var file = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, true);
        await content.CopyToAsync(file);
        return storedFileName;
    }
    public Task<Stream> OpenReadAsync(string storedFileName)
    {
        var path = GetSafePath(storedFileName);
        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true);
        return Task.FromResult(stream);
    }
    public Task DeleteAsync(string storedFileName)
    {
        var path = GetSafePath(storedFileName);
        if (File.Exists(path)) File.Delete(path);
        return Task.CompletedTask;
    }
    private string GetSafePath(string storedFileName)
    {
        if (string.IsNullOrWhiteSpace(storedFileName) || Path.GetFileName(storedFileName) != storedFileName)
            throw new InvalidOperationException("Nombre de archivo inválido");
        var path = Path.GetFullPath(Path.Combine(_rootPath, storedFileName));
        if (!Path.GetDirectoryName(path)!.Equals(_rootPath, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Ruta de archivo inválida");
        return path;
    }
}