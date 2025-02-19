
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace FileUpload.Services
{
    public interface IFileService
    {
        public Task<FileDetails> PostFileAsync(IFormFile fileData);

        public Task PostMultiFileAsync(List<FileUploadModel> fileData);

        public Task DownloadFileById(int fileName);
    }
}