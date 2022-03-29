using AspEndpoint.Helpers;
using AspEndpoint.Models;
using FileManagerLibrary;

namespace AspEndpoint.Services.FileService
{
    public class FileService: IFileService
    {
        protected readonly DataContext _dataContext;       
        private readonly IFileManager _fileManager;
        public readonly FileModel fileModel;

        public FileService(DataContext context, IFileManager fileManager)
        {
            fileModel = new FileModel();
            _fileManager = fileManager;
            _dataContext = context;
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

        public async Task<FileModel> GetAsync(int id)
        {
            return await _dataContext.Files.FindNotDeletedAsync(id);
        }

        public bool IsImage(string? fileName)
        {
            if (fileName == null) return false;
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

        public async Task CutImageAsync(byte[] image, int newSize)
        {
            byte[]? newImage = Picture.Cut(image, newSize);
            if (newImage == null) return;
            await _fileManager.WriteBytesAsync(fileModel.Path + newSize + "_" + fileModel.Name, newImage);
        }

        public async Task<string> Remove(int id)
        {
            FileModel fileModel = await _dataContext.Files.FindNotDeletedAsync(id);
            _dataContext.Files.SoftDelete(fileModel);
            await _dataContext.SaveChangesAsync();
            return "Record successfully deleted!";
        }

        public async Task<string> Restore(int id)
        {
            FileModel fileModel = await _dataContext.Files.FindDeletedAsync(id);
            _dataContext.Files.Restore(fileModel);
            await _dataContext.SaveChangesAsync();
            return "Record successfully restored!";
        }
    }
}
