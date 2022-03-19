using Storage.Net.Blobs;

namespace FileManagerLibrary
{
    public interface IFileManager : IDisposable
    {
        Task<string> UploadByUrlAsync(string url);
        Task RemoveFolderAsync(string path);
        Task RemoveFilesAsync(string path);
        Task RemoveSubFoldersAsync(string path);
        Task WriteBytesAsync(string path, byte[] file);
        Task<byte[]> ReadBytesAsync(string path);
    }
}
