using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;
using Google.Protobuf;

public class SharedResourceRepository : ISharedResourceRepository
{
    private readonly StoreContext _context;

    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly Dictionary<string, FileType> AllowedFileExtensions = new Dictionary<string, FileType>()
    {
        {".jpg", FileType.Image},
        {".png", FileType.Image},
        {".pdf", FileType.Document},
        {".mp4", FileType.Video},
        {".mp3", FileType.Audio},
        {".xlsx", FileType.Document},
        {".docx", FileType.Document},
        {".zip", FileType.Archive} 
    };

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
 
        return sharedResources;
    }
    public async Task<SharedResource> CreateSharedResourceAsync(SharedResourceUploadModel fileData)
    {
        try
        {
            var extension = Path.GetExtension(fileData.FileDetails.FileName).ToLowerInvariant();
            if (!AllowedFileExtensions.ContainsKey(extension))
            {
                throw new InvalidOperationException("Unsupported file type.");
            }

            if (fileData.FileDetails.FileName.Length >= 25)
            {
                throw new InvalidOperationException("FileName length must be lessthan than 25 chacter.");

            }
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileData.FileDetails.FileName;
            var directoryPath = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await fileData.FileDetails.CopyToAsync(fileStream);
            }

            var sharedResource = new SharedResource
            {
                FileTitle = fileData.FileTitle,
                FileDescription = fileData.FileDescription,
                FileName = fileData.FileDetails.FileName,
                FilePath = filePath,
                FileType = AllowedFileExtensions[extension],
                FileUrl = Guid.NewGuid().ToString(),
                Created_at = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                Updated_at = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            _context.SharedResources.Add(sharedResource);
            await _context.SaveChangesAsync();

            return sharedResource;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create shared resource: {ex.Message}");
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

            var filePath = sharedResource.FilePath;

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found on the server.");
                throw new InvalidOperationException("File not found on the server");
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + sharedResource.FileName;
            var tempDirectoryPath = Path.Combine(_environment.WebRootPath, "temp");
            if (!Directory.Exists(tempDirectoryPath))
            {
                Directory.CreateDirectory(tempDirectoryPath);
            }

            var tempFilePath = Path.Combine(tempDirectoryPath, uniqueFileName);
            File.Copy(filePath, tempFilePath, true);

            var hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var fileUrl = Path.Combine(hostUrl, "temp", uniqueFileName);

            return fileUrl;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error downloading file: {ex.Message}");
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
            var extension = Path.GetExtension(fileData.FileDetails.FileName).ToLowerInvariant();
            if (!AllowedFileExtensions.ContainsKey(extension))
            {
                throw new InvalidOperationException("Unsupported file type.");
            }

            if (fileData.FileDetails.FileName.Length >= 25)
            {
                throw new InvalidOperationException("FileName length must be lessthan than 25 chacter.");

            }
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileData.FileDetails.FileName;
            var directoryPath = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await fileData.FileDetails.CopyToAsync(fileStream);
            }

            sharedResourceToUpdate.FileName = fileData.FileDetails.FileName;
            sharedResourceToUpdate.FilePath = filePath;
            sharedResourceToUpdate.FileType = AllowedFileExtensions[extension];
            sharedResourceToUpdate.FileUrl = Guid.NewGuid().ToString();
            sharedResourceToUpdate.Updated_at = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

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
        {".xlsx", FileType.Document},
        {".docx", FileType.Document},
   {".mp3", FileType.Audio}, 
   {".zip", FileType.Archive}
    };
}