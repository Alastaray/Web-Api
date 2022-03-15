using AspEndpoint.Models;
using FileManagerProject;
using Storage.Net.Blobs;


namespace AspEndpoint.Services
{
    public class FileDownloadService : FileService
    {
        public readonly FileModel fileModel;
        public FileDownloadService(FileContext context, IFileManager fileManager) : base(context, fileManager)
        {
            fileModel = new FileModel();
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
            await _fileContext.files.AddAsync(fileModel);
            await _fileContext.SaveChangesAsync();
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
