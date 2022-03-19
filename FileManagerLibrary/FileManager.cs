using Storage.Net;
using Storage.Net.Blobs;
using System.Configuration;

namespace FileManagerLibrary
{
    public class FileManager : IFileManager
    {
        IBlobStorage _storage;
        public string asd = "da";
        public FileManager()
        {
            StorageFactory.Modules.UseGoogleCloudStorage();
            _storage = StorageFactory.Blobs.FromConnectionString(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
        }
        public async Task<string> UploadByUrlAsync(string url)
        {
            string? configParam = ConfigurationManager.AppSettings["MaxFileSize"]?.Replace('.', ',');
            double maxFileSize = double.Parse(configParam ?? "5");
            if (await GetFileSizeAsync(url) > maxFileSize)
                throw new Exception("File has size than more " + maxFileSize + "MB!");
            HttpClient httpClient = new HttpClient();
            byte[] file = await httpClient.GetByteArrayAsync(url);
            string name = Hasher.CreateHashName(url);
            string path = Hasher.CreateHashPath(name);
            await _storage.WriteAsync(path + name, file);
            return path + name;
        }

        public async Task RemoveFolderAsync(string path)
        {
            var blob = await _storage.GetBlobAsync(path);
            await _storage.DeleteAsync(blob);
        }

        public async Task RemoveFilesAsync(string path)
        {
            if (path != null)
            {
                var filesList = await _storage.ListAsync(path);
                foreach (var fileModel in filesList)
                    await _storage.DeleteAsync(fileModel);
            }
            else
                throw new Exception("Path is empty!");
        }

        public async Task RemoveSubFoldersAsync(string path)
        {
            if (path != null)
            {
                string[]? folders = path.Split('/');
                foreach (var folder in folders)
                {
                    if (folder != "")
                        await _storage.DeleteAsync(folder);
                }
            }
            else
                throw new Exception("Path is empty!");
        }

        public async Task<double> GetFileSizeAsync(string url)
        {
            HttpClient webRequest = new HttpClient();
            var webResponse = await webRequest.GetAsync(url);
            string[] fileSizeBytes = (string[])webResponse.Content.Headers.GetValues("Content-Length");
            return Math.Round(Convert.ToDouble(fileSizeBytes[0]) / 1024.0 / 1024.0, 2);
        }

        public async Task WriteBytesAsync(string path, byte[] file)
        {
            await _storage.WriteAsync(path, file);
        }

        public async Task<byte[]> ReadBytesAsync(string path)
        {
            return await _storage.ReadBytesAsync(path);
        }

        public void Dispose()
        {
            _storage.Dispose();
        }

    }
}
