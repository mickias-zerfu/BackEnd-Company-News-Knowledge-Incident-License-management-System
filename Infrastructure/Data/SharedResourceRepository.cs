using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class SharedResourceRepository : ISharedResourceRepository
{
    private readonly StoreContext _Context;

    public SharedResourceRepository(StoreContext Context)
    {
        _Context = Context;
    }

    public async Task<SharedResource> GetSharedResourceByIdAsync(int Id)
    {
        // var file = _Context.FileDetails.Where(x => x.ID == Id).FirstOrDefaultAsync();

        return await _Context.SharedResources.FindAsync(Id);
    }

    public async Task<IReadOnlyList<SharedResource>> GetSharedResourcesAsync()
    {
        return await _Context.SharedResources.ToListAsync();
    }

    public async Task<SharedResource> CreateSharedResourceAsync(SharedResourceUploadModel fileData)
    {
        try
        {
                DateTime today = DateTime.Today;
            var fileDetails = new SharedResource()
            {
                Id = 0,
                FileTitle = fileData.FileTitle,
                FileDescription = fileData.FileDescription,
                FileName = fileData.FileDetails.FileName,
                FileType = GetFileType(fileData.FileDetails),
                Created_at = today.ToString(),
                Updated_at = today.ToString()
            };
            using (var stream = new MemoryStream())
            {
                fileData.FileDetails.CopyTo(stream);
                fileDetails.FileData = stream.ToArray();
            }
            var result = _Context.SharedResources.Add(fileDetails);
            await _Context.SaveChangesAsync();

            return fileDetails;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<SharedResource> UpdateSharedResourceAsync(SharedResource sharedResource)
    {
        _Context.Entry(sharedResource).State = EntityState.Modified;
        await _Context.SaveChangesAsync();
        return sharedResource;
    }

    public async Task DeleteSharedResourceAsync(int id)
    {
        var sharedResource = await _Context.SharedResources.FindAsync(id);
        if (sharedResource != null)
        {
            _Context.SharedResources.Remove(sharedResource);
            await _Context.SaveChangesAsync();
        }
    }

    public async Task DownloadFileById(int Id)
    {
        try
        {
            var file = _Context.FileDetails.Where(x => x.ID == Id).FirstOrDefaultAsync();

            var content = new System.IO.MemoryStream(file.Result.FileData);
            var path = Path.Combine(
               "C:\\Users\\HP\\Desktop\\AGENDAS", "FileDownloaded",
               file.Result.FileName);

            await CopyStream(content, path);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private FileType GetFileType(IFormFile fileData)
    {
        var contentType = fileData.ContentType;
        var fileName = fileData.FileName;

        if (contentType.Contains("pdf") || fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            return FileType.PDF;
        }
        else if (contentType.Contains("docx") || fileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
        {
            return FileType.DOCX;
        }
        else
        {
            // Handle other file types or return a default FileType value
            return FileType.Default;
        }
    }

    private async Task<byte[]> ReadFileDataAsync(IFormFile fileData)
    {
        using (var stream = new MemoryStream())
        {
            await fileData.CopyToAsync(stream);
            return stream.ToArray();
        }
    }
    public async Task CopyStream(Stream stream, string downloadPath)
    {
        using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
        {
            await stream.CopyToAsync(fileStream);
        }
    }

}