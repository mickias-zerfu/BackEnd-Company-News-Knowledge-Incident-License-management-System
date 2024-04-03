using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;

public class SharedResourceRepository : ISharedResourceRepository
{
    private readonly StoreContext _context;

    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SharedResourceRepository(StoreContext context, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<SharedResource> GetSharedResourceByIdAsync(int id)
    {
        var sharedResource = await _context.SharedResources
          .SingleOrDefaultAsync(x => x.Id == id);

        if (sharedResource == null)
        {
            throw new KeyNotFoundException("Shared resource not found");
        }

        return sharedResource;
    }

    public async Task<IReadOnlyList<SharedResource>> GetSharedResourcesAsync()
    {
        var sharedResources = await _context.SharedResources
          .ToListAsync();

        if (sharedResources.Count == 0)
        {
            throw new Exception("No shared resources found");
        }

        return sharedResources;
    }

    public async Task<SharedResource> CreateSharedResourceAsync(SharedResourceUploadModel fileData)
    {
        try
        {
            DateTime today = DateTime.Today;

            // Map SharedResourceUploadModel to SharedResource entity
            var sharedResource = new SharedResource
            {
                FileTitle = fileData.FileTitle,
                FileDescription = fileData.FileDescription,
                FileName = fileData.FileDetails.FileName,
                FileType = GetFileType(fileData.FileDetails), // Assuming GetFileType is implemented
                FileUrl = Guid.NewGuid().ToString(),
                Created_at = today.ToString(),
                Updated_at = today.ToString()
            };

            using (var stream = new MemoryStream())
            {
                fileData.FileDetails.CopyTo(stream);
                sharedResource.FileData = stream.ToArray();
            }

            // Add to context and save changes
            _context.SharedResources.Add(sharedResource);
            await _context.SaveChangesAsync();

            return sharedResource;
        }
        catch (Exception)
        {
            throw; // Handle or log the exception as needed
        }
    }
    public async Task<SharedResource> UpdateSharedResourceAsync(int id, SharedResourceUploadModel fileData)
    {
        var sharedResourceToUpdate = await _context.SharedResources.FindAsync(id);

        if (sharedResourceToUpdate == null)
        {
            throw new Exception("Shared resource not found");
        }

        // Map properties
        sharedResourceToUpdate.FileTitle = fileData.FileTitle;
        sharedResourceToUpdate.FileDescription = fileData.FileDescription;

        if (fileData.FileDetails != null)
        {

            using (var memoryStream = new MemoryStream())
            {
                await fileData.FileDetails.CopyToAsync(memoryStream);
                sharedResourceToUpdate.FileData = memoryStream.ToArray();
            }
        }

        // Save changes
        _context.SharedResources.Update(sharedResourceToUpdate);
        await _context.SaveChangesAsync();

        return sharedResourceToUpdate;
    }

    public async Task DeleteSharedResourceAsync(int id)
    {
        var sharedResource = await _context.SharedResources.FindAsync(id);

        if (sharedResource == null)
        {
            throw new KeyNotFoundException("Shared resource not found");
        }

        _context.SharedResources.Remove(sharedResource);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to delete shared resource: {ex.Message}");
        }
    }
    public async Task<string> DownloadFileById(int id)
    {
        try
        {
            var sharedResource = await _context.SharedResources.FindAsync(id);
            if (sharedResource == null)
            {
                throw new ArgumentException("Shared resource not found");
            }

            var fileName = sharedResource.FileName;
            var fileData = sharedResource.FileData;

            // Create a unique file name for the temporary file
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

            // Get the directory path for storing the temporary file
            var directoryPath = Path.Combine(_environment.WebRootPath, "temp");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, uniqueFileName);
            using (var stream = new MemoryStream(fileData))
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }

            var hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var fileUrlC = Path.Combine(hostUrl, "temp");
            var fileUrl = Path.Combine(fileUrlC, uniqueFileName);

            return fileUrl;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error downloading file: {ex.Message}");
        }
    }

    private FileType GetFileType(IFormFile fileData)
    {
        var extension = Path.GetExtension(fileData.FileName);

        if (ExtensionMap.ContainsKey(extension))
        {
            return ExtensionMap[extension];
        }

        // default or unknown type
        return FileType.Document;
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
    private static Dictionary<string, FileType> ExtensionMap =
      new Dictionary<string, FileType>()
    {
   {".jpg", FileType.Image},
   {".png", FileType.Image},
   {".pdf", FileType.Document},
   {".mp4", FileType.Video},
   {".mp3", FileType.Audio},
   {".exe", FileType.Software},
   {".zip", FileType.Archive}
    };
}