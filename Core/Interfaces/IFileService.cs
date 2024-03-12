
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace FileUpload.Services
{
    public interface IFileService
    {
        public Task<string> PostFileAsync(IFormFile fileData, FileType fileType);

        public Task PostMultiFileAsync(List<FileUploadModel> fileData);

        public Task DownloadFileById(int fileName);
    }
}