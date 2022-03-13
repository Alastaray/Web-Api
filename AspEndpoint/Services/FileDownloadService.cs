using AspEndpoint.Models;
using Storage.Net;
using Storage.Net.Blobs;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AspEndpoint.Services
{
    public class FileDownloadService : FileService
    {
        protected readonly IBlobStorage _storage;
        public readonly FileModel fileModel;
        public FileDownloadService(FileContext context, IConfiguration configuration) : base(context, configuration) 
        {
            fileModel = new FileModel();
            _storage = StorageFactory.Blobs.FromConnectionString(configuration.GetConnectionString("StorageConnection"));

        }

        public async Task<string> FileDownloadAsync(string url)
        {
            await CheckFileSizeAsync(url);
            await DownloadAsync(url);
            /*if (IsImage(fileModel.Name))
            {
                Picture picture = new Picture(fileModel);
                await picture.CutAsync(100);
                await picture.CutAsync(300);
            }*/
            return await SaveToDatabaseAsync(fileModel);
        }

        public bool IsImage(string? fileName)
        {
            if(fileName == null)return false;
            string[] extensions = {
                ".jpeg", ".png", ".jpg",
                ".tiff", ".raw", ".bmp"
            };
            foreach (string extension in extensions)
            {
                if (fileName.Contains(extension))
                    return true;
            }
            return false;
        }

        public async Task<double> GetFileSizeAsync(string url)
        {
            HttpClient webRequest = new HttpClient();
            var webResponse = await webRequest.GetAsync(url);
            string[] fileSizeBytes = (string[])webResponse.Content.Headers.GetValues("Content-Length");
            return Math.Round(Convert.ToDouble(fileSizeBytes[0]) / 1024.0 / 1024.0, 2);
        }

        public async Task CheckFileSizeAsync(string url)
        {
            int maxFileSize = int.Parse(_config["MaxFileSize"] ?? "5");
            if (await GetFileSizeAsync(url) > maxFileSize)
                throw new Exception("File has size than more " + maxFileSize + "MB!");
        }

        public async Task<string> SaveToDatabaseAsync(FileModel fileModel)
        {
            await _fileContext.files.AddAsync(fileModel);
            await _fileContext.SaveChangesAsync();
            return fileModel.Path + fileModel.Name;
        }

        public async Task DownloadAsync(string url)
        {
            HttpClient httpClient = new HttpClient();
            byte[] file = await httpClient.GetByteArrayAsync(url);
            fileModel.Name = Hasher.CreateHashName(url);
            fileModel.Path = Hasher.CreateHashPath(fileModel.Name);
            await _storage.WriteAsync(fileModel.Path + fileModel.Name, file);
        }

       
    }
}
