
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FileUpload.Services
{
    public class FileService : IFileService
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
        {".zip", FileType.Archive}
    };
        public FileService(StoreContext context, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<FileDetails> PostFileAsync(IFormFile fileData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var extension = Path.GetExtension(fileData.FileName).ToLowerInvariant();
                    if (!AllowedFileExtensions.ContainsKey(extension))
                    {
                        throw new InvalidOperationException("Unsupported file type.");
                    }
                    if (fileData.FileName.Length >= 25)
                    {
                        throw new InvalidOperationException("FileName length must be letha than 25 chacter.");
                    }
                    var fileDetails = SaveFile(fileData);
                    using (var stream = new MemoryStream())
                    {
                        fileData.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }
                    var result = _context.FileDetails.Add(fileDetails);
                    await _context.SaveChangesAsync();

                    return fileDetails;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task PostMultiFileAsync(List<FileUploadModel> fileData)
        {
            try
            {
                foreach (FileUploadModel file in fileData)
                {
                    var fileDetails = new FileDetails()
                    {
                        ID = 0,
                        FileName = file.FileDetails.FileName
                    };

                    using (var stream = new MemoryStream())
                    {
                        file.FileDetails.CopyTo(stream);
                        // fileDetails.FileData = stream.ToArray();
                    }

                    var result = _context.FileDetails.Add(fileDetails);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DownloadFileById(int Id)
        {
            try
            {
                var file = _context.FileDetails.Where(x => x.ID == Id).FirstOrDefaultAsync();

                var content = new System.IO.MemoryStream(file.Result.FileData);
                var path = Path.Combine(
                   Directory.GetCurrentDirectory(), "FileDownloaded",
                   file.Result.FileName);

                await CopyStream(content, path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }


        private FileDetails SaveFile(IFormFile file)
        {

            var folderName = "Images/new";
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderName, fileName);
            var fullPath = Path.Combine("wwwroot", filePath);
            // Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var fileDetails = new FileDetails
            {
                FileName = fileName,
                FileUrl = Path.Combine(hostUrl, filePath),
                FileType = GetFileType(file)
            };
            return fileDetails;
        }

        private FileType GetFileType(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);

            if (ExtensionMap.ContainsKey(extension))
            {
                return ExtensionMap[extension];
            }

            // default or unknown type
            return FileType.Document;
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
}