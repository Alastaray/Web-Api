using AspEndpoint.Models;
using Storage.Net.Blobs;


namespace AspEndpoint.Services
{
    public class FileDownloadService : FileService
    {
        public readonly FileModel fileModel;
        public FileDownloadService(FileContext context, IConfiguration configuration) : base(context, configuration) 
        {
            fileModel = new FileModel();
        }

        public async Task<string> FileDownloadAsync(string url)
        {
            await CheckFileSizeAsync(url);
            await DownloadAsync(url);
            if (IsImage(fileModel.Name))
            {
                byte[] file = await _storage.ReadBytesAsync(fileModel.Path + fileModel.Name);
                await CutImageAsync(file, 100);
                await CutImageAsync(file, 300);
            }
            return await SaveToDatabaseAsync();
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

        private async Task<string> SaveToDatabaseAsync()
        {
            await _fileContext.files.AddAsync(fileModel);
            await _fileContext.SaveChangesAsync();
            return fileModel.Path + fileModel.Name;
        }

        private async Task DownloadAsync(string url)
        {
            HttpClient httpClient = new HttpClient();
            byte[] file = await httpClient.GetByteArrayAsync(url);
            fileModel.Name = Hasher.CreateHashName(url);
            fileModel.Path = Hasher.CreateHashPath(fileModel.Name);
            await _storage.WriteAsync(fileModel.Path + fileModel.Name, file);
        }

        private async Task CutImageAsync(byte[] file, int newSize)
        {
            byte[]? newFile = Picture.Cut(file, newSize);
            if (newFile == null) return;
            await _storage.WriteAsync(fileModel.Path + newSize + "_" + fileModel.Name, newFile);
        }
    }
}
