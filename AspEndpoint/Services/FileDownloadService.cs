using AspEndpoint.Models;

namespace AspEndpoint.Services
{
    public class FileDownloadService : FileService
    {
        public FileDownloadService(ImageContext context, IConfiguration configuration) : base(context, configuration) { }

        public async Task<string> FileDownloadAsync(string url)
        {
            await CheckFileSizeAsync(url);
            Picture picture = await DownloadAsync(url, Directory.GetCurrentDirectory() + "\\Files");      
            if(IsImage(picture.imageModel.Name))
            {
                await picture.CutAsync(100);
                await picture.CutAsync(300);
            }
            return await SaveToDatabaseAsync(picture.imageModel);
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

        public async Task<string> SaveToDatabaseAsync(ImageModel imageModel)
        {
            await _imageContext.images.AddAsync(imageModel);
            await _imageContext.SaveChangesAsync();
            return imageModel.Path + imageModel.Name;
        }

        public async Task<Picture> DownloadAsync(string url, string Path)
        {
            Picture picture = new Picture();
            if (!await picture.DownloadAsync(url, Path))
                throw new Exception("File already exists!");
            return picture;
        }
    }
}
