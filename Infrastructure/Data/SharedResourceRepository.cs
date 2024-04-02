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
                FileType = fileData.FileType,
                // FileType = GetFileType(fileData.FileDetails),
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

    public async Task<SharedResource> UpdateSharedResourceAsync(int id, SharedResourceUploadModel fileData)
    {
        try
        {
            if (fileData == null)
            {
                throw new ArgumentNullException(nameof(fileData), "File data is null.");
            }

            DateTime today = DateTime.Today;
            var existingResource = await _Context.SharedResources.FindAsync(id);
            if (existingResource == null)
            {
                // Resource with the specified ID not found
                return null;
            }

            // Update the existing resource properties
            existingResource.FileTitle = fileData.FileTitle ?? existingResource.FileTitle; // Use existing title if new title is null
            existingResource.FileDescription = fileData.FileDescription;
            existingResource.FileName = fileData.FileDetails != null ? fileData.FileDetails.FileName : existingResource.FileName;
            existingResource.FileType = fileData.FileType;
            existingResource.Updated_at = today.ToString();

            // Update file data if provided
            if (fileData.FileDetails != null)
            {
                using (var stream = new MemoryStream())
                {
                    fileData.FileDetails.CopyTo(stream);
                    existingResource.FileData = stream.ToArray();
                }
            }

            // Save changes to the database
            await _Context.SaveChangesAsync();

            return existingResource;
        }
        catch (Exception)
        {
            throw;
        }
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

    public async Task<String> DownloadFileById(int Id)
    {
        try
        {
            var file = await _Context.SharedResources.FindAsync(Id);

            if (file == null)
            {
                // Handle the case when the file is not found
                return "File not found";
            }

            var content = new System.IO.MemoryStream(file.FileData);
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources/FileDownloaded");

            // Create the directory if it does not exist
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, file.FileName);

            await CopyStream(content, filePath);
             return filePath;
        }
        catch (Exception)
        {
            throw;
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