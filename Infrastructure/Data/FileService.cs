
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FileUpload.Services
{
    public class FileService : IFileService
    {
        private readonly StoreContext _Context;

        public FileService(StoreContext context)
        {
            this._Context = context;
        }

        public async Task<FileDetails> PostFileAsync(IFormFile fileData, FileType fileType)
        {
            using (var transaction = await _Context.Database.BeginTransactionAsync())
            {
                try
                {
                    var fileDetails = SaveFile(fileData);
                    using (var stream = new MemoryStream())
                    {
                        fileData.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }
                    var result = _Context.FileDetails.Add(fileDetails);
                    await _Context.SaveChangesAsync();

                    return fileDetails;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        // public async Task<FileResult> GetFile(int id) 
        // {        
        //     var fileDetails = await _Context.FileDetails
        //     .SingleOrDefaultAsync(f => f.Id == id);

        //     if(fileDetails == null) {
        //         return NotFound();
        //     }

        // var memoryStream = new MemoryStream(fileDetails.FileData);

        // return File(memoryStream, GetMimeType(fileDetails), 
        //     fileName: fileDetails.FileName);

        // }

        // private string GetMimeType(FileDetails fileDetails) {

        //   // map file extension to mime type
        //   var extension = Path.GetExtension(fileDetails.FileName);

        //   // map extension to mime type
        //   return MimeTypes.GetMimeType(extension);

        // }
        public async Task PostMultiFileAsync(List<FileUploadModel> fileData)
        {
            try
            {
                foreach (FileUploadModel file in fileData)
                {
                    var fileDetails = new FileDetails()
                    {
                        ID = 0,
                        FileName = file.FileDetails.FileName,
                        FileType = file.FileType,
                    };

                    using (var stream = new MemoryStream())
                    {
                        file.FileDetails.CopyTo(stream);
                        // fileDetails.FileData = stream.ToArray();
                    }

                    var result = _Context.FileDetails.Add(fileDetails);
                }
                await _Context.SaveChangesAsync();
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
                var file = _Context.FileDetails.Where(x => x.ID == Id).FirstOrDefaultAsync();

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


            var folderName = "Resources/Images/new";
            var filePath = GetFilePath(folderName, file);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            var fileDetails = new FileDetails();
            fileDetails.FileName = GetFileName(file);
            fileDetails.FileType = GetFileType(file);
            fileDetails.FileUrl = filePath;
            return fileDetails;
        }
        private string GetFilePath(string folderName, IFormFile file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            Directory.CreateDirectory(path);
            // Get unique filename
            var filename = Guid.NewGuid() + Path.GetExtension(file.FileName);

            return Path.Combine(path, filename);
        }

        private string GetFileName(IFormFile file)
        {
            return Path.GetFileName(file.FileName);
        }

        // Helper to get file type
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