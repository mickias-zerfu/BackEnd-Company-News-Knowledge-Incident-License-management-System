using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface ISharedResourceRepository
    {
        Task<SharedResource> GetSharedResourceByIdAsync(int id);
        Task<IReadOnlyList<SharedResource>> GetSharedResourcesAsync();
        public Task <SharedResource> CreateSharedResourceAsync(SharedResourceUploadModel fileData);
        Task<SharedResource> UpdateSharedResourceAsync(int id, SharedResourceUploadModel fileData);
        Task DeleteSharedResourceAsync(int id);
        public Task<string> DownloadFileById(int id); 
    }
}