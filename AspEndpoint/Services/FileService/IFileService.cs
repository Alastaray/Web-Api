using AspEndpoint.Models;

namespace AspEndpoint.Services.FileService
{
    public interface IFileService
    {
        public Task<string> FileDownloadAsync(string url);

        public Task CutImageAsync(byte[] file, int newSize);

        public Task<FileModel> GetAsync(int id);

        public bool IsImage(string? fileName);

        public Task<string> Remove(int id);

        public Task<string> Restore(int id);
    }
}
