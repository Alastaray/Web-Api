using Storage.Net.Blobs;

namespace FileManagerProject
{
    public interface IFileManager : IDisposable
    {
        public IBlobStorage? blobStorage { get; }
        public Task<string> UploadByUrlAsync(string url);
        public Task RemoveFolderAsync(string path);
        public Task RemoveFilesAsync(string path);
        public Task RemoveSubFoldersAsync(string path);
    }
}
