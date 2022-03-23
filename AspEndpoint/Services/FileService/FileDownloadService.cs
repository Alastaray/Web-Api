using AspEndpoint.Models;
using FileManagerLibrary;


namespace AspEndpoint.Services
{
    public class FileDownloadService : FileService
    {
        public readonly FileModel fileModel;
        private readonly IFileManager _fileManager;
        public FileDownloadService(DataContext context, IFileManager fileManager) : base(context)
        {
            fileModel = new FileModel();
            _fileManager = fileManager;
    }

        public async Task<string> FileDownloadAsync(string url)
        {
            string path = await _fileManager.UploadByUrlAsync(url);
            
            fileModel.Name = path.Split('/')[^1];
            fileModel.Path = path.Substring(0, path.IndexOf(fileModel.Name));

            if (IsImage(fileModel.Name))
            {
                byte[] file = await _fileManager.ReadBytesAsync(fileModel.Path + fileModel.Name);
                await CutImageAsync(file, 100);
                await CutImageAsync(file, 300);
            }
            await _dataContext.Files.AddAsync(fileModel);
            await _dataContext.SaveChangesAsync();
            return fileModel.Path + fileModel.Name;
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

        private async Task CutImageAsync(byte[] file, int newSize)
        {
            byte[]? newFile = Picture.Cut(file, newSize);
            if (newFile == null) return;
            await _fileManager.WriteBytesAsync(fileModel.Path + newSize + "_" + fileModel.Name, newFile);
        }
    }
}
